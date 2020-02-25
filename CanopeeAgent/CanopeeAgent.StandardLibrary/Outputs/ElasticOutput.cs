using System;
using System.Collections.Generic;
using System.Composition;
using CanopeeAgent.Common;
using Elasticsearch.Net;
using Microsoft.Extensions.Configuration;
using Nest;
using System.Text.Json;

namespace CanopeeAgent.StandardIndicators.Outputs
{
    [Export("Elastic",typeof(IOutput))]
    public class ElasticOutput : IOutput
    {
        private Dictionary<Type, string> _indexesByType;
        private string _url;
        private ElasticClient _client;

        public void SendToOutput(ICollectedEvent collectedEvent)
        {
            string serializedEvent = JsonSerializer.Serialize(collectedEvent, collectedEvent.GetType());
            var index = _indexesByType[collectedEvent.GetType()]; 
            var r = _client.LowLevel.Index<IndexResponse>(index,
                PostData.String(serializedEvent));
            if(!r.IsValid)
            {
                Console.WriteLine(r);
            }
        }

        public void Initialize(IConfiguration configurationOutput)
        {
            _indexesByType = new Dictionary<Type, string>();
            _url = configurationOutput["Url"];
            
            var uri = new Uri(_url);
            var settings = new ConnectionSettings(uri);
            _client = new ElasticClient(settings);
            
            //create index if it doesn't exists
            foreach (var indexConfig in configurationOutput.GetSection("Indexes").GetChildren())
            {
                _indexesByType.Add(Type.GetType(indexConfig["InfosType"], true), indexConfig["Index"]);
            }
            
            foreach (var indexByType in _indexesByType)
            {
                var index = indexByType.Value;
                if (!_client.Indices.Exists(new IndexExistsDescriptor(index)).Exists)
                {
                    //TODO: Manage configuration for index on creation
                    var result = _client.Indices.Create(index);
                    if (!result.Acknowledged)
                    {
                        throw new ApplicationException(result.DebugInformation);
                    }
                }    
            }
            
        }
    }
}