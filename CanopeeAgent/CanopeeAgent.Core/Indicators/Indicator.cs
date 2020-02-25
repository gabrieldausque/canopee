using System;
using System.Collections.Generic;
using System.Composition;
using System.Data;
using System.Runtime.InteropServices;
using CanopeeAgent.Common;
using CanopeeAgent.Core.Configuration;
using CanopeeAgent.StandardIndicators.Indicators.Hardware;
using Microsoft.Extensions.Configuration;

namespace CanopeeAgent.Core.Indicators
{
    [Export("Default", typeof(IIndicator))]
    public class Indicator : IIndicator
    {
        protected string _agentId;

        public virtual void Initialize(IConfigurationSection configuration)
        {
            _agentId = ConfigurationService.Instance.AgentId;

            var triggerConfiguration = configuration.GetSection("Trigger");
            Trigger = TriggerFactory.Instance.GetTrigger(triggerConfiguration);
            Trigger.EventTriggered += (sender, args) => { this.Collect(); };

            var inputConfiguration = configuration.GetSection("Input");
            Input = InputFactory.Instance.GetInput(inputConfiguration, _agentId);
            
            var transformConfiguration = configuration.GetSection("Transform");
            Transform = TransformFactory.Instance.GetTransform(transformConfiguration);

            var outputConfiguration = configuration.GetSection("Output");
            Output = OutputFactory.Instance.GetOutput(outputConfiguration);
        }

        public virtual void Collect()
        {
            foreach (var collectedEvent in Input.Collect())
            {
                Transform.Transform(collectedEvent);
                Output.SendToOutput(collectedEvent);    
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

        public virtual void Dispose()
        {
            Trigger.Stop();
        }

        public IInput Input { get; set; }
        public ITransform Transform { get; set; }
        public IOutput Output { get; set; }
        public ITrigger Trigger { get; set; }
    }
}