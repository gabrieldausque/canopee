using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using Canopee.Common;
using Canopee.Core.Configuration;
using Canopee.Core.Logging;
using Microsoft.Extensions.Configuration;

namespace Canopee.Core.Hosting
{
    public abstract class BaseCanopeeHost : ICanopeeHost
    {
        protected ICanopeeLogger Logger = null;
        protected bool CanRun = false;
        public BaseCanopeeHost()
        {
            var configuration = ConfigurationService.Instance.GetLoggingConfiguration();
            Logger = CanopeeLoggerFactory.Instance().GetLogger(configuration, this.GetType());   
        }

        public virtual void Dispose()
        {
        }

        public virtual void Run()
        {
            Logger.LogInfo("Check if a process already exists");
            SetCanRun();
        }

        protected void SetCanRun()
        {
            if (Configuration.ConfigurationService.Instance.IsUniqueInstance())
            {
                var currentProcess = Process.GetCurrentProcess();
                foreach (var p in Process.GetProcessesByName(currentProcess.ProcessName))
                {
                    if (p.Id != currentProcess.Id)
                    {
                        Logger.LogInfo(
                            $"Other instance of {currentProcess.ProcessName} is running with pid {p.Id.ToString()}.Stopping ...");
                        CanRun = false;
                        return;
                    }
                }
            }
            CanRun = true;
        }

        public abstract void Stop();
    }
}