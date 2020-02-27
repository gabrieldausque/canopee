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
        protected object _lockCollect = new object();
        protected bool _isCollecting;

        public Indicator()
        {
            Transforms = new List<ITransform>();
        }

        public virtual void Initialize(IConfigurationSection configuration)
        {
            _agentId = ConfigurationService.Instance.AgentId;

            var triggerConfiguration = configuration.GetSection("Trigger");
            Trigger = TriggerFactory.Instance.GetTrigger(triggerConfiguration);
            Trigger.EventTriggered += (sender, args) => { this.Collect(); };

            var inputConfiguration = configuration.GetSection("Input");
            Input = InputFactory.Instance.GetInput(inputConfiguration, _agentId);
            
            var transformsConfiguration = configuration.GetSection("Transforms");
            foreach(var transformConfiguration in transformsConfiguration.GetChildren())
            {
                var transform = TransformFactory.Instance.GetTransform(transformConfiguration);
                Transforms.Add(transform);
            }

            var outputConfiguration = configuration.GetSection("Output");
            Output = OutputFactory.Instance.GetOutput(outputConfiguration);
        }

        public virtual void Collect()
        {
            try
            {
                if (!_isCollecting)
                {
                    lock (_lockCollect)
                    {
                        if (_isCollecting)
                            return;
                        _isCollecting = true;
                    }
                    var collectedEvents = Input.Collect();
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
                var color = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.ToString());
                Console.ForegroundColor = color;
            }
            finally
            {
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

        public virtual void Dispose()
        {
            Trigger.Stop();
        }

        public IInput Input { get; set; }
        public ICollection<ITransform> Transforms { get; set; }
        public IOutput Output { get; set; }
        public ITrigger Trigger { get; set; }
    }
}