using System;
using System.Collections.Generic;
using System.Composition;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using Canopee.Common;
using log4net;
using log4net.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Canopee.StandardLibrary.Loggers
{
    [Export("Log4Net",typeof(ICanopeeLogger) )]
    [Shared]
    public class Log4NetCanopeeLogger:ICanopeeLogger
    {
        private static readonly object LockObject = new object();
        private static bool _isInitialized = false;
        private Dictionary<string,log4net.ILog> _loggers = new Dictionary<string,ILog>();
        private Type _callerType = typeof(object);
        public void Initialize(IConfiguration loggerConfiguration, Type callerType)
        {
            lock (LockObject)
            {
                if (!_isInitialized)
                {
                    _callerType??=callerType;
                    var log4NetConfigurationFileName = string.IsNullOrWhiteSpace(loggerConfiguration["configurationFile"])?"log4net.config":loggerConfiguration["configurationFile"];
                    var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
                    XmlConfigurator.ConfigureAndWatch(logRepository, new FileInfo(log4NetConfigurationFileName));
                    _isInitialized = true;
                }
            }
        }

        public void Log(string message, LogLevel level, 
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
        {
            if (_loggers.TryGetValue(_callerType.FullName, out var logger))
            {
                logger = LogManager.GetLogger(this.GetType());
                _loggers.Add(_callerType.FullName, logger);
            }
            switch (level)
            {
                case LogLevel.Information:
                case LogLevel.None:    
                {
                    logger?.Info(message);
                    break;
                }
                case LogLevel.Error:
                {
                    logger?.Error(message);
                    break;
                }
                case LogLevel.Debug:
                case LogLevel.Trace:
                {
                    logger?.Debug(message);
                    break;
                }
                case LogLevel.Critical:
                {
                    logger?.Fatal(message);
                    break;
                }
                case LogLevel.Warning:
                {
                    logger?.Warn(message);
                    break;
                }
            }
        }
    }
}