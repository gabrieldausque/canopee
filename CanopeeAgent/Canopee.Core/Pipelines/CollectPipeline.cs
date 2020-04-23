using System;
using System.Collections.Generic;
using System.Composition;
using System.Security.Cryptography.X509Certificates;
using Canopee.Common;
using Canopee.Common.Logging;
using Canopee.Common.Pipelines;
using Canopee.Common.Pipelines.Events;
using Canopee.Core.Configuration;
using Canopee.Core.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Canopee.Core.Pipelines
{
    /// <summary>
    /// The pipeline that will collect, transforms and send to an output <see cref="ICollectedEvent"/>
    /// </summary>
    [Export("Default", typeof(ICollectPipeline))]
    public class CollectPipeline : ICollectPipeline
    {
        /// <summary>
        /// The internal <see cref="ICanopeeLogger"/>
        /// </summary>
        protected readonly ICanopeeLogger Logger = null;
        
        /// <summary>
        /// The agent id (uuidv4 format)
        /// </summary>
        protected string AgentId;
        
        /// <summary>
        /// The lock object used to avoid that two collect of the same pipeline run simultaneously
        /// </summary>
        protected readonly object LockCollect = new object();
        
        /// <summary>
        /// The is collecting flag
        /// </summary>
        protected bool IsCollecting;
        
        /// <summary>
        /// The name of the current <see cref="ICollectPipeline"/>
        /// </summary>
        public string Name { get; private set; }
        
        /// <summary>
        /// The Id of the current <see cref="ICollectPipeline"/>. We recommend uuidv4
        /// </summary>
        public string Id { get; private set; }

        /// <summary>
        /// Default constructor. Initialize the logger and the guid
        /// </summary>
        public CollectPipeline()
        {
            Logger = CanopeeLoggerFactory.Instance()
                .GetLogger(ConfigurationService.Instance.GetLoggingConfiguration(), this.GetType());
            Transforms = new List<ITransform>();
            Id = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Initialize the current pipeline with a pipelineConfigurationSection. Set the id if specified, create trigger, input, all transformations and output.
        /// </summary>
        /// <param name="pipelineConfigurationSection">a pipeline pipelineConfigurationSection section</param>
        public virtual void Initialize(IConfigurationSection pipelineConfigurationSection)
        {
            
            AgentId = ConfigurationService.Instance.AgentId;
            Name = pipelineConfigurationSection["Name"];
            
            if (!string.IsNullOrWhiteSpace(pipelineConfigurationSection["Id"]))
            {
                Id = pipelineConfigurationSection["Id"];
            }
            
            var triggerConfiguration = pipelineConfigurationSection.GetSection("Trigger");
            Trigger = TriggerFactory.Instance().GetTrigger(triggerConfiguration);
            Trigger.SubscribeToTrigger((sender, args) => { this.Collect(args); }, new TriggerSubscriptionContext()
            {
                PipelineId =  Id,
                PipelineName = Name
            });

            var inputConfiguration = pipelineConfigurationSection.GetSection("Input");
            Input = InputFactory.Instance().GetInput(inputConfiguration, AgentId);
            
            var transformsConfiguration = pipelineConfigurationSection.GetSection("Transforms");
            foreach(var transformConfiguration in transformsConfiguration.GetChildren())
            {
                var transform = TransformFactory.Instance().GetTransform(transformConfiguration);
                Transforms.Add(transform);
            }

            var outputConfiguration = pipelineConfigurationSection.GetSection("Output");
            Output = OutputFactory.Instance().GetOutput(outputConfiguration);
        }

        /// <summary>
        /// Collect, transforms and output one or more <see cref="ICollectedEvent"/>
        /// </summary>
        /// <param name="fromTriggerEventArgs">a event that the <see cref="ITrigger"/> that has started this collect can pass to give informations</param>
        public virtual void Collect(TriggerEventArgs fromTriggerEventArgs)
        {
            Logger.LogInfo($"Collecting pipeline {this}");
            try
            {
                if (!IsCollecting)
                {
                    lock (LockCollect)
                    {
                        if (IsCollecting)
                            return;
                        IsCollecting = true;
                    }
                    var collectedEvents = Input.Collect(fromTriggerEventArgs);
                    foreach (var collectedEvent in collectedEvents)
                    {
                        foreach(var transformer in Transforms)
                        {
                            transformer.Transform(collectedEvent);
                        }
                        Output.SendToOutput(collectedEvent);
                    }
                }
            }
            catch(Exception ex)
            {
                Logger.LogError($"Error while collecting {this} : {ex}");
            }
            finally
            {
                Logger.LogInfo($"Collect finished for {this}");
                IsCollecting = false;
            }
        }

        /// <summary>
        /// Start the <see cref="ITrigger"/> watch
        /// </summary>
        public virtual void Run()
        {
            Trigger.Start();
        }

        /// <summary>
        /// Stop the <see cref="ITrigger"/> watch
        /// </summary>
        public virtual void Stop()
        {
            Trigger.Stop();
        }


        /// <summary>
        /// The internal disposed flag
        /// </summary>
        private bool _disposed = false;
        
        /// <summary>
        /// Dispose the current object
        /// </summary>
        public virtual void Dispose()
        {
            Dispose(true); 
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose all needed internal object
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                Trigger?.Dispose();
                //TODO : dispose other stuff
                _disposed = true;
            }
        }

        /// <summary>
        /// The input that will collect one or more <see cref="ICollectedEvent"/>
        /// </summary>
        public IInput Input { get; set; }
        
        /// <summary>
        /// All <see cref="ITransform"/> that will transform <see cref="ICollectedEvent"/> collected by the <see cref="CollectPipeline.Input"/> 
        /// </summary>
        public ICollection<ITransform> Transforms { get; set; }
        
        /// <summary>
        /// The <see cref="IOutput"/> that will send the output to external output (service, database, file, etc ...) 
        /// </summary>
        public IOutput Output { get; set; }
        
        /// <summary>
        /// The <see cref="ITrigger"/> that start the <see cref="CollectPipeline.Collect"/> when needed
        /// </summary>
        public ITrigger Trigger { get; set; }

        /// <summary>
        /// Display the current pipeline in the format : Name:Id@AgentId
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{Name}:{Id}@{AgentId}";
        }
    }
}