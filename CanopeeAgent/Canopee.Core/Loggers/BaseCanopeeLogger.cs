using System;
using Canopee.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Canopee.Core.Logging
{
    public abstract class BaseCanopeeLogger : ICanopeeLogger
    {
        protected Type CallerType = typeof(object);
        
        public virtual void Initialize(IConfiguration loggerConfiguration, Type callerType)
        {
            CallerType = callerType;
        }

        public abstract void Log(string message, LogLevel level = LogLevel.Information, 
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0);
        
        public void LogInfo(string message, [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
        {
            Log(message, LogLevel.Information, memberName, sourceFilePath, sourceLineNumber);
        }

        public void LogError(string message, [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
        {
            Log(message, LogLevel.Error, memberName, sourceFilePath, sourceLineNumber);
        }

        public void LogWarning(string message, [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
        {
            Log(message, LogLevel.Warning, memberName, sourceFilePath, sourceLineNumber);
        }

        public void LogCritical(string message, [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
        {
            Log(message, LogLevel.Critical, memberName, sourceFilePath, sourceLineNumber);
        }

        public void LogDebug(string message, [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
        {
            Log(message, LogLevel.Debug, memberName, sourceFilePath, sourceLineNumber);
        }
    }
}