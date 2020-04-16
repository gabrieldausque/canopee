using System;
using Canopee.Common.Configuration.Events;
using Microsoft.Extensions.Configuration;

namespace Canopee.Common.Configuration
{
    /// <summary>
    /// Contract for the object that will synchronize configuration from an external source.
    /// </summary>
    public interface IConfigurationSynchronizer
    {
        /// <summary>
        /// Get the actual configuration from the external source 
        /// </summary>
        /// <returns>a <see cref="JsonObject"/> that contains all configuration properties</returns>
        JsonObject GetConfigFromSource();

        /// <summary>
        /// Start the synchronization process
        /// </summary>
        void Start();

        /// <summary>
        /// Stop the synchronization process
        /// </summary>
        void Stop();
        
        /// <summary>
        /// Event raised if a new configuration is obtained from the source
        /// </summary>
        event EventHandler<NewConfigurationEventArg> OnNewConfiguration;
        /// <summary>
        /// Initialize the synchronizer after creation
        /// </summary>
        /// <param name="configurationServiceConfiguration">the configuration service configuration</param>
        /// <param name="loggingConfiguration">the logging configuration</param>
        void Initialize(IConfiguration configurationServiceConfiguration, IConfiguration loggingConfiguration);
    }
}