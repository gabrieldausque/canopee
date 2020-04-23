using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using Canopee.Common;
using Canopee.Common.Hosting;
using Canopee.Common.Logging;
using Canopee.Core.Configuration;
using Canopee.Core.Logging;
using Microsoft.Extensions.Configuration;

namespace Canopee.Core.Hosting
{
    /// <summary>
    /// Abstract class that reduce host implementation time
    /// </summary>
    public abstract class BaseCanopeeHost : ICanopeeHost
    {
        /// <summary>
        /// The internal logger
        /// </summary>
        protected ICanopeeLogger Logger = null;
        
        /// <summary>
        /// Check if the instance can run 
        /// </summary>
        protected bool CanRun = false;
        
        /// <summary>
        /// Default constructor. Instanciate the logger in it
        /// </summary>
        public BaseCanopeeHost(IConfigurationSection loggingConfiguration)
        {
            Logger = CanopeeLoggerFactory.Instance().GetLogger(loggingConfiguration, this.GetType());   
        }

        /// <summary>
        /// Dispose the host. Currently do nothing. Override it in your host implementation.
        /// </summary>
        public virtual void Dispose()
        {
        }

        /// <summary>
        /// Starting the current host. Set the CanRun property and start the ConfigurationService (for synchronization) 
        /// </summary>
        public virtual void Start()
        {
            Logger.LogInfo("Check if a process already exists");
            SetCanRun();
            Logger.LogInfo("Start configuration synchronisation if needed");
            ConfigurationService.Instance.Start();
        }

        /// <summary>
        /// Set the CanRun property to true if UniqueInstance configuration is set to false or if no other process with the same name is running
        /// </summary>
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

        /// <summary>
        /// Stop the current process
        /// </summary>
        public abstract void Stop();
    }
}