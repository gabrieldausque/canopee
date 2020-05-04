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
    /// <summary>
    /// The object in charge of checking and notifying if a new configuration is available from an ASPNet CanopeeServer.
    /// It will :
    ///  - get the local configuration
    ///  - get default configuration from distant source
    ///  - get the associated group configurations for agent Id sorted by ascending priority, merged with the default
    ///  - get the agent id specific configuration and merged with the previous one
    ///  - compare the local configuration to the merged configurations
    /// If the new configuration is different from the local one, it raised an event with the new configuration
    ///
    /// the behavior of the configuration is set through the configuration :
    ///
    /// <example>
    /// <code>
    ///     {
    ///         ...
    ///         "Canopee": {
    ///             ...
    ///                 "Configuration": {
    ///                     "IsSync": true,
    ///                     "SynchronizerType":"Default",
    ///                     "NoSSLCheck": true,
    ///                     "url": "http://localhost:5000",
    ///                     "DueTimeInMs": "3000",
    ///                     "PeriodInMs": "30000"
    ///                 },
    ///             ...
    ///         }
    ///     }
    /// </code>
    /// </example>
    ///
    /// All configuration are gotten from the Configuration element of the Canopee configuration
    /// 
    /// </summary>
    [Export("Default",typeof(ICanopeeConfigurationSynchronizer))]
    public class CanopeeServerConfigurationSynchronizer : ICanopeeConfigurationSynchronizer
    {
        /// <summary>
        /// The internal <see cref="ICanopeeConfigurationReader"/>
        /// </summary>
        private readonly ICanopeeConfigurationReader _reader;
        
        /// <summary>
        /// The internal logger
        /// </summary>
        private static ICanopeeLogger Logger = null;
        
        /// <summary>
        /// The due time in ms before starting the first check
        /// </summary>
        private int _dueTime;
        
        /// <summary>
        /// The period for each check
        /// </summary>
        private int _period;
        
        /// <summary>
        /// The timer that will trigger a configuration check
        /// </summary>
        private Timer _timer;
        
        /// <summary>
        /// The internal flag that indicate if a check is already running
        /// </summary>
        private bool _running;

        /// <summary>
        /// The default constructor. 
        /// </summary>
        /// <param name="reader">The <see cref="ICanopeeConfigurationReader"/> that will be used to get centralized configuration</param>
        [ImportingConstructor]
        public CanopeeServerConfigurationSynchronizer([Import("Default")] ICanopeeConfigurationReader reader)
        {
            _reader = reader;
        }
        
        /// <summary>
        /// Get the merged configuration from the centralized source accessed through the <see cref="ICanopeeConfigurationSynchronizer"/>.
        /// It will merged in order :
        ///  - default configuration from distant source
        ///  - associated group configurations for agent Id sorted by ascending priority, merged with the default
        ///  - the agent id specific configuration and merged with the previous one
        /// </summary>
        /// <returns></returns>
        public JsonObject GetConfigFromSource()
        {
            var agentId = ConfigurationService.Instance.AgentId;
            var agentGroups = _reader.GetGroups(agentId).ToList();
            agentGroups.Sort(((left, right) => left.Priority.CompareTo(right.Priority)));
            //Get default configuration
            var currentConfig = _reader.GetConfiguration();
            foreach (var group in agentGroups)
            {
                var groupConfig = _reader.GetConfiguration( group: group.Group);
                if (groupConfig != null)
                {
                    MergeCanopeeConfiguration(currentConfig, groupConfig);    
                }

                var groupConfigForAgent = _reader.GetConfiguration(agentId, group.Group);
                if(groupConfigForAgent != null)
                {
                    MergeCanopeeConfiguration(currentConfig, groupConfigForAgent);
                }
            }

            var agentConfiguration = _reader.GetConfiguration(agentId: agentId);
            if (agentConfiguration != null)
            {
                MergeCanopeeConfiguration(currentConfig,agentConfiguration);    
            }
            return currentConfig;
        }

        /// <summary>
        /// Merge two Canopee configuration.
        /// It will merge following configuration :
        /// - Logging
        /// - Trigger
        /// - Db
        /// - Pipelines
        /// return true if a merge operation has been done (aka a properties from configToMerge has overwritten a property of the currentConfig)
        /// </summary>
        /// <param name="currentConfig">the config to modify</param>
        /// <param name="configToMerge">the config that will overwrite any corresponding property in the currentConfig</param>
        /// <returns></returns>
        private bool MergeCanopeeConfiguration(JsonObject currentConfig, JsonObject configToMerge)
        {
            var newLogging = SynchronizeJsonObjectProperty(currentConfig, configToMerge, "Logging");
            var newTrigger = SynchronizeJsonObjectProperty(currentConfig, configToMerge, "Trigger");
            var newDb = SynchronizeJsonObjectProperty(currentConfig, configToMerge, "Db"); 
            var newPipelines = SynchronizePipelines(currentConfig, configToMerge);
            return newLogging || newTrigger || newDb || newPipelines;
        }

        /// <summary>
        /// Start the synchronization process
        /// </summary>
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


        /// <summary>
        /// Helper that will overwrite a property in the currentConfig with the property of the newConfig if the value of newConfig is different.
        /// Beware : It will not suppress an existing value.
        /// </summary>
        /// <param name="currentConfig">the config to modify</param>
        /// <param name="newConfig">the config that may have new value for property</param>
        /// <param name="propertyName">the property to check</param>
        /// <returns>true if a modification has been done, false otherwise</returns>
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

        /// <summary>
        /// Update existing pipelines and add new ones from the newConfig in the currentConfig
        /// </summary>
        /// <param name="currentConfig">the config to modify</param>
        /// <param name="newConfig">the new config to get updated or new pipelines from</param>
        /// <returns>true if a modification has been done. false otherwise</returns>
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

        /// <summary>
        /// Stop the synchronization process
        /// </summary>
        public void Stop()
        {
            _timer.Change(0, Timeout.Infinite);
        }

        /// <summary>
        /// Event raised if a new configuration is found during synchronization process
        /// </summary>
        public event EventHandler<NewConfigurationEventArg> OnNewConfiguration;
        
        /// <summary>
        /// Initialize the current synchronizer with the Configuration configuration section and the Logging configuration 
        /// </summary>
        /// <param name="serviceConfiguration">the configuration service</param>
        /// <param name="loggingConfiguration">the logger configuration</param>
        public void Initialize(IConfiguration serviceConfiguration, IConfiguration loggingConfiguration)
        {
            Logger = CanopeeLoggerFactory.Instance().GetLogger(loggingConfiguration, this.GetType());
            _reader.Initialize(serviceConfiguration, loggingConfiguration);
            this._dueTime = serviceConfiguration.GetValue<int>("DueTimeInMs");
            this._period = serviceConfiguration.GetValue<int>("PeriodInMs");
        }
    }
}