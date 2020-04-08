using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using Canopee.Common;
using Canopee.Common.Configuration;
using Canopee.Common.Configuration.AspNet.Dtos;
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
            var currentConfig = _reader.GetConfiguration("*", "*");
            foreach (var group in agentGroups)
            {
                MergeCanopeeConfiguration(currentConfig, _reader.GetConfiguration(group.AgentId, group.Group));
            }

            MergeCanopeeConfiguration(currentConfig, _reader.GetConfiguration(agentId, "*"));
            return currentConfig;
        }

        private bool MergeCanopeeConfiguration(JsonObject currentConfig, JsonObject configToMerge)
        {
            return SynchronizeJsonObjectProperty(currentConfig, configToMerge, "Logging") &&
                SynchronizeJsonObjectProperty(currentConfig, configToMerge, "Trigger") &&
                SynchronizeJsonObjectProperty(currentConfig, configToMerge, "Db") &&
                SynchronizePipelines(currentConfig, configToMerge);
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
                    newCanopeeConfig.TryGetProperty<JsonObject>(propertyName, out var newLogging);
                    currentCanopeeConfig.TryGetProperty<JsonObject>(propertyName,out var currentLogging);
                    if ((currentLogging == null && newLogging != null) ||
                        (currentLogging != null &&
                         newLogging != null &&
                         currentLogging.ToString() != newLogging.ToString()))
                    {
                        currentConfig.SetProperty(propertyName, newLogging);
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

        private bool SynchronizePipelines(JsonObject newConfig, JsonObject currentConfig)
        {
            var newPipelines = newConfig.GetProperty<JsonObject>("Canopee")
                .GetProperty<List<object>>("Pipelines");
            var currentPipelines = currentConfig.GetProperty<JsonObject>("Canopee")
                .GetProperty<List<object>>("Pipelines");
            bool isNewConfigurationToLoad = false;

            foreach (var pipelineConfig in currentPipelines.ToArray())
            {
                var existingPipeline = newPipelines.FirstOrDefault(p =>
                    ((JsonObject) p).GetProperty<string>("Id") ==
                    ((JsonObject) pipelineConfig).GetProperty<string>("Id"));
                if (existingPipeline == null)
                {
                    currentPipelines.Remove(pipelineConfig);
                }
            }
            
            //get new pipelines
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

    public interface ICanopeeConfigurationReader
    {
        void Initialize(IConfiguration serviceConfiguration, IConfiguration loggingConfiguration);
        
        ICollection<AgentGroupDto> GetGroups(string agentId);
        
        JsonObject GetConfiguration(string agentId, string group);
    }
}