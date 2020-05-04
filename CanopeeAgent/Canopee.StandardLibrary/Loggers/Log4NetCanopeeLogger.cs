using System;
using System.Collections.Generic;
using System.Composition;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using Canopee.Common;
using Canopee.Common.Logging;
using Canopee.Core.Logging;
using log4net;
using log4net.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Canopee.StandardLibrary.Loggers
{
    /// <summary>
    /// A Log4net logger wrapper, by default will use the log4net.config file that will exists in the working directory.
    ///
    /// The configuration will be :
    ///
    /// <example>
    /// <code>
    ///     {
    ///         ...
    ///        "Canopee": {
    ///        ...
    ///            "Logging": {
    ///                "LoggerType": "Log4Net",
    ///                "configurationFile":"mylog4netconfigfile.conf"     
    ///            },
    ///        ...
    ///       }
    ///     ...
    ///     }
    /// </code>
    /// </example>
    ///
    /// The LoggerType is Log4Net
    /// configurationFile attribute is optional. it define the name of the log4net configuration file. By default it is log4net.config.
    /// 
    /// </summary>
    [Export("Log4Net",typeof(ICanopeeLogger) )]
    [Shared]
    public class Log4NetCanopeeLogger:BaseCanopeeLogger
    {
        private static readonly object LockObject = new object();
        private static bool _isInitialized = false;
        private readonly Dictionary<string,log4net.ILog> _loggers = new Dictionary<string,ILog>();
        private Type _callerType = typeof(object);
        public override void Initialize(IConfiguration loggerConfiguration, Type callerType)
        {
            base.Initialize(loggerConfiguration, callerType);
            _callerType=(callerType == null)?this.GetType():callerType;
            lock (LockObject)
            {
                if (!_isInitialized)
                {
                    var log4NetConfigurationFileName = string.IsNullOrWhiteSpace(loggerConfiguration["configurationFile"])?"log4net.config":loggerConfiguration["configurationFile"];
                    var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
                    XmlConfigurator.ConfigureAndWatch(logRepository, new FileInfo(log4NetConfigurationFileName));
                    _isInitialized = true;
                }
            }
        }

        public override void Log(string message, LogLevel level, 
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
        {
            try
            {
                log4net.LogicalThreadContext.Properties["CallerType"] = _callerType.FullName;
                if (!_loggers.TryGetValue(_callerType.FullName, out var logger))
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
                log4net.LogicalThreadContext.Properties["CallerType"] = string.Empty;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}