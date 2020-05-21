using Canopee.Common;
using Canopee.Common.Pipelines;
using Microsoft.Extensions.Configuration;

namespace Canopee.Core.Pipelines
{
    /// <summary>
    /// The factory in charge of creating <see cref="ITransform"/>
    /// </summary>
    public class TransformFactory : FactoryFromDirectoryBase
    {
        /// <summary>
        /// The lock object for the singleton
        /// </summary>
        private static readonly object LockInstance = new object();
        
        /// <summary>
        /// The singleton instance
        /// </summary>
        private static TransformFactory _instance;

        /// <summary>
        /// Get and create if needed the singleton instance
        /// </summary>
        /// <param name="directoryCatalog">the directory from which to load the <see cref="ITransform"/> catalog</param>
        /// <returns></returns>
        public static TransformFactory Instance(string directoryCatalog=@"./Pipelines")
        {
            lock (LockInstance)
            {
                if (_instance == null)
                {
                    _instance = new TransformFactory(directoryCatalog);
                }
            }
            return _instance;
        }

        /// <summary>
        /// Set the global factory instance with a new one.
        /// </summary>
        /// <param name="globalInstance"></param>
        public static void SetGlobalInstance(TransformFactory globalInstance)
        {
            lock (LockInstance)
            {
                _instance = globalInstance;
            }
        }
        
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="directoryCatalog">the directory from which to load the <see cref="ITransform"/> catalog</param>
        public TransformFactory(string directoryCatalog = @"./Pipelines") : base(directoryCatalog)
        {
        }

        /// <summary>
        /// Create the <see cref="ITransform"/> corresponding to the configuration
        /// </summary>
        /// <param name="configurationTransform">the configuration of the <see cref="ITransform"/></param>
        /// <param name="loggingConfiguration">the logger configuration</param>
        /// <returns></returns>
        public ITransform GetTransform(IConfigurationSection configurationTransform, IConfigurationSection loggingConfiguration)
        {
            var type = string.IsNullOrWhiteSpace(configurationTransform["TransformType"]) ? "Default" : configurationTransform["TransformType"];
            bool.TryParse(configurationTransform["OSSpecific"], out var isOsSpecific);
            if(isOsSpecific && type != "Default")
            {
                type = $"{type}{GetCurrentPlatform().ToString()}";
            }
            var transformer = Container.GetExport<ITransform>(type);
            transformer?.Initialize(configurationTransform, loggingConfiguration);
            return transformer;
        }
    }
}