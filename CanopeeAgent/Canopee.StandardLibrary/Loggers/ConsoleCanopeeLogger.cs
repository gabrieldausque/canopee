using System;
using System.Composition;
using Canopee.Common;
using Canopee.Core.Logging;
using Microsoft.Extensions.Configuration;
using Nest;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace Canopee.StandardLibrary.Loggers
{
    [Export("Console", typeof(ICanopeeLogger))]
    [Export("Default", typeof(ICanopeeLogger))]
    public class ConsoleCanopeeLogger : BaseCanopeeLogger
    {
        private static readonly object LockObject = new object();

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