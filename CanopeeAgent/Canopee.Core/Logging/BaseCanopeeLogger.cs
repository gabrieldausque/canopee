using System;
using Canopee.Common;
using Canopee.Common.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Canopee.Core.Logging
{
    /// <summary>
    /// Base implementation of the ICanopeeLogger. Simplify implementation of other Logger as it implements shortcut for Log Method with standard LogLevel
    /// </summary>
    public abstract class BaseCanopeeLogger : ICanopeeLogger
    {
        /// <summary>
        /// The caller Type
        /// Default: typeof(Object)
        /// </summary>
        protected Type CallerType = typeof(object);
        
        /// <summary>
        /// Initialize the logger
        /// </summary>
        /// <param name="loggerConfiguration">The specific logger configuration</param>
        /// <param name="callerType">the type of object that will use this logger</param>
        public virtual void Initialize(IConfiguration loggerConfiguration, Type callerType)
        {
            CallerType = callerType;
        }

        /// <summary>
        /// Log a message with the specified LogLevel
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="level">The LogLevel</param>
        /// <param name="memberName">the function caller. Defaulted by the compiler</param>
        /// <param name="sourceFilePath">the source file path of the function caller if available. Defaulted by the compiler</param>
        /// <param name="sourceLineNumber">the source line number. Defaulted by the compiler</param>
        public abstract void Log(string message, LogLevel level = LogLevel.Information, 
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0);
        
        /// <summary>
        /// Log with LogLevel.Info
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="memberName">the function caller. Defaulted by the compiler</param>
        /// <param name="sourceFilePath">the source file path of the function caller if available. Defaulted by the compiler</param>
        /// <param name="sourceLineNumber">the source line number. Defaulted by the compiler</param>
        public void LogInfo(string message, [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
        {
            Log(message, LogLevel.Information, memberName, sourceFilePath, sourceLineNumber);
        }

        /// <summary>
        /// Log with LogLevel.Error
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="memberName">the function caller. Defaulted by the compiler</param>
        /// <param name="sourceFilePath">the source file path of the function caller if available. Defaulted by the compiler</param>
        /// <param name="sourceLineNumber">the source line number. Defaulted by the compiler</param>
        public void LogError(string message, [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
        {
            Log(message, LogLevel.Error, memberName, sourceFilePath, sourceLineNumber);
        }

        /// <summary>
        /// Log with LogLevel.Warning
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="memberName">the function caller. Defaulted by the compiler</param>
        /// <param name="sourceFilePath">the source file path of the function caller if available. Defaulted by the compiler</param>
        /// <param name="sourceLineNumber">the source line number. Defaulted by the compiler</param>
        public void LogWarning(string message, [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
        {
            Log(message, LogLevel.Warning, memberName, sourceFilePath, sourceLineNumber);
        }

        /// <summary>
        /// Log with LogLevel.Critical
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="memberName">the function caller. Defaulted by the compiler</param>
        /// <param name="sourceFilePath">the source file path of the function caller if available. Defaulted by the compiler</param>
        /// <param name="sourceLineNumber">the source line number. Defaulted by the compiler</param>
        public void LogCritical(string message, [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
        {
            Log(message, LogLevel.Critical, memberName, sourceFilePath, sourceLineNumber);
        }

        /// <summary>
        /// Log with LogLevel.Debug
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="memberName">the function caller. Defaulted by the compiler</param>
        /// <param name="sourceFilePath">the source file path of the function caller if available. Defaulted by the compiler</param>
        /// <param name="sourceLineNumber">the source line number. Defaulted by the compiler</param>
        public void LogDebug(string message, [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
        {
            Log(message, LogLevel.Debug, memberName, sourceFilePath, sourceLineNumber);
        }
    }
}