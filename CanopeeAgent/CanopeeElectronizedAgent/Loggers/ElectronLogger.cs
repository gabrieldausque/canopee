using System;
using System.Composition;
using System.Linq;
using Canopee.Common;
using Canopee.Core.Logging;
using ElectronNET.API;
using Microsoft.Extensions.Logging;

namespace CanopeeElectronizedAgent.Loggers
{
    [Export("Electron", typeof(ICanopeeLogger))]
    public class ElectronLogger : BaseCanopeeLogger
    {
        public override void Log(string message, LogLevel level = LogLevel.Information, string memberName = "", string sourceFilePath = "",
            int sourceLineNumber = 0)
        {
            try
            {
                var browserWindow = Electron.WindowManager.BrowserWindows.FirstOrDefault();
                if (browserWindow != null)
                {
                    var formattedMessage =
                        $"<div class=\"canopeelog-{level.ToString().ToLower()}\">{DateTime.Now:MM/dd/yyyy HH:mm:ss} {level} {CallerType.FullName} {memberName} {message}</div>";
                    Electron.IpcMain.Send(browserWindow, "canopee-logs", formattedMessage);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while logging to ipcrenderer : {ex}");
            }
        }
    }
}