using Canopee.Common;
using Elasticsearch.Net;
using Microsoft.Extensions.Configuration;
using Nest;
using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Text;
using Canopee.Core.Pipelines;
using ITransform = Canopee.Common.ITransform;

namespace Canopee.StandardLibrary.Transforms
{
    [Export("ElasticVLookup", typeof(ITransform))]
    public class ElasticLookupTransform : BaseTransform
    {
        private string _elasticUrl;
        private string _searchedIndex;
        private ElasticClient _client;
        private TransformFieldMapping _key;
        private List<TransformFieldMapping> _requestedFieldMappings;

        public ElasticLookupTransform()
        {
            _requestedFieldMappings = new List<TransformFieldMapping>();
        }
        public override void Initialize(IConfigurationSection transformConfiguration)
        {
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

        public override ICollectedEvent Transform(ICollectedEvent input)
        {
            try
            {
                object keyValue = null;
                if(input.GetType().GetProperty(_key.LocalName) != null)
                {
                    keyValue = input.GetType().GetProperty(_key.LocalName).GetValue(input);
                } 
                else if (input.ExtractedFields.ContainsKey(_key.LocalName))
                {
                    keyValue = input.ExtractedFields[_key.LocalName];
                }
                else
                {
                    throw new MissingMethodException($"Property {_key.LocalName} not found in type {input.GetType().ToString()}");
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
                    var inputProperties = input.GetType().GetProperties();
                    foreach(var prop in document.Keys)
                    {
                        var destinationPropertyName = _requestedFieldMappings.First(p => p.SearchedName == prop).LocalName;
                        if(inputProperties.Any(p => p.Name == destinationPropertyName))
                        {
                            inputProperties.First(p => p.Name == destinationPropertyName).SetValue(input, document[prop]);
                        } 
                        else
                        {
                            if (input.ExtractedFields.ContainsKey(destinationPropertyName))
                            {
                                input.ExtractedFields[destinationPropertyName] = document[prop];
                            } else
                            {
                                input.ExtractedFields.Add(destinationPropertyName, document[prop]);
                            }
                        }
                    }
                }
                return input;
            }
            catch (Exception ex)
            {
                Logger.LogError($"Error while transforming : {ex}");
                throw;
            }
        }
    }
}
