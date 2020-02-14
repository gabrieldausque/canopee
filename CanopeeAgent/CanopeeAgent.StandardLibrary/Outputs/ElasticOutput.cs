using System;
using System.Collections.Generic;
using System.Composition;
using CanopeeAgent.Common;
using Elasticsearch.Net;
using Nest;
using Newtonsoft.Json;

namespace CanopeeAgent.StandardIndicators.Outputs
{
    [Export("Elastic",typeof(IOutput))]
    public class ElasticOutput : IOutput
    {
        private string _index;
        private string _url;
        private ElasticClient _client;

        public void SendToOutput(ICollectedEvent collectedEvent)
        {
            string serializedEvent = JsonConvert.SerializeObject(collectedEvent); 
            var r = _client.LowLevel.Index<IndexResponse>(_index,
                PostData.String(serializedEvent));
            Console.WriteLine(serializedEvent);
            Console.WriteLine(r);
        }

        public void Initialize(Dictionary<string, string> configurationOutput)
        {
            _index = configurationOutput["Index"];
            _url = configurationOutput["Url"];
            
            var uri = new Uri(_url);
            var settings = new ConnectionSettings(uri).DefaultIndex(_index);
            _client = new ElasticClient(settings);
            
            //create index if it doesn't exists
            if (!_client.Indices.Exists(new IndexExistsDescriptor(_index)).Exists)
            {
                var result = _client.Indices.Create(_index);
                Console.WriteLine(result.ToString());
            }
        }
    }
}