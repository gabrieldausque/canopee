using System;
using System.Collections.Generic;
using System.Composition;
using Canopee.Common;
using Elasticsearch.Net;
using Microsoft.Extensions.Configuration;
using Nest;
using System.Text.Json;
using Canopee.Core.Pipelines;
using Microsoft.Extensions.Logging;

namespace Canopee.StandardLibrary.Outputs
{
    [Export("Elastic",typeof(IOutput))]
    public class ElasticOutput : BaseOutput
    {
        public ElasticOutput()
        {
            _indexesByType = new Dictionary<string, string>();
        }
        
        private Dictionary<string, string> _indexesByType;
        private string _url;
        private ElasticClient _client;
        private string _defaultIndex;

        public override void SendToOutput(ICollectedEvent collectedEvent)
        {
            try
            {
                Logger.LogDebug($"Sending datas to {_url}");
                string serializedEvent = JsonSerializer.Serialize(collectedEvent, collectedEvent.GetType());
                var index = GetIndexByType(collectedEvent.GetEventType()); 
                var r = _client.LowLevel.Index<IndexResponse>(index,
                    PostData.String(serializedEvent));
                if(!r.IsValid)
                {
                    throw new ApplicationException(r.DebugInformation);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError($"Error while sending output to ElasticSearch : {ex}");
                throw;
            }

        }

        private string GetIndexByType(string eventTypeFullName)
        {
            return (_indexesByType.ContainsKey(eventTypeFullName)) ? _indexesByType[eventTypeFullName] : _defaultIndex;
        }

        public override void Initialize(IConfiguration configurationOutput)
        {
            _defaultIndex = configurationOutput["DefaultIndex"];
            _url = configurationOutput["Url"];
            
            var uri = new Uri(_url);
            var settings = new ConnectionSettings(uri);
            _client = new ElasticClient(settings);
            
            foreach (var indexConfig in configurationOutput.GetSection("Indexes").GetChildren())
            {
                _indexesByType.Add(indexConfig["InfosType"], indexConfig["Index"]);
            }

            try
            {
                foreach (var indexByType in _indexesByType)
                {
                    var index = indexByType.Value;
                    if (!_client.Indices.Exists(new IndexExistsDescriptor(index)).Exists)
                    {
                        var result = _client.Indices.Create(index);
                        if (!result.Acknowledged)
                        {
                            throw new ApplicationException($"Error creating index {index} : {result.DebugInformation}");
                        }
                    }    
                }
            }
            catch (Exception ex)
            {
                Logger.LogError($"Error while creating indexes {ex}");
                throw;
            }
        }
    }
}