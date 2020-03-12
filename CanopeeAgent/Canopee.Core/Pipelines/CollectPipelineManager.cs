using System;
using System.Collections.Generic;
using System.Linq;
using Canopee.Common;
using Canopee.Core.Configuration;

namespace Canopee.Core.Pipelines
{
    public class CollectPipelineManager : IDisposable
    {
        private readonly Dictionary<string, ICollectPipeline> _pipelines;

        public CollectPipelineManager()
        {
            _pipelines = new Dictionary<string, ICollectPipeline>();
            var collectPipelinesFactory = new CollectPipelineFactory();

            var config = ConfigurationService.Instance
                .Configuration.GetSection("Pipelines").GetChildren();

            Console.WriteLine($"{config.Count()} Pipelines to read");
            foreach (var pipelineConfig in config)
            {
                Console.WriteLine($"Reading Pipeline {pipelineConfig["Name"]}");
                var pipeline = collectPipelinesFactory.GetPipeline(pipelineConfig);
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

        protected bool Disposed = false; 
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (Disposed) return;

            if (disposing)
            {
                foreach (var pipeline in _pipelines)
                {
                    pipeline.Value?.Dispose();
                }
                _pipelines.Clear();
                Disposed = true;
            }
        }
    }
}