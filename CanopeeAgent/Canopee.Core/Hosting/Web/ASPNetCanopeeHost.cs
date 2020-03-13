using System;
using Canopee.Common;
using Canopee.Core.Pipelines;
using Microsoft.Extensions.Configuration;

namespace Canopee.Core.Hosting.Web
{
    public class ASPNetCanopeeHost : BaseCanopeeHost
    {
        private CollectPipelineManager _collectPipelineManager;
        
        public ASPNetCanopeeHost(IConfiguration configuration)
        {
            _collectPipelineManager = new CollectPipelineManager();
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
            Logger.Log("Starting the trigger and the pipeline manager");
            HostTrigger.Start();
            _collectPipelineManager.Run();
            Logger.Log("Pipeline manager Started");
        }

        public override void Stop()
        {
            Logger.Log("Stopping the trigger and the pipeline manager");
            HostTrigger.Stop();
            _collectPipelineManager.Stop();
            Logger.Log("Stopped the trigger and the pipeline manager");
        }
        
        private void Dispose(bool disposing)
        {
            if (_disposed) return;
            if (disposing)
            {
                _collectPipelineManager?.Dispose();
                HostTrigger?.Dispose();
            }
        }
    }
}