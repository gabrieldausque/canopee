using System.Collections.Generic;
using System.Linq;
using Canopee.Common;
using Canopee.Common.Pipelines;
using Microsoft.Extensions.Configuration;

namespace Canopee.Core.Pipelines
{
    /// <summary>
    /// The factory in charge of creating <see cref="IOutput"/>
    /// </summary>
    public class OutputFactory : FactoryFromDirectoryBase
    {
        /// <summary>
        /// The lock object for the singleton
        /// </summary>
        private static readonly object LockInstance = new object();
        
        /// <summary>
        /// The singleton instance
        /// </summary>
        private static OutputFactory _instance;

        /// <summary>
        /// Get and create if needed the singleton instance
        /// </summary>
        /// <param name="directoryCatalog">the directory from which to load the <see cref="IOutput"/> catalog available</param>
        /// <returns></returns>
        public static OutputFactory Instance(string directoryCatalog=@"./Pipelines")  {
            lock (LockInstance)
            {
                if (_instance == null)
                {
                    _instance = new OutputFactory(directoryCatalog);
                }
            }
            return _instance;
        }

        /// <summary>
        /// Set the global factory instance with a new one.
        /// </summary>
        /// <param name="globalInstance"></param>
        public static void SetGlobalInstance(OutputFactory globalInstance)
        {
            lock (LockInstance)
            {
                _instance = globalInstance;
            }
        }
        
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="directoryCatalog">the directory from which to load the <see cref="IOutput"/> catalog available</param>
        public OutputFactory(string directoryCatalog = @"./Pipelines") : base(directoryCatalog)
        {
        }
        
        /// <summary>
        /// Create an instance of <see cref="IOutput"/> with the specified configuration
        /// </summary>
        /// <param name="configurationOutput">the configuration of the <see cref="IOutput"/></param>
        /// <param name="loggingConfiguration">the logger configuraiton</param>
        /// <returns></returns>
        public IOutput GetOutput(IConfiguration configurationOutput, IConfigurationSection loggingConfiguration)
        {
            var type = string.IsNullOrWhiteSpace(configurationOutput["OutputType"]) ? "Default" : configurationOutput["OutputType"];
            var output = Container.GetExport<IOutput>(type);
            output?.Initialize(configurationOutput, loggingConfiguration);
            return output;
        }

        public ICollection<IOutput> GetOutputs(IConfigurationSection outputsConfiguration, IConfigurationSection loggingConfiguration)
        {
            var toReturn = new List<IOutput>();
            var outputsCollection = outputsConfiguration.GetChildren()?.ToArray();
            if (outputsCollection != null && outputsCollection.Length > 0)
            {
                foreach (var outputConfiguration in outputsCollection)
                {
                    toReturn.Add(GetOutput(outputConfiguration, loggingConfiguration));
                }
            }
            else
            {
                toReturn.Add(GetOutput(outputsConfiguration,loggingConfiguration));
            }
            return toReturn;
        }
    }
}