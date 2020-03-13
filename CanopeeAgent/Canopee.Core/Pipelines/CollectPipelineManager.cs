using System;
using System.Collections.Generic;
using System.Linq;
using Canopee.Common;
using Canopee.Core.Configuration;
using Canopee.Core.Logging;
using Microsoft.Extensions.Logging;

namespace Canopee.Core.Pipelines
{
    public class CollectPipelineManager : IDisposable
    {
        private static ICanopeeLogger _canopeeLogger = null;
        private readonly Dictionary<string, ICollectPipeline> _pipelines;
        private readonly ICanopeeLogger Logger = null;

        public CollectPipelineManager()
        {
            var loggingConfiguration = ConfigurationService.Instance.GetCanopeeConfiguration().GetSection("Logging");
            Logger = CanopeeLoggerFactory.Instance().GetLogger(loggingConfiguration, this.GetType());
            _pipelines = new Dictionary<string, ICollectPipeline>();
            var collectPipelinesFactory = new CollectPipelineFactory();

            var config = ConfigurationService.Instance
                .Configuration.GetSection("Pipelines").GetChildren();

            Logger.Log($"{config.Count()} Pipelines to read");
            foreach (var pipelineConfig in config)
            {
                Logger.Log($"Reading Pipeline {pipelineConfig["Name"]}");
                var pipeline = collectPipelinesFactory.GetPipeline(pipelineConfig);
                _pipelines.Add(pipelineConfig["Name"], pipeline);
            }
        }

        public void Run()
        {
            foreach (var pipeline in _pipelines)
            {
                Logger.Log($"Starting pipeline {pipeline}");
                pipeline.Value.Run();
            }
        }

        public void Stop()
        {
            foreach (var pipeline in _pipelines)
            {
                Logger.Log($"Stopping pipeline {pipeline}");
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