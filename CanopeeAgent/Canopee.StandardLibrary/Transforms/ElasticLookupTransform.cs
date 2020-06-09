using Canopee.Common;
using Elasticsearch.Net;
using Microsoft.Extensions.Configuration;
using Nest;
using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Text;
using Canopee.Common.Pipelines.Events;
using Canopee.Core.Pipelines;
using ITransform = Canopee.Common.Pipelines.ITransform;

namespace Canopee.StandardLibrary.Transforms
{
    /// <summary>
    /// This transforms will make a lookup for a specific key field in an ElasticSearch recordset and add expected field in the <see cref="ICollectedEvent"/>
    ///
    /// The configuration will be :
    ///
    /// <example>
    /// <code>
    ///
    ///     {
    ///         ...
    ///         "Canopee": {
    ///             ...
    ///                 "Pipelines": [
    ///                  ...   
    ///                   {
    ///                     "Name": "OS",
    ///                     ...
    ///                     "Transforms" : [
    ///                         {
    ///                            "TransformType": "ElasticVLookup",
    ///                            "Url": "https://myelasticsearchserver:9200",
    ///                            "SearchedIndex": "MySearchIndex", 
    ///                            "SelectStatement": "SELECT * FROM userinfos",
    ///                            "Key" : {
    ///                                 "LocalName":"KeyNameInCollectedEvent",
    ///                                 "SearchedName":"KeyNameInRecordSet"
    ///                             }, 
    ///                            "Fields": [
    ///                            {
    ///                                "LocalName": "Field1NewName",
    ///                                "SearchedName": "Field1"
    ///                            },
    ///                            {
    ///                                "LocalName": "Field2NewName",
    ///                                "SearchedName": "Field2"
    ///                            }
    ///                         }
    ///                     ]
    ///                  ...
    ///                 }
    ///                 ...   
    ///                 ]
    ///             ...
    ///         }
    ///     }    /// 
    /// </code>
    /// </example>
    ///
    /// The TransformType attribute will be ElasticVLookup
    /// The Url attribute will contain the url of the elasticsearch server
    /// The SearchedIndex attribute will contain the name of the index to search recordset
    /// The Key attribute will contain a mapping definition for the key field : LocalName is the name of the key in the <see cref="ICollectedEvent"/>, SearchedName is the name of the key in the record set 
    /// The Fields attribute will contain all mappings for each wanted field : LocalName attribute will define the name of the field in the <see cref="ICollectedEvent"/>, SearchedName will define the name of the field in the recordset from the select statement
    ///  
    /// </summary>
    [Export("ElasticVLookup", typeof(ITransform))]
    public class ElasticLookupTransform : BaseTransform
    {
        /// <summary>
        /// The elastic search server url
        /// </summary>
        private string _elasticUrl;
        
        /// <summary>
        /// The name of the index to look in
        /// </summary>
        private string _searchedIndex;
        
        /// <summary>
        /// the ElasticClient used to connect and search for
        /// </summary>
        private ElasticClient _client;
        
        /// <summary>
        /// The key of the search
        /// </summary>
        private TransformFieldMapping _key;
        
        /// <summary>
        /// The list of field to be added/modified in the <see cref="ICollectedEvent"/>
        /// </summary>
        private readonly List<TransformFieldMapping> _requestedFieldMappings;

        /// <summary>
        /// Default constructor. Initialize <see cref="_requestedFieldMappings"/>
        /// </summary>
        public ElasticLookupTransform()
        {
            _requestedFieldMappings = new List<TransformFieldMapping>();
        }
        
        /// <summary>
        /// Initialize this <see cref="ITransform"/> with configurations. Add all defined <see cref="TransformFieldMapping"/> from the configuration.
        /// </summary>
        /// <param name="transformConfiguration">the transform configuration </param>
        /// <param name="loggingConfiguration">the logger configuration</param>
        public override void Initialize(IConfigurationSection transformConfiguration, IConfigurationSection loggingConfiguration)
        {
            base.Initialize(transformConfiguration, loggingConfiguration);
            _elasticUrl = transformConfiguration["Url"];
            _searchedIndex = transformConfiguration["SearchedIndex"];
            var uri = new Uri(_elasticUrl);
            var settings = new ConnectionSettings(uri).EnableDebugMode();
            _client = new ElasticClient(settings);
            _key = new TransformFieldMapping()
            {
                LocalName = transformConfiguration["Key:LocalName"],
                SearchedName = transformConfiguration["Key:SearchedName"]
            };
            foreach(var field in transformConfiguration.GetSection("Fields").GetChildren())
            {
                _requestedFieldMappings.Add(new TransformFieldMapping()
                {
                    LocalName = field["LocalName"],
                    SearchedName = field["SearchedName"]
                });
            }
        }

        /// <summary>
        /// Add new fields from the SelectStatement to a collected event
        /// </summary>
        /// <param name="collectedEventToTransform">the <see cref="ICollectedEvent"/> to modify</param>
        /// <returns>the <see cref="ICollectedEvent"/> to enrich</returns>
        public override ICollectedEvent Transform(ICollectedEvent collectedEventToTransform)
        {
            try
            {
                object keyValue = null;
                if(collectedEventToTransform.GetType().GetProperty(_key.LocalName) != null)
                {
                    keyValue = collectedEventToTransform.GetType().GetProperty(_key.LocalName).GetValue(collectedEventToTransform);
                } 
                else if (collectedEventToTransform.ExtractedFields.ContainsKey(_key.LocalName))
                {
                    keyValue = collectedEventToTransform.ExtractedFields[_key.LocalName];
                }
                else
                {
                    throw new MissingMethodException($"Property {_key.LocalName} not found in type {collectedEventToTransform.GetType().ToString()}");
                }
                var response = _client.Search<dynamic>(sd => sd
                    .Index(_searchedIndex)
                    .Sort(s => s
                        .Descending("EventDate"))
                    .Query(q => q
                        .Match( s => s
                            .Field(_key.SearchedName)
                            .Query(keyValue.ToString())
                            )
                        )
                    .Source(sf => sf
                        .Includes(i => i
                            .Fields(_requestedFieldMappings.Select(m => m.SearchedName).ToArray())
                            )
                        )
                    .Size(1)
                );
                foreach(var document in response.Documents)
                {
                    var inputProperties = collectedEventToTransform.GetType().GetProperties();
                    foreach(var prop in document.Keys)
                    {
                        var destinationPropertyName = _requestedFieldMappings.First(p => p.SearchedName == prop).LocalName;
                        if(inputProperties.Any(p => p.Name == destinationPropertyName))
                        {
                            inputProperties.First(p => p.Name == destinationPropertyName).SetValue(collectedEventToTransform, document[prop]);
                        } 
                        else
                        {
                            if (collectedEventToTransform.ExtractedFields.ContainsKey(destinationPropertyName))
                            {
                                collectedEventToTransform.ExtractedFields[destinationPropertyName] = document[prop];
                            } else
                            {
                                collectedEventToTransform.ExtractedFields.Add(destinationPropertyName, document[prop]);
                            }
                        }
                    }
                }
                return collectedEventToTransform;
            }
            catch (Exception ex)
            {
                Logger.LogError($"Error while transforming : {ex}");
                throw;
            }
        }
    }
}
