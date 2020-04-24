using System;
using System.Collections.Generic;
using Canopee.Common.Pipelines.Events;
using Microsoft.Extensions.Configuration;

namespace Canopee.Common.Pipelines
{
    /// <summary>
    /// The interface for an extract, transform and load process that is the heart of Canopee platform.
    /// </summary>
    public interface ICollectPipeline : IDisposable
    {
        /// <summary>
        /// The name of the current <see cref="ICollectPipeline"/>
        /// </summary>
        public string Name { get; }
        
        /// <summary>
        /// The id of the current <see cref="ICollectPipeline"/>. Must be unique over the application.
        /// </summary>
        public string Id { get; }
        
        /// <summary>
        /// Initialize the pipeline based on a pipelineConfigurationSection object
        /// </summary>
        /// <param name="pipelineConfigurationSection">the pipeline pipelineConfigurationSection</param>
        /// <param name="loggingConfiguration">the logging configuration section</param>
        void Initialize(IConfigurationSection pipelineConfigurationSection, IConfigurationSection loggingConfiguration);
        /// <summary>
        /// This method will start immediately the collect, transform and load of one or more <see cref="ICollectedEvent"/>. This will be triggered by the trigger of the pipeline
        /// </summary>
        /// <param name="fromTriggerArgs"></param>
        void Collect(TriggerEventArgs fromTriggerArgs);
        /// <summary>
        /// Start to listen to the trigger to make the collect of <see cref="ICollectedEvent"/> wanted
        /// </summary>
        void Start();
        /// <summary>
        /// Stop to listen to the trigger
        /// </summary>
        void Stop();
        /// <summary>
        /// The <see cref="IInput"/> that will collect one or more <see cref="ICollectedEvent"/> when the trigger is raised
        /// </summary>
        IInput Input { get; set; }
        /// <summary>
        /// The collection of <see cref="ITransform"/> that will complete extracted information from <see cref="ICollectedEvent"/> collected by the <see cref="ICollectPipeline.Input"/>
        /// </summary>
        ICollection<ITransform> Transforms { get; set; }
        /// <summary>
        /// The collection of <see cref="IOutput"/> where to send a <see cref="ICollectedEvent"/>
        /// </summary>
        ICollection<IOutput> Outputs { get; set; }
        /// <summary>
        /// The <see cref="ITrigger"/> that will be start the collect of the <see cref="ICollectPipeline"/> that owns it
        /// </summary>
        ITrigger Trigger { get; set; }
    }
}