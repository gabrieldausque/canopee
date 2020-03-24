using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json;
using Canopee.Common;
using Canopee.Core.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

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

        public IConfiguration Configuration { get; private set; }

        private readonly string lastConfigurationFilePath = "appsettings.json";
        
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
                Logger = CanopeeLoggerFactory.Instance().GetLogger(Configuration.GetSection("Canopee:Logging"), this.GetType());
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

        public string AgentId
        {
            get
            {
                var agentId = Configuration.GetSection("Canopee")["AgentId"];
                if (string.IsNullOrEmpty(agentId))
                {
                    agentId = Guid.NewGuid().ToString();
                    SetValueInFile("AgentId", agentId);
                }
                Configuration["Canopee:AgentId"] = agentId;
                return agentId;
            }
        }

        private void SetValueInFile(string key, object value)
        {
            string configurationFileContent = File.ReadAllText(lastConfigurationFilePath);
            var deserialized = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, object>>>(configurationFileContent);
            deserialized["Canopee"][key] = value;
            configurationFileContent = JsonSerializer.Serialize(deserialized, new JsonSerializerOptions()
            {
                WriteIndented = true
            });
            Logger.LogDebug($"Write agent Id to {Path.GetFullPath(lastConfigurationFilePath)}");
            File.WriteAllText(lastConfigurationFilePath, configurationFileContent);
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
    }
}