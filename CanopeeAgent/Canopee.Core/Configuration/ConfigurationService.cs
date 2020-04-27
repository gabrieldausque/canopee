using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Canopee.Common;
using Canopee.Common.Configuration;
using Canopee.Common.Logging;
using Canopee.Core.Logging;
using Microsoft.Extensions.Configuration;

namespace Canopee.Core.Configuration
{
    /// <summary>
    /// Object in charge of managing the configuration :
    ///     - read from local
    ///     - write modification to local file
    ///     - synchronize from a CanopeeServer that exposes the centralized configuration (Experimental feature)
    /// </summary>
    public class ConfigurationService
    {
        /// <summary>
        /// The internal logger. By default a console logger
        /// </summary>
        private static ICanopeeLogger Logger = null; 
        /// <summary>
        /// The lock object used for singleton 
        /// </summary>
        private static readonly object InstanceLock = new object();
        /// <summary>
        /// The singleton instance
        /// </summary>
        private static ConfigurationService _instance;
        /// <summary>
        /// Get the singleton instance
        /// </summary>
        public static ConfigurationService Instance
        {
            get
            {
                lock (InstanceLock)
                {
                    if (_instance == null)
                    {
                        _instance = new ConfigurationService();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// Start the synchronization of configuration if needed
        /// </summary>
        public void Start()
        {
            _synchronizer?.Start();
        }

        /// <summary>
        /// Raised if a new configuration is obtained through the synchronization process
        /// </summary>
        public event EventHandler OnNewConfiguration;
        
        /// <summary>
        /// All configurations sections
        /// </summary>
        public IConfiguration Configuration { get; private set; }

        /// <summary>
        /// The last configuration file read, depending on the CANOPEE_ENVIRONMENT environment variable. If the variable is defined, the last file to be read will be :
        /// appsettings.[environment variable value].json
        /// Default : appsettings.json
        /// </summary>
        private readonly string lastConfigurationFilePath = "appsettings.json";
        
        /// <summary>
        /// The synchronizer object, in charge of evaluating if a new configuration has been setup
        /// </summary>
        private ICanopeeConfigurationSynchronizer _synchronizer;

        /// <summary>
        /// Default constructor
        /// </summary>
        public ConfigurationService()
        {
            var messages = new List<string>();
            try
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory());
                messages.Add("Loading configuration file : appsettings.json");
                builder.AddJsonFile("appsettings.json", true);
                var currentEnvironment = System.Environment.GetEnvironmentVariable("CANOPEE_ENVIRONMENT");
                if (!string.IsNullOrWhiteSpace(currentEnvironment))
                {
                    var environmentFile = $"appsettings.{currentEnvironment}.json";
                    messages.Add($"Loading configuration file : {environmentFile}");
                    builder.AddJsonFile($"{environmentFile}", true);
                    lastConfigurationFilePath = environmentFile;
                }
                Configuration = builder.Build();
                Logger = CanopeeLoggerFactory.Instance().GetLogger(GetLoggingConfiguration(), this.GetType());
                
                if(!string.IsNullOrWhiteSpace(GetCanopeeConfiguration()["Configuration:IsSync"]) &&
                   GetCanopeeConfiguration().GetSection("Configuration").GetValue<bool>("IsSync"))
                {
                    Logger.LogInfo("Configuration is synchronized");
                    _synchronizer = ConfigurationSynchronizerFactory.Instance()
                        .GetSynchronizer(GetConfigurationServiceConfiguration(), GetLoggingConfiguration());
                    _synchronizer.OnNewConfiguration += (sender, arg) =>
                    {
                        arg.NewConfiguration.WriteTo(this.lastConfigurationFilePath);
                        Logger.LogWarning("New configuration received from synchronizer and written to file. Raising NewConfiguration Event");
                        RaiseOnNewConfiguration();
                    };
                }
                else
                {
                    Logger.LogInfo("Configuration is only local");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                messages.Add($"Error while loading configuration : {ex}");
            }
            finally
            {
                foreach (var message in messages)
                {
                    Logger?.Log( message);    
                }
            }
        }

        /// <summary>
        /// Raise the <see cref="ConfigurationService.OnNewConfiguration"/> event
        /// </summary>
        private void RaiseOnNewConfiguration()
        {
            try
            {
                OnNewConfiguration?.Invoke(this, new EventArgs());
            }
            catch (Exception ex)
            {
                Logger.LogError($"Error while raising event for new configuration {ex}");
            }
        }

        /// <summary>
        /// Get the Configuration section of the Canopee section
        /// </summary>
        /// <returns>the Configuration section as IConfiguration</returns>
        public IConfiguration GetConfigurationServiceConfiguration()
        {
            return GetCanopeeConfiguration().GetSection("Configuration");
        }
        
        /// <summary>
        /// Get the agent id configured. If no agent id is configured, will set up a new one as a Guid
        /// </summary>
        public string AgentId
        {
            get
            {
                var agentId = Configuration.GetSection("Canopee")["AgentId"];
                if (string.IsNullOrEmpty(agentId))
                {
                    agentId = Guid.NewGuid().ToString();
                    SetValueInFile("AgentId", agentId);
                    Configuration["Canopee:AgentId"] = agentId;
                }
                return agentId;
            }
        }

        /// <summary>
        /// Write a new value for a specific property in the Canopee Section to the last configuration file read
        /// </summary>
        /// <param name="key">the name of the section or value to modify</param>
        /// <param name="value">the value to set</param>
        /// <exception cref="NotSupportedException">If no Canopee section, raise this exception</exception>
        private void SetValueInFile(string key, object value)
        {
            var configurationFile = JsonObject.LoadFromFile(lastConfigurationFilePath);
            var canopeeJsonObject = configurationFile["Canopee"] as JsonObject;
            if (canopeeJsonObject == null)
            {
                throw new NotSupportedException($"The application must have a Canopee section in {lastConfigurationFilePath}. Please correct and relaunch application");
            }
            canopeeJsonObject.SetProperty(key, value);
            configurationFile.WriteTo(lastConfigurationFilePath);
        }

        /// <summary>
        /// Get the whole Canopee Section
        /// </summary>
        /// <returns></returns>
        public IConfigurationSection GetCanopeeConfiguration()
        {
            return Configuration.GetSection("Canopee");
        }

        /// <summary>
        /// Get the UniqueInstance parameter value.
        /// Default : true
        /// </summary>
        /// <returns></returns>
        public bool IsUniqueInstance()
        {
            if(string.IsNullOrWhiteSpace(GetCanopeeConfiguration()["UniqueInstance"]))
            {
                return true;
            }
            return GetCanopeeConfiguration().GetValue<bool>("UniqueInstance");
        }
        
        /// <summary>
        /// Get the logging configuration section
        /// </summary>
        /// <returns></returns>
        public IConfiguration GetLoggingConfiguration()
        {
            return GetCanopeeConfiguration().GetSection("Logging");
        }

        /// <summary>
        /// Get the pipelines configuration section
        /// </summary>
        /// <returns></returns>
        public IConfiguration GetPipelinesConfiguration()
        {
            return GetCanopeeConfiguration().GetSection("Pipelines");
        }

        /// <summary>
        /// Get the whole configuration file as a JsonObject. Used for write operation
        /// </summary>
        /// <returns></returns>
        public JsonObject GetConfigurationAsJsonObject()
        {
            return JsonObject.LoadFromFile(this.lastConfigurationFilePath);
        }
    }
}