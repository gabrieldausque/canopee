using System.IO;
using Microsoft.Extensions.Configuration;

namespace CanopeeAgent.Core.Configuration
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
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true);
            Configuration = builder.Build();
        }
        
    }
}