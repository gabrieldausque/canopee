using System;
using System.Diagnostics;
using Canopee.Common;
using Canopee.Common.Pipelines;
using Canopee.Core.Pipelines;
using Microsoft.Extensions.Configuration;

namespace Canopee.Core.Hosting.Web
{
    /// <summary>
    /// The Host needed for AspNet core hosting of the Canopee pipeline framework
    /// </summary>
    public class ASPNetCanopeeHost : BaseCanopeeHost
    {
        /// <summary>
        /// The pipeline manager
        /// </summary>
        protected readonly CollectPipelineManager CollectPipelineManager;
        
        /// <summary>
        /// Construct a new instance using the passed configuration
        /// </summary>
        /// <param name="canopeeConfiguration">The Canopee section configuration</param>
        public ASPNetCanopeeHost(IConfigurationSection canopeeConfiguration):base(canopeeConfiguration.GetSection("Logging"))
        {
            CollectPipelineManager = new CollectPipelineManager(canopeeConfiguration.GetSection("Pipelines"),canopeeConfiguration.GetSection("Logging"));
            HostTrigger =
                TriggerFactory.Instance().GetTrigger(canopeeConfiguration.GetSection("Trigger"), canopeeConfiguration.GetSection("Logging"));
        }

        /// <summary>
        /// The host trigger used that will be shared to external consumer (for example controller that will start trigger on REST call)
        /// </summary>
        public ITrigger HostTrigger { get; private set; }
       
        /// <summary>
        /// Start the host trigger and the pipeline manager
        /// </summary>
        public override void Start()
        {
            base.Start();
            if (CanRun)
            {
                Logger.LogInfo("Starting the trigger and the pipeline manager");
                HostTrigger.Start();
                CollectPipelineManager.Run();
                Logger.LogInfo("Pipeline manager Started");
            }
            else
            {
                Logger.LogWarning("Trigger and the pipeline manager not started");
                this.Stop();
            }
        }

        /// <summary>
        /// Stop host trigger and pipeline manager
        /// </summary>
        public override void Stop()
        {
            Logger.LogInfo("Stopping the trigger and the pipeline manager");
            HostTrigger.Stop();
            CollectPipelineManager.Stop();
            Logger.LogInfo("Stopped the trigger and the pipeline manager");
            Logger.LogInfo("Exiting the process");
            Process.GetCurrentProcess().Kill();
        }

        /// <summary>
        /// The dispose internal flag
        /// </summary>
        private bool _disposed =false;
        
        /// <summary>
        /// Dispose the current host
        /// </summary>
        public override void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        
        /// <summary>
        /// Dispose hosttrigger, pipelinemanager and all other components that needs to be disposed
        /// </summary>
        /// <param name="disposing"></param>
        private void Dispose(bool disposing)
        {
            if (_disposed) return;
            if (disposing)
            {
                CollectPipelineManager?.Dispose();
                HostTrigger?.Dispose();
            }
        }
    }
}