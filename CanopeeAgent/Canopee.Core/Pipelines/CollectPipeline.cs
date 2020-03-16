using System;
using System.Collections.Generic;
using System.Composition;
using System.Security.Cryptography.X509Certificates;
using Canopee.Common;
using Canopee.Common.Events;
using Canopee.Core.Configuration;
using Canopee.Core.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Canopee.Core.Pipelines
{
    [Export("Default", typeof(ICollectPipeline))]
    public class CollectPipeline : ICollectPipeline
    {
        private readonly ICanopeeLogger Logger = null;
        protected string _agentId;
        protected readonly object LockCollect = new object();
        protected bool _isCollecting;
        public string Name { get; private set; }
        public string Id { get; private set; }

        public CollectPipeline()
        {
            Logger = CanopeeLoggerFactory.Instance()
                .GetLogger(ConfigurationService.Instance.GetLoggingConfiguration(), this.GetType());
            Transforms = new List<ITransform>();
            Id = Guid.NewGuid().ToString();
        }

        public virtual void Initialize(IConfigurationSection configuration)
        {
            
            _agentId = ConfigurationService.Instance.AgentId;
            Name = configuration["Name"];
            
            if (!string.IsNullOrWhiteSpace(configuration["Id"]))
            {
                Id = configuration["Id"];
            }
            
            var triggerConfiguration = configuration.GetSection("Trigger");
            Trigger = TriggerFactory.Instance().GetTrigger(triggerConfiguration);
            Trigger.SubscribeToTrigger((sender, args) => { this.Collect(args); }, new TriggerSubscriptionContext()
            {
                PipelineId =  Id,
                PipelineName = Name
            });

            var inputConfiguration = configuration.GetSection("Input");
            Input = InputFactory.Instance().GetInput(inputConfiguration, _agentId);
            
            var transformsConfiguration = configuration.GetSection("Transforms");
            foreach(var transformConfiguration in transformsConfiguration.GetChildren())
            {
                var transform = TransformFactory.Instance().GetTransform(transformConfiguration);
                Transforms.Add(transform);
            }

            var outputConfiguration = configuration.GetSection("Output");
            Output = OutputFactory.Instance().GetOutput(outputConfiguration);
        }

        public virtual void Collect(TriggerEventArgs fromTriggerEventArgs)
        {
            Logger.LogInfo($"Collecting pipeline {this}");
            try
            {
                if (!_isCollecting)
                {
                    lock (LockCollect)
                    {
                        if (_isCollecting)
                            return;
                        _isCollecting = true;
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
                _isCollecting = false;
            }
        }

        public virtual void Run()
        {
            Trigger.Start();
        }

        public virtual void Stop()
        {
            Trigger.Stop();
        }


        private bool _disposed = false;
        public virtual void Dispose()
        {
            Dispose(true); 
            GC.SuppressFinalize(this);
        }

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

        public IInput Input { get; set; }
        public ICollection<ITransform> Transforms { get; set; }
        public IOutput Output { get; set; }
        public ITrigger Trigger { get; set; }

        public override string ToString()
        {
            return $"{Name}:{Id}@{_agentId}";
        }
    }
}