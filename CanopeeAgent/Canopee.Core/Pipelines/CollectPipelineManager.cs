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
        private readonly Dictionary<string, ICollectPipeline> _pipelines;
        private readonly ICanopeeLogger Logger = null;

        public CollectPipelineManager()
        {
            var loggingConfiguration = ConfigurationService.Instance.GetLoggingConfiguration();
            Logger = CanopeeLoggerFactory.Instance().GetLogger(loggingConfiguration, this.GetType());
            _pipelines = new Dictionary<string, ICollectPipeline>();
            var collectPipelinesFactory = new CollectPipelineFactory();

            var config = ConfigurationService.Instance.GetPipelinesConfiguration().GetChildren();

            Logger.LogInfo($"{config.Count()} Pipelines to read");
            foreach (var pipelineConfig in config)
            {
                Logger.LogInfo($"Reading Pipeline {pipelineConfig["Name"]}");
                var pipeline = collectPipelinesFactory.GetPipeline(pipelineConfig);
                if (!_pipelines.ContainsKey(pipelineConfig["Name"]))
                {
                    _pipelines.Add(pipelineConfig["Name"], pipeline);    
                }
                else
                {
                    Logger.LogWarning($"A pipeline with name {pipelineConfig["Name"]} already exists. Replacing with the new one");
                    _pipelines[pipelineConfig["Name"]] = pipeline;
                }
                
            }
        }

        public void Run()
        {
            foreach (var pipeline in _pipelines)
            {
                Logger.LogInfo($"Starting pipeline {pipeline}");
                pipeline.Value.Run();
            }
        }

        public void Stop()
        {
            foreach (var pipeline in _pipelines)
            {
                Logger.LogInfo($"Stopping pipeline {pipeline}");
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