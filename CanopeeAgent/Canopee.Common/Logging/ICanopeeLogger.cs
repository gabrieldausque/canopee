using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Canopee.Common.Logging
{
    /// <summary>
    /// Contract for the object that will log information in object
    /// </summary>
    public interface ICanopeeLogger
    {
        /// <summary>
        /// Initialize the logger object
        /// </summary>
        /// <param name="loggerConfiguration">the logger configuration</param>
        /// <param name="callerType">the type of the object that will call the logger</param>
        void Initialize(IConfiguration loggerConfiguration, Type callerType);
        
        /// <summary>
        /// Log a message with the specified log level
        /// </summary>
        /// <param name="message">the message to log</param>
        /// <param name="level">the <see cref="LogLevel"/> of the message</param>
        /// <param name="memberName">the member name specified on runtime</param>
        /// <param name="sourceFilePath">the source path specified on runtime</param>
        /// <param name="sourceLineNumber">the source line number specified on runtime</param>
        void Log(string message, LogLevel level = LogLevel.Information,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0);

        /// <summary>
        /// Log a message with info level
        /// </summary>
        /// <param name="message">the message to log</param>
        /// <param name="memberName">the member name specified on runtime</param>
        /// <param name="sourceFilePath">the source path specified on runtime</param>
        /// <param name="sourceLineNumber">the source line number specified on runtime</param>
        void LogInfo(string message,
            [System.Runtime.CompilerServices.CallerMemberName]
            string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath]
            string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber]
            int sourceLineNumber = 0);

        /// <summary>
        /// Log a message with error level
        /// </summary>
        /// <param name="message">the message to log</param>
        /// <param name="memberName">the member name specified on runtime</param>
        /// <param name="sourceFilePath">the source path specified on runtime</param>
        /// <param name="sourceLineNumber">the source line number specified on runtime</param>
        void LogError(string message,
            [System.Runtime.CompilerServices.CallerMemberName]
            string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath]
            string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber]
            int sourceLineNumber = 0);
        
        /// <summary>
        /// Log a message with warning level
        /// </summary>
        /// <param name="message">the message to log</param>
        /// <param name="memberName">the member name specified on runtime</param>
        /// <param name="sourceFilePath">the source path specified on runtime</param>
        /// <param name="sourceLineNumber">the source line number specified on runtime</param>
        void LogWarning(string message,
            [System.Runtime.CompilerServices.CallerMemberName]
            string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath]
            string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber]
            int sourceLineNumber = 0);
        
        /// <summary>
        /// Log a message with critical level
        /// </summary>
        /// <param name="message">the message to log</param>
        /// <param name="memberName">the member name specified on runtime</param>
        /// <param name="sourceFilePath">the source path specified on runtime</param>
        /// <param name="sourceLineNumber">the source line number specified on runtime</param>
        void LogCritical(string message,
            [System.Runtime.CompilerServices.CallerMemberName]
            string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath]
            string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber]
            int sourceLineNumber = 0);
        
        /// <summary>
        /// Log a message with debug level
        /// </summary>
        /// <param name="message">the message to log</param>
        /// <param name="memberName">the member name specified on runtime</param>
        /// <param name="sourceFilePath">the source path specified on runtime</param>
        /// <param name="sourceLineNumber">the source line number specified on runtime</param>
        void LogDebug(string message,
            [System.Runtime.CompilerServices.CallerMemberName]
            string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath]
            string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber]
            int sourceLineNumber = 0);
    }
    
    
}