using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Threading;
using Canopee.Common;
using Canopee.Common.Events;
using Canopee.Core.Configuration;
using Canopee.Core.Logging;
using Microsoft.Extensions.Configuration;

namespace Canopee.StandardLibrary.Configuration
{
    [Export("CanopeeServer",typeof(IConfigurationSynchronizer))]
    public class CanopeeServerConfigurationSynchronizer : IConfigurationSynchronizer
    {
        private static ICanopeeLogger Logger = null;
        
        private string _url;
        private int _dueTime;
        private int _period;
        private Timer _timer;

        public CanopeeServerConfigurationSynchronizer()
        {
            var configuration = ConfigurationService.Instance.GetLoggingConfiguration();
            Logger = CanopeeLoggerFactory.Instance().GetLogger(configuration, this.GetType());   
        }
        
        public JsonObject GetConfigFromSource()
        {
            //TODO : first get the list of group for agent Id
            //TODO : get the default conf, 
            //TODO : foreach agentgroup get the configuration
            //TODO : get the agent conf
            //TODO : foreach configuration property, override based on default < agentgroup by priority < agent
            //TODO : foreach pipelines, overrides pipeline based on name, following priority order default < agentgroup by priority < agent
            //TODO : return the object
            return null;
        }

        public void Start()
        {
            this._timer = new Timer((state) =>
            {
                var newConfig = GetConfigFromSource();
                var currentConfig = ConfigurationService.Instance.GetConfigurationAsJsonObject();
                
                //TODO : synchronize Logging section
                //TODO : synchronize Trigger Section
                //TODO : synchronize UniqueInstance value
                //TODO : synchronize DB section
                
                var isNewConfigurationToLoad = SynchronizePipelines(newConfig, currentConfig);
                

                if (isNewConfigurationToLoad)
                {
                    if (OnNewConfiguration != null)
                    {
                        try
                        {
                            OnNewConfiguration.Invoke(this, new NewConfigurationEventArg()
                            {
                                NewConfiguration = currentConfig
                            });
                        }
                        catch (Exception ex)
                        {
                            Logger.LogError($"Error while synchronizing configuration : {ex}");
                        }
                    }
                }
            }, null,_dueTime,_period);
        }

        private bool SynchronizePipelines(JsonObject newConfig, JsonObject currentConfig)
        {
            var newPipelines = newConfig.GetProperty<JsonObject>("Canopee")
                .GetProperty<List<object>>("Pipelines");
            var currentPipelines = currentConfig.GetProperty<JsonObject>("Canopee")
                .GetProperty<List<object>>("Pipelines");
            bool isNewConfigurationToLoad = false;
            foreach (var pipelineConfig in newPipelines)
            {
                var currentPipeline = currentPipelines
                    .FirstOrDefault(p =>
                        ((JsonObject) p).GetProperty<string>("Id") == ((JsonObject) pipelineConfig).GetProperty<string>("Id"));
                if (currentPipeline != null)
                {
                    if (currentPipeline.ToString() != pipelineConfig.ToString())
                    {
                        currentPipelines.Remove(currentPipeline);
                        currentPipelines.Add(pipelineConfig);
                        isNewConfigurationToLoad = true;
                    }
                }
                else
                {
                    currentPipelines.Add(pipelineConfig);
                    isNewConfigurationToLoad = true;
                }
            }

            return isNewConfigurationToLoad;
        }

        public void Stop()
        {
            _timer.Change(0, Timeout.Infinite);
        }

        public event EventHandler<NewConfigurationEventArg> OnNewConfiguration;
        public void Initialize(IConfiguration configurationServiceConfiguration)
        {
            this._url = configurationServiceConfiguration["url"];
            this._dueTime = configurationServiceConfiguration.GetValue<int>("DueTimeInMs");
            this._period = configurationServiceConfiguration.GetValue<int>("PeriodInMs");
        }
    }
}