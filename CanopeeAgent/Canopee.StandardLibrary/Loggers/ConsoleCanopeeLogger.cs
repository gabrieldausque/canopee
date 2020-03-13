using System;
using System.Composition;
using Canopee.Common;
using Microsoft.Extensions.Configuration;
using Nest;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace Canopee.StandardLibrary.Loggers
{
    [Export("Console", typeof(ICanopeeLogger))]
    [Export("Default", typeof(ICanopeeLogger))]
    public class ConsoleCanopeeLogger : ICanopeeLogger
    {
        private static readonly object LockObject = new object();
        private Type _callerType;

        public void Initialize(IConfiguration loggerConfiguration, Type callerType)
        {
            _callerType = callerType;
        }

        public void Log(string message, LogLevel level = LogLevel.Information, 
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
        {
            var suffixString = $"{DateTime.Now:MM/dd/yyyy HH:mm:ss} {_callerType.FullName} {memberName} {level} ";
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