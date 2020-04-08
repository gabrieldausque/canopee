using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using Canopee.Common;
using Canopee.Common.Configuration;
using Canopee.Common.Configuration.Events;
using Canopee.Common.Logging;
using Canopee.Core.Configuration;
using Canopee.Core.Logging;
using Microsoft.Extensions.Configuration;
using Nest;

namespace Canopee.StandardLibrary.Configuration
{
    [Export("Default",typeof(IConfigurationSynchronizer))]
    public class CanopeeServerConfigurationSynchronizer : IConfigurationSynchronizer
    {
        private readonly ICanopeeConfigurationReader _reader;
        private static ICanopeeLogger Logger = null;
        
        private int _dueTime;
        private int _period;
        private Timer _timer;
        private bool _running;

        [ImportingConstructor]
        public CanopeeServerConfigurationSynchronizer([Import("Default")] ICanopeeConfigurationReader reader)
        {
            _reader = reader;
        }
        
        public JsonObject GetConfigFromSource()
        {
            var agentId = ConfigurationService.Instance.AgentId;
            var agentGroups = _reader.GetGroups(agentId).ToList();
            agentGroups.Sort(((left, right) => left.Priority.CompareTo(right.Priority)));
            //Get default configuration
            var currentConfig = _reader.GetConfiguration();
            foreach (var group in agentGroups)
            {
                var groupConfig = _reader.GetConfiguration(string.Empty, group.Group);
                if (groupConfig != null)
                {
                    MergeCanopeeConfiguration(currentConfig, groupConfig);    
                }
            }

            var agentConfiguration = _reader.GetConfiguration(agentId);
            if (agentConfiguration != null)
            {
                MergeCanopeeConfiguration(currentConfig,agentConfiguration);    
            }
            return currentConfig;
        }

        private bool MergeCanopeeConfiguration(JsonObject currentConfig, JsonObject configToMerge)
        {
            var newLogging = SynchronizeJsonObjectProperty(currentConfig, configToMerge, "Logging");
            var newTrigger = SynchronizeJsonObjectProperty(currentConfig, configToMerge, "Trigger");
            var newDb = SynchronizeJsonObjectProperty(currentConfig, configToMerge, "Db"); 
            var newPipelines = SynchronizePipelines(currentConfig, configToMerge);
            return newLogging || newTrigger || newDb || newPipelines;
        }

        public void Start()
        {
            var configuration = ConfigurationService.Instance.GetLoggingConfiguration();
            Logger = CanopeeLoggerFactory.Instance().GetLogger(configuration, this.GetType());
            this._timer = new Timer((state) =>
            {
                if (!_running)
                {
                    _running = true;
                    try
                    {
                        var isNewConfigurationToLoad = false;
                        var currentConfig = ConfigurationService.Instance.GetConfigurationAsJsonObject();
                        var newConfig = GetConfigFromSource();
                        isNewConfigurationToLoad = MergeCanopeeConfiguration(currentConfig, newConfig);
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
                    }
                    catch (Exception ex)
                    {
                        Logger?.LogError($"Error while synchronizing configuration : {ex}");
                    }
                    finally
                    {
                        _running = false;
                    }
                }
                
            }, null,_dueTime,_period);
        }

        private bool SynchronizeJsonObjectProperty(JsonObject currentConfig, JsonObject newConfig, string propertyName)
        {
            if(newConfig.TryGetProperty<JsonObject>("Canopee", out var newCanopeeConfig))
            {
                if (currentConfig.TryGetProperty<JsonObject>("Canopee", out var currentCanopeeConfig))
                {
                    newCanopeeConfig.TryGetProperty<JsonObject>(propertyName, out var newPropertyValue);
                    currentCanopeeConfig.TryGetProperty<JsonObject>(propertyName,out var currentPropertyValue);
                    if ((currentPropertyValue == null && newPropertyValue != null) ||
                        (currentPropertyValue != null &&
                         newPropertyValue != null &&
                         currentPropertyValue.ToString() != newPropertyValue.ToString()))
                    {
                        currentCanopeeConfig.SetProperty(propertyName, newPropertyValue);
                        return true;
                    }    
                }
                else
                {
                    currentConfig.SetProperty("Canopee", newCanopeeConfig);
                }
            } 
            return false;
        }

        private bool SynchronizePipelines(JsonObject currentConfig, JsonObject newConfig)
        {
            var newPipelines = newConfig.GetProperty<JsonObject>("Canopee")
                .GetProperty<List<object>>("Pipelines");
            var currentPipelines = currentConfig.GetProperty<JsonObject>("Canopee")
                .GetProperty<List<object>>("Pipelines");
            bool isNewConfigurationToLoad = false;
            
            //get new pipelines
            foreach (var pipelineConfig in newPipelines)
            {
                var currentPipeline = currentPipelines
                    .FirstOrDefault(p =>
                    {
                        var jsonObj = (JsonObject) p;
                        var testJson = (JsonObject) pipelineConfig;
                        if (jsonObj.TryGetProperty<string>("Id", out var testedId) &&
                            testJson.TryGetProperty<string>("Id", out var id))
                        {
                            return testedId == id;
                        } 
                        
                        if (jsonObj.TryGetProperty<string>("Name", out var testedName) &&
                                   testJson.TryGetProperty<string>("Name", out var name))
                        {
                            return testedName == name;
                        }
                        return false;
                });
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
            //cleaning deleted pipelines
            return isNewConfigurationToLoad;
        }

        public void Stop()
        {
            _timer.Change(0, Timeout.Infinite);
        }

        public event EventHandler<NewConfigurationEventArg> OnNewConfiguration;
        public void Initialize(IConfiguration serviceConfiguration, IConfiguration loggingConfiguration)
        {
            Logger = CanopeeLoggerFactory.Instance().GetLogger(loggingConfiguration, this.GetType());
            _reader.Initialize(serviceConfiguration, loggingConfiguration);
            this._dueTime = serviceConfiguration.GetValue<int>("DueTimeInMs");
            this._period = serviceConfiguration.GetValue<int>("PeriodInMs");
        }
    }
}