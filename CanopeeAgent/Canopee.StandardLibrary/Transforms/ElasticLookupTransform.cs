﻿using Canopee.Common;
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
    [Export("ElasticVLookup", typeof(ITransform))]
    public class ElasticLookupTransform : BaseTransform
    {
        private string _elasticUrl;
        private string _searchedIndex;
        private ElasticClient _client;
        private TransformFieldMapping _key;
        private readonly List<TransformFieldMapping> _requestedFieldMappings;

        public ElasticLookupTransform()
        {
            _requestedFieldMappings = new List<TransformFieldMapping>();
        }
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
