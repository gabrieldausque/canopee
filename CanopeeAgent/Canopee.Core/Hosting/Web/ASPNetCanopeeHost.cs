using System;
using System.Diagnostics;
using Canopee.Common;
using Canopee.Core.Pipelines;
using Microsoft.Extensions.Configuration;

namespace Canopee.Core.Hosting.Web
{
    public class ASPNetCanopeeHost : BaseCanopeeHost
    {
        protected readonly CollectPipelineManager CollectPipelineManager;
        
        public ASPNetCanopeeHost(IConfiguration configuration)
        {
            CollectPipelineManager = new CollectPipelineManager();
            HostTrigger =
                TriggerFactory.Instance().GetTrigger(configuration.GetSection("Canopee").GetSection("Trigger"));
        }

        public ITrigger HostTrigger { get; private set; }


        private bool _disposed =false;
        public override void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public override void Run()
        {
            base.Run();
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

        public override void Stop()
        {
            Logger.LogInfo("Stopping the trigger and the pipeline manager");
            HostTrigger.Stop();
            CollectPipelineManager.Stop();
            Logger.LogInfo("Stopped the trigger and the pipeline manager");
            Logger.LogInfo("Exiting the process");
            Process.GetCurrentProcess().Kill();
        }
        
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