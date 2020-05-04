using System;
using System.Composition;
using Canopee.Common;
using Canopee.Common.Logging;
using Canopee.Core.Logging;
using Microsoft.Extensions.Configuration;
using Nest;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace Canopee.StandardLibrary.Loggers
{
    /// <summary>
    /// This is the default logger. Log all message to the console with a color code based on the LogLevel.
    ///
    /// Configuration will be :
    ///
    /// <example>
    /// <code>
    ///     {
    ///         ...
    ///        "Canopee": {
    ///        ...
    ///            "Logging": {
    ///                    "LoggerType": "Console"
    ///            },
    ///        ...
    ///       }
    ///     ...
    ///     }
    /// </code>
    /// </example>
    ///
    /// The LoggerType is Console
    /// 
    /// </summary>
    [Export("Console", typeof(ICanopeeLogger))]
    [Export("Default", typeof(ICanopeeLogger))]
    public class ConsoleCanopeeLogger : BaseCanopeeLogger
    {
        /// <summary>
        /// The lock object used to avoid confusion of color mode for each message
        /// </summary>
        private static readonly object LockObject = new object();

        /// <summary>
        /// Log a message with the corresponding log level
        /// </summary>
        /// <param name="message">the message to log</param>
        /// <param name="level">the log level</param>
        /// <param name="memberName">the name of the calling method (set by the runtime)</param>
        /// <param name="sourceFilePath">the source file of the calling method (set by the runtime)</param>
        /// <param name="sourceLineNumber">the line number of the source file (set by the runtime)</param>
        public override void Log(string message, LogLevel level = LogLevel.Information, 
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
        {
            try
            {
                var suffixString = $"{DateTime.Now:MM/dd/yyyy HH:mm:ss} {level} {CallerType.FullName} {memberName}";
                switch (level)
                {
                    case LogLevel.Information:
                    case LogLevel.None:
                    {
                        WriteLine($"{suffixString} {message}", ConsoleColor.Blue);
                        break;
                    }
                    case LogLevel.Error:
                    {
                        WriteLine($"{suffixString} {message}", ConsoleColor.Red);
                        break;
                    }
                    case LogLevel.Debug:
                    case LogLevel.Trace:
                    {
                        WriteLine($"{suffixString} {message}", ConsoleColor.Cyan);
                        break;
                    }
                    case LogLevel.Warning:
                    {
                        WriteLine($"{suffixString} {message}", ConsoleColor.Yellow);
                        break;
                    }
                    case LogLevel.Critical:
                    {
                        WriteLine($"{suffixString} {message}", ConsoleColor.Magenta);
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        /// <summary>
        /// Write a line setting the passed consolecolor as foreground color and restore the previous one after.
        /// </summary>
        /// <param name="message">the message to write to the console</param>
        /// <param name="foregroundColor">the foreground color</param>
        private void WriteLine(string message, ConsoleColor foregroundColor)
        {
            lock (LockObject)
            {
                var previousColor = Console.ForegroundColor;
                Console.ForegroundColor = foregroundColor;
                Console.WriteLine(message);
                Console.ForegroundColor = previousColor;
            }            
        }
    }
}