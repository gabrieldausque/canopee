using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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

        public override void Start()
        {
            SetCanRun();
            CreateElectronRenderer().Wait();
            base.Start();
        }

        public async Task CreateElectronRenderer ()
        {
            try
            {
                var iconPath = Path.GetFullPath("./wwwroot/images/CanopeeLogo.png");
                Electron.App.BrowserWindowCreated += () =>
                {
                    Logger.Log($"Loading icon from {iconPath}");
                    if (Electron.Tray.MenuItems.Count == 0)
                    {
                        MenuItem[] menu =
                        {
                            new MenuItem(){
                                Label = "Show Canopee Agent",
                                Click = () => {
                                    Electron.WindowManager.BrowserWindows.FirstOrDefault()?.Show();
                                    Electron.WindowManager.BrowserWindows.FirstOrDefault()?.Reload();
                                }
                            },
                            new MenuItem(){
                                Label = "Hide Canopee Agent",
                                Click = () => { Electron.WindowManager.BrowserWindows.FirstOrDefault()?.Hide(); }
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
                        Show = false,
                        Icon = iconPath
                    }); 
                var window = Electron.WindowManager.BrowserWindows.FirstOrDefault();
                Logger.LogDebug("Subscribing to close event");
                window.OnClosed += Stop;
                window.SetMenuBarVisibility(false);
                window.SetMinimumSize(1280, 768);
                window.SetMaximumSize(1280, 768);
                window.SetMinimizable(false);
                window.SetMaximizable(false);
                await window.WebContents.Session.ClearCacheAsync();
                if (!CanRun)
                {
                    Logger.LogWarning($"Host can't run, another instance already running. Exiting");
                    this.Stop();
                }
            }
            catch (Exception e)
            {
                Logger.LogError($"Error while creating electron renderer ${e}");
                throw;
            }
        }

        public override void Stop()
        {
            Logger.LogInfo("Stopping the Electron app");
            Electron.App.Exit();
            base.Stop();        
        }
    }
}