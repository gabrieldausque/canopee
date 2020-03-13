using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Canopee.Common
{
    public interface ICanopeeLogger
    {
        void Initialize(IConfiguration loggerConfiguration, Type callerType);
        
        void Log(string message, LogLevel level = LogLevel.Information,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0);
    }
}