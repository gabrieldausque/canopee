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
    public class ConfigurationService
    {
        private static ICanopeeLogger Logger = null; 
        private static readonly object InstanceLock = new object();
        private static ConfigurationService _instance;
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

        public void Start()
        {
            _synchronizer?.Start();
        }

        public event EventHandler OnNewConfiguration;
        public IConfiguration Configuration { get; private set; }

        private readonly string lastConfigurationFilePath = "appsettings.json";
        private IConfigurationSynchronizer _synchronizer;

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

        public IConfiguration GetConfigurationServiceConfiguration()
        {
            return GetCanopeeConfiguration().GetSection("Configuration");
        }
        
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

        public IConfiguration GetCanopeeConfiguration()
        {
            return Configuration.GetSection("Canopee");
        }

        public bool IsUniqueInstance()
        {
            if(string.IsNullOrWhiteSpace(GetCanopeeConfiguration()["UniqueInstance"]))
            {
                return true;
            }
            return GetCanopeeConfiguration().GetValue<bool>("UniqueInstance");
        }
        
        public IConfiguration GetLoggingConfiguration()
        {
            return GetCanopeeConfiguration().GetSection("Logging");
        }

        public IConfiguration GetPipelinesConfiguration()
        {
            return GetCanopeeConfiguration().GetSection("Pipelines");
        }

        public JsonObject GetConfigurationAsJsonObject()
        {
            return JsonObject.LoadFromFile(this.lastConfigurationFilePath);
        }
    }
}