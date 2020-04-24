using Canopee.Common;
using Canopee.Common.Pipelines;
using Microsoft.Extensions.Configuration;

namespace Canopee.Core.Pipelines
{
    /// <summary>
    /// The factory in charge of creating <see cref="ITrigger"/>
    /// </summary>
    public class TriggerFactory : FactoryFromDirectoryBase
    {
        /// <summary>
        /// The lock object for the singleton instance
        /// </summary>
        private static readonly object LockInstance = new object();
        
        /// <summary>
        /// The singleton instance
        /// </summary>
        private static TriggerFactory _instance;

        /// <summary>
        /// Get and create if needed the singleton instance
        /// </summary>
        /// <param name="directoryCatalog">the directory from which to load the <see cref="ITransform"/> catalog</param>
        /// <returns></returns>
        public static TriggerFactory Instance(string directoryCatalog = @"./Pipelines")
        {
                lock (LockInstance)
                {
                    if (_instance == null)
                    {
                        _instance = new TriggerFactory(directoryCatalog);
                    }
                }
                return _instance;
        }

        /// <summary>
        /// The default constructor.
        /// </summary>
        /// <param name="directoryCatalog">the directory from which to load the <see cref="ITransform"/> catalog</param>
        public TriggerFactory(string directoryCatalog = @"./Pipelines") : base(directoryCatalog)
        {
        }

        /// <summary>
        /// Create the <see cref="ITrigger"/> corresponding to the configuration
        /// </summary>
        /// <param name="configurationTrigger">the configuration of the <see cref="ITrigger"/></param>
        /// <param name="loggingConfiguration">the logger configuration</param>
        /// <returns></returns>
        public ITrigger GetTrigger(IConfigurationSection configurationTrigger, IConfigurationSection loggingConfiguration)
        {
            var type = string.IsNullOrWhiteSpace(configurationTrigger["TriggerType"]) ? "Default" : configurationTrigger["TriggerType"];
            var trigger = Container.GetExport<ITrigger>(type);
            trigger?.Initialize(configurationTrigger, loggingConfiguration);
            return trigger;
        }
    }
}