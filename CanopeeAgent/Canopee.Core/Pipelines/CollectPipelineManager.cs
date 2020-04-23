using System;
using System.Collections.Generic;
using System.Linq;
using Canopee.Common;
using Canopee.Common.Logging;
using Canopee.Common.Pipelines;
using Canopee.Core.Configuration;
using Canopee.Core.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Canopee.Core.Pipelines
{
    /// <summary>
    /// Manage all pipelines :
    /// - create them
    /// - 
    /// </summary>
    public class CollectPipelineManager : IDisposable
    {
        private readonly Dictionary<string, ICollectPipeline> _pipelines;
        private readonly ICanopeeLogger Logger = null;

        public CollectPipelineManager(IConfigurationSection pipelinesConfiguration, IConfigurationSection loggingConfiguration)
        {
            Logger = CanopeeLoggerFactory.Instance().GetLogger(loggingConfiguration, this.GetType());
            _pipelines = new Dictionary<string, ICollectPipeline>();
            var collectPipelinesFactory = new CollectPipelineFactory();

            var config = pipelinesConfiguration.GetChildren();
            
            Logger.LogInfo($"{config.ToArray().Length.ToString()} Pipelines to read");
            foreach (var pipelineConfig in config)
            {
                Logger.LogInfo($"Reading Pipeline {pipelineConfig["Name"]}");
                var pipeline = collectPipelinesFactory.GetPipeline(pipelineConfig, loggingConfiguration);
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
                Logger.LogInfo($"Starting pipeline {pipeline.ToString()}");
                pipeline.Value.Run();
            }
        }

        public void Stop()
        {
            foreach (var pipeline in _pipelines)
            {
                Logger.LogInfo($"Stopping pipeline {pipeline.ToString()}");
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