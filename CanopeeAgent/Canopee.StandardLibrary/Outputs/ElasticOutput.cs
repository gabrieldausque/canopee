using System;
using System.Collections.Generic;
using System.Composition;
using Canopee.Common;
using Elasticsearch.Net;
using Microsoft.Extensions.Configuration;
using Nest;
using System.Text.Json;
using Canopee.Common.Pipelines;
using Canopee.Common.Pipelines.Events;
using Canopee.Core.Pipelines;
using Microsoft.Extensions.Logging;

namespace Canopee.StandardLibrary.Outputs
{
    /// <summary>
    /// Send the <see cref="ICollectedEvent"/> to an ElasticSearch server.
    ///
    /// The configuration will be :
    ///
    /// <example>
    ///
    /// <code>
    ///     {
    ///         ...
    ///         "Canopee": {
    ///             ...
    ///                 "Pipelines": [
    ///                  ...   
    ///                   {
    ///                     "Name": "OS",
    ///                     ...
    ///                     "Outputs" : [{
    ///                        "OutputType": "Elastic",
    ///                        "DefaultIndex": "canopee-hw-hardwareinfos",
    ///                        "Indexes": [
    ///                            {
    ///                                "InfosType": "Canopee.StandardLibrary.Inputs.Hardware.HardwareInfos",
    ///                                "Index": "canopee-hw-hardwareinfos"
    ///                            },
    ///                            {
    ///                                "InfosType": "Canopee.StandardLibrary.Inputs.Hardware.DiskInfos",
    ///                                "Index": "canopee-hw-disks"
    ///                            },
    ///                            {
    ///                                "InfosType": "Canopee.StandardLibrary.Inputs.Hardware.DisplayInfos",
    ///                                "Index": "canopee-hw-display"
    ///                            },
    ///                            {
    ///                                "InfosType": "Canopee.StandardLibrary.Inputs.Hardware.GraphicalCardInfos",
    ///                                "Index": "canopee-hw-graphicalcards"
    ///                            },
    ///                            {
    ///                                "InfosType": "Canopee.StandardLibrary.Inputs.Hardware.UsbPeripheralInfos",
    ///                                "Index": "canopee-hw-usbperipherals"
    ///                            }
    ///                           ],
    ///                        "Url": "http://127.0.0.1:9200"
    ///                    }
    ///                  ...
    ///                 }
    ///                 ...   
    ///                 ]
    ///             ...
    ///         }
    ///     }
    /// </code>
    /// 
    /// </example>
    ///
    /// 
    /// 
    /// </summary>
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

        public override void Initialize(IConfiguration configurationOutput, IConfigurationSection loggingConfiguration)
        {
            base.Initialize(configurationOutput, loggingConfiguration);
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