using System;
using Canopee.Common;
using Canopee.Common.Logging;
using Microsoft.Extensions.Configuration;

namespace Canopee.Core.Logging
{
    /// <summary>
    /// Factory that create <see cref="ICanopeeLogger"/> object
    /// </summary>
    public class CanopeeLoggerFactory : FactoryFromDirectoryBase
    {
        /// <summary>
        /// The singleton lock object
        /// </summary>
        private static readonly object LockInstance = new object();
        
        /// <summary>
        /// The singleton instance
        /// </summary>
        private static CanopeeLoggerFactory _instance;

        /// <summary>
        /// Get the singleton instance initialized with the specified directory catalog
        /// </summary>
        /// <param name="directoryCatalog">
        /// The directory to read all assemblies from to get <see cref="ICanopeeLogger"/> catalog
        /// Default: ./Pipelines
        /// </param>
        /// <returns></returns>
        public static CanopeeLoggerFactory Instance(string directoryCatalog=@"./Pipelines")
        {
            lock (LockInstance)
            {
                if (_instance == null)
                {
                    _instance = new CanopeeLoggerFactory(directoryCatalog);
                }
            }
            return _instance;
        }
        
        /// <summary>
        /// DEfault constructor
        /// </summary>
        /// <param name="directoryCatalog">
        /// The directory to read all assemblies from to get <see cref="ICanopeeLogger"/> catalog
        /// Default: ./Pipelines
        /// </param>
        public CanopeeLoggerFactory(string directoryCatalog) : base(directoryCatalog)
        {
        }

        /// <summary>
        /// Get the logger for the specified Logger configuration. If no LoggerType specified, used Default for contract name
        /// </summary>
        /// <param name="loggerConfiguration">The logger configuration</param>
        /// <param name="callerType">The caller type</param>
        /// <returns></returns>
        public ICanopeeLogger GetLogger(IConfiguration loggerConfiguration, Type callerType)
        {
            var type = string.IsNullOrWhiteSpace(loggerConfiguration["LoggerType"]) ? "Default" : loggerConfiguration["LoggerType"];
            var logger = Container.GetExport<ICanopeeLogger>(type);
            logger.Initialize(loggerConfiguration, callerType);
            return logger;
        }
    }
}