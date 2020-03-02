using System;
using System.Collections.Generic;
using System.Composition;
using Canopee.Common;
using Elasticsearch.Net;
using Microsoft.Extensions.Configuration;
using Nest;
using System.Text.Json;

namespace Canopee.StandardLibrary.Outputs
{
    [Export("Elastic",typeof(IOutput))]
    public class ElasticOutput : IOutput
    {
        public ElasticOutput()
        {
            _indexesByType = new Dictionary<string, string>();
        }
        
        private Dictionary<string, string> _indexesByType;
        private string _url;
        private ElasticClient _client;
        private string _defaultIndex;

        public void SendToOutput(ICollectedEvent collectedEvent)
        {
            string serializedEvent = JsonSerializer.Serialize(collectedEvent, collectedEvent.GetType());
            var index = GetIndexByType(collectedEvent.GetEventType()); 
            var r = _client.LowLevel.Index<IndexResponse>(index,
                PostData.String(serializedEvent));
            if(!r.IsValid)
            {
                Console.WriteLine(r);
                Console.WriteLine(r.ServerError);
            }
        }

        private string GetIndexByType(string eventTypeFullName)
        {
            return (_indexesByType.ContainsKey(eventTypeFullName)) ? _indexesByType[eventTypeFullName] : _defaultIndex;
        }

        public void Initialize(IConfiguration configurationOutput)
        {
            _defaultIndex = configurationOutput["DefaultIndex"];
            _url = configurationOutput["Url"];
            
            var uri = new Uri(_url);
            var settings = new ConnectionSettings(uri);
            _client = new ElasticClient(settings);
            
            //create index if it doesn't exists
            foreach (var indexConfig in configurationOutput.GetSection("Indexes").GetChildren())
            {
                _indexesByType.Add(indexConfig["InfosType"], indexConfig["Index"]);
            }
            
            foreach (var indexByType in _indexesByType)
            {
                var index = indexByType.Value;
                if (!_client.Indices.Exists(new IndexExistsDescriptor(index)).Exists)
                {
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