using System;
using System.Collections.Generic;
using System.Composition;
using Canopee.Common;
using Canopee.Core.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Canopee.StandardLibrary.Loggers
{
    [Export("MultiLogger", typeof(ICanopeeLogger))]
    public class MultiCanopeeLogger : BaseCanopeeLogger
    {
        private List<ICanopeeLogger> _loggers;

        public MultiCanopeeLogger()
        {
            _loggers = new List<ICanopeeLogger>();
        }

        public override void Initialize(IConfiguration loggerConfiguration, Type callerType)
        {
            base.Initialize(loggerConfiguration, callerType);
            foreach (var loggerConf in loggerConfiguration.GetSection("Loggers").GetChildren())
            {
                _loggers.Add(CanopeeLoggerFactory.Instance().GetLogger(loggerConf, callerType));
            }
        }

        public override void Log(string message, LogLevel level = LogLevel.Information, string memberName = "", string sourceFilePath = "",
            int sourceLineNumber = 0)
        {
            foreach (var innerLogger in _loggers)
            {
                try
                {
                    innerLogger.Log(message, level, memberName, sourceFilePath, sourceLineNumber);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error when logging : {ex}");
                }
            }
        }
    }
}