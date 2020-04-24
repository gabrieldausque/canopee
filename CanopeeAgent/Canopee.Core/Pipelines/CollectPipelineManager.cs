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
    /// - start them
    /// - stop them
    /// - dispose them
    /// </summary>
    public class CollectPipelineManager : IDisposable
    {
        /// <summary>
        /// The internal collection for all pipelines
        /// </summary>
        private readonly Dictionary<string, ICollectPipeline> _pipelines;
        
        /// <summary>
        /// The internal <see cref="ICanopeeLogger"/>
        /// </summary>
        private readonly ICanopeeLogger Logger = null;

        /// <summary>
        /// Create the CollectPipelineManager from pipelines and logger configuration
        /// </summary>
        /// <param name="pipelinesConfiguration">the pipelines configuration collection</param>
        /// <param name="loggingConfiguration">the logger configuration</param>
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

        /// <summary>
        /// Start all pipeline
        /// </summary>
        public void Run()
        {
            foreach (var pipeline in _pipelines)
            {
                Logger.LogInfo($"Starting pipeline {pipeline.ToString()}");
                pipeline.Value.Run();
            }
        }

        /// <summary>
        /// Stop all pipelines
        /// </summary>
        public void Stop()
        {
            foreach (var pipeline in _pipelines)
            {
                Logger.LogInfo($"Stopping pipeline {pipeline.ToString()}");
                pipeline.Value.Stop();
            }
        }

        /// <summary>
        /// Internal disposed flag
        /// </summary>
        private bool _disposed = false;
        
        /// <summary>
        /// Dispose the instance
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose the instance and all of its owned pipelines
        /// </summary>
        /// <param name="disposing"></param>
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