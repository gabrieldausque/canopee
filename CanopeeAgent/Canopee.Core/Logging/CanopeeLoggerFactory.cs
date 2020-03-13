using System;
using Canopee.Common;
using Microsoft.Extensions.Configuration;

namespace Canopee.Core.Logging
{
    public class CanopeeLoggerFactory : FactoryFromDirectoryBase
    {
        private static readonly object LockInstance = new object();
        private static CanopeeLoggerFactory _instance;

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
        
        public CanopeeLoggerFactory(string directoryCatalog) : base(directoryCatalog)
        {
        }

        public ICanopeeLogger GetLogger(IConfiguration loggerConfiguration, Type callerType)
        {
            var type = string.IsNullOrWhiteSpace(loggerConfiguration["LoggerType"]) ? "Default" : loggerConfiguration["LoggerType"];
            var logger = Container.GetExport<ICanopeeLogger>(type);
            logger.Initialize(loggerConfiguration, callerType);
            return logger;
        }
    }
}