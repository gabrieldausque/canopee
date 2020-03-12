using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace Canopee.Core.Configuration
{
    public class ConfigurationService
    {
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
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory());
            Console.WriteLine("Loading configuration file : appsettings.json");
            builder.AddJsonFile("appsettings.json", true);
            var currentEnvironment = System.Environment.GetEnvironmentVariable("CANOPEE_ENVIRONMENT");
            if (!string.IsNullOrWhiteSpace(currentEnvironment))
            {
                Console.WriteLine($"Loading configuration file : appsettings.{currentEnvironment}.json");
                builder.AddJsonFile($"appsettings.{currentEnvironment}.json", true);
            }
            Configuration = builder.Build();
        }

        public string AgentId
        {
            get { return Configuration.GetSection("Canopee")["AgentId"]; }
        }

    }
}