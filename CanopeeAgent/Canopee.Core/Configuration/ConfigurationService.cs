using System;
using System.Collections.Generic;
using System.IO;
using Canopee.Common;
using Canopee.Core.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Canopee.Core.Configuration
{
    public class ConfigurationService
    {
        private static ICanopeeLogger CanopeeLogger = null; 
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
                    messages.Add($"Loading configuration file : appsettings.{currentEnvironment}.json");
                    builder.AddJsonFile($"appsettings.{currentEnvironment}.json", true);
                }
                Configuration = builder.Build();
                CanopeeLogger = CanopeeLoggerFactory.Instance().GetLogger(Configuration.GetSection("Logging"), this.GetType());
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
                    CanopeeLogger?.Log( message);    
                }
            }
        }

        public string AgentId
        {
            get { return Configuration.GetSection("Canopee")["AgentId"]; }
        }
        
        public IConfiguration GetCanopeeConfiguration()
        {
            return Configuration.GetSection("Canopee");
        }

        public IConfiguration GetLoggingConfiguration()
        {
            return GetCanopeeConfiguration().GetSection("Logging");
        }
    }
}