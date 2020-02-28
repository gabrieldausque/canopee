using System;
using System.Collections.Generic;
using Canopee.Common;
using Canopee.Common.Configuration;

namespace Canopee.Core.Pipelines
{
    public class CollectPipelineManager : IDisposable
    {
        private Dictionary<string, ICollectPipeline> _pipelines;
        private CollectPipelineFactory _collectPipelinesFactory;

        public CollectPipelineManager()
        {
            _pipelines = new Dictionary<string, ICollectPipeline>();
            _collectPipelinesFactory = new CollectPipelineFactory();

            var config = ConfigurationService.Instance
                .Configuration.GetSection("Indicators").GetChildren();

            foreach (var pipelineConfig in config)
            {
                var pipeline = _collectPipelinesFactory.GetIndicator(pipelineConfig);
                _pipelines.Add(pipelineConfig["Name"], pipeline);
            }
        }

        public void Run()
        {
            foreach (var pipeline in _pipelines)
            {
                pipeline.Value.Run();
            }
        }

        public void Stop()
        {
            foreach (var pipeline in _pipelines)
            {
                pipeline.Value.Stop();
            }
        }

        protected bool _disposed = false; 
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                foreach (var pipeline in _pipelines)
                {
                    pipeline.Value?.Dispose();
                }
                _pipelines.Clear();
                _disposed = true;
            }
        }
    }
}