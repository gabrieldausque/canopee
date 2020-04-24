using Canopee.Common;
using Canopee.Common.Configuration;
using Microsoft.Extensions.Configuration;

namespace Canopee.Core.Configuration
{
    /// <summary>
    /// Factory that create <see cref="ICanopeeConfigurationSynchronizer"/> object
    /// </summary>
    public class ConfigurationSynchronizerFactory : FactoryFromDirectoryBase
    {
        /// <summary>
        /// The singleton lock object 
        /// </summary>
        private static readonly object LockInstance = new object();
        
        /// <summary>
        /// The singleton instance
        /// </summary>
        private static ConfigurationSynchronizerFactory _instance;

        /// <summary>
        /// Get the singleton instance
        /// </summary>
        /// <param name="directoryCatalog">
        /// Define where to read all catalog assemblies
        /// Optional. Default : ./Pipelines
        /// </param>
        /// <returns>the singleton instance</returns>
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
        
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="directoryCatalog">
        /// Define where to read all catalog assemblies
        /// Optional. Default : ./Pipelines
        /// </param>
        public ConfigurationSynchronizerFactory(string directoryCatalog = @"./Pipelines") : base(directoryCatalog)
        {
        }

        /// <summary>
        /// Get an instance of a <see cref="ICanopeeConfigurationSynchronizer"/> object from the specified configuration
        /// If no Canopee:Configuration:SynchronizerType value is defined, the contract name used will be Default
        /// </summary>
        /// <param name="configurationServiceConfiguration">The Canopee:Configuration section</param>
        /// <param name="loggingConfiguration">The Canopee:Logging section</param>
        /// <returns></returns>
        public ICanopeeConfigurationSynchronizer GetSynchronizer(IConfiguration configurationServiceConfiguration,
            IConfiguration loggingConfiguration)
        {
            var type = string.IsNullOrWhiteSpace(configurationServiceConfiguration["SynchronizerType"]) ? "Default" 
                : configurationServiceConfiguration["SynchronizerType"];
            var synchronizer = Container.GetExport<ICanopeeConfigurationSynchronizer>(type);
            synchronizer.Initialize(configurationServiceConfiguration, loggingConfiguration);
            return synchronizer;
        }
    }
}