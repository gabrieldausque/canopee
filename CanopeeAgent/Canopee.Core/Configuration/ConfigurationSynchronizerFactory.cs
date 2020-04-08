using Canopee.Common;
using Canopee.Common.Configuration;
using Microsoft.Extensions.Configuration;

namespace Canopee.Core.Configuration
{
    public class ConfigurationSynchronizerFactory : FactoryFromDirectoryBase
    {
        private static readonly object LockInstance = new object();
        private static ConfigurationSynchronizerFactory _instance;

        public static ConfigurationSynchronizerFactory Instance(string directoryCatalog=@"./Pipelines")  {
            lock (LockInstance)
            {
                if (_instance == null)
                {
                    _instance = new ConfigurationSynchronizerFactory(directoryCatalog);
                }
            }
            return _instance;
        }
        public ConfigurationSynchronizerFactory(string directoryCatalog = @"./Pipelines") : base(directoryCatalog)
        {
        }

        public IConfigurationSynchronizer GetSynchronizer(IConfiguration configurationServiceConfiguration,
            IConfiguration loggingConfiguration)
        {
            var type = string.IsNullOrWhiteSpace(configurationServiceConfiguration["SynchronizerType"]) ? "Default" 
                : configurationServiceConfiguration["SynchronizerType"];
            var synchronizer = Container.GetExport<IConfigurationSynchronizer>(type);
            synchronizer.Initialize(configurationServiceConfiguration, loggingConfiguration);
            return synchronizer;
        }
    }
}