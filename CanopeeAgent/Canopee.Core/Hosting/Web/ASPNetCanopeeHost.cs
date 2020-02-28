using System;
using Canopee.Common;
using Canopee.Core.Pipelines;
using Microsoft.Extensions.Configuration;

namespace Canopee.Core.Hosting.Web
{
    public class ASPNetCanopeeHost : ICanopeeHost
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
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Run()
        {
            _collectPipelineManager.Run();
        }

        public void Stop()
        {
            _collectPipelineManager.Stop();
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