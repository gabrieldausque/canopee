using Canopee.Common;
using Canopee.Common.Pipelines;
using Microsoft.Extensions.Configuration;

namespace Canopee.Core.Pipelines
{
    /// <summary>
    /// Factory in charge of creating a <see cref="IInput"/> 
    /// </summary>
    public class InputFactory : FactoryFromDirectoryBase
    {
        /// <summary>
        /// The lock object for the singleton
        /// </summary>
        private static readonly object LockInstance = new object();
        
        /// <summary>
        /// The singleton instance
        /// </summary>
        private static InputFactory _instance;

        /// <summary>
        /// Get the singleton instance and create it if needed
        /// </summary>
        /// <param name="directoryCatalog">the directory to load <see cref="IInput"/> catalog from</param>
        /// <returns></returns>
        public static InputFactory Instance(string directoryCatalog = @"./Pipelines")
        {
            lock (LockInstance)
            {
                if (_instance == null)
                {
                    _instance = new InputFactory(directoryCatalog);
                }
            }
            return _instance;
        }

        /// <summary>
        /// Set the global factory instance with a new one.
        /// </summary>
        /// <param name="globalInstance"></param>
        public static void SetGlobalInstance(InputFactory globalInstance)
        {
            lock (LockInstance)
            {
                _instance = globalInstance;
            }
        }
        
        /// <summary>
        /// Default constructor. Create the input factory using the specified catalog
        /// </summary>
        /// <param name="directoryCatalog">the directory to load <see cref="IInput"/> catalog from</param>
        public InputFactory(string directoryCatalog = @"./Pipelines") : base(directoryCatalog)
        {
        }

        /// <summary>
        /// Get the corresponding <see cref="IInput"/> from the specified configuration
        /// </summary>
        /// <param name="configurationInput">the <see cref="IInput"/> configuration</param>
        /// <param name="loggingConfiguration">the logger configuration</param>
        /// <param name="agentId">the agentId to set in the <see cref="IInput"/></param>
        /// <returns></returns>
        public IInput GetInput(IConfigurationSection configurationInput,IConfigurationSection loggingConfiguration, string agentId)
        {
            var type = string.IsNullOrWhiteSpace(configurationInput["InputType"]) ? "Default" : configurationInput["InputType"];
            bool.TryParse(configurationInput["OSSpecific"], out var isOsSpecific);
            if(isOsSpecific && type != "Default")
            {
                type = $"{type}{GetCurrentPlatform().ToString()}";
            }
            var input = Container.GetExport<IInput>(type);
            input?.Initialize(configurationInput,loggingConfiguration, agentId);
            return input;
        }
    }
}
