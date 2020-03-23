using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Canopee.Core.Hosting.Web;
using ElectronNET.API;
using ElectronNET.API.Entities;
using Microsoft.Extensions.Configuration;

namespace CanopeeElectronizedAgent.Host
{
    public class CanopeeElectronHost : ASPNetCanopeeHost
    {
        public CanopeeElectronHost(IConfiguration configuration) : base(configuration)
        {
            
        }

        public async void CreateElectronRenderer ()
        {
            try
            {
                Electron.App.BrowserWindowCreated += () =>
                {
                    var iconPath = Path.GetFullPath("./wwwroot/images/NotifyIcon.png");
                    Logger.Log($"Loading icon from {iconPath}");
                    if (Electron.Tray.MenuItems.Count == 0)
                    {
                        MenuItem[] menu =
                        {
                            new MenuItem(){
                                Label = "Show Canopee Agent",
                                Click = () => {
                                    Electron.WindowManager.BrowserWindows.FirstOrDefault()?.Show();
                                }
                            },
                            new MenuItem(){
                                Label = "Exit Canopee Agent",
                                Click = () => {
                                    Electron.App.Exit();
                                    Process.GetCurrentProcess().Kill();
                                }
                            }   
                        };
                        Electron.Tray.Show(iconPath, menu);
                        Electron.Tray.SetToolTip("Canopee Agent Menu");
                    }
                };
               await Electron.WindowManager.CreateWindowAsync(
                    new BrowserWindowOptions()
                    {
                        Show = false
                    });
                foreach (var window in Electron.WindowManager.BrowserWindows)
                {
                    Logger.LogDebug("Subscribing to close event");
                    window.OnClosed += () =>
                    {
                        Stop();
                        Electron.App.Exit();
                        System.Diagnostics.Process.GetCurrentProcess().Kill();
                    };
                    await window.WebContents.Session.ClearCacheAsync();
                }
            }
            catch (Exception e)
            {
                Logger.LogError($"Error while creating electron renderer ${e}");
                throw;
            }
        }
    }
}