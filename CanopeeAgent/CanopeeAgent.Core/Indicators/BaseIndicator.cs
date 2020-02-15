using System;
using System.Runtime.InteropServices;
using CanopeeAgent.Common;
using CanopeeAgent.Core.Configuration;
using CanopeeAgent.StandardIndicators.Indicators.Hardware;
using Microsoft.Extensions.Configuration;

namespace CanopeeAgent.Core.Indicators
{
    public abstract class BaseIndicator : IIndicator
    {
        protected OSPlatform GetCurrentPlatform()
        {
            if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return OSPlatform.Linux;
            }
            
            if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return OSPlatform.Windows;
            }
            
            if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.FreeBSD))
            {
                return OSPlatform.FreeBSD;
            }
            
            if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                return OSPlatform.OSX;
            }
            throw new NotSupportedException("The current OS is not Supported");
        }

        public virtual void Initialize(IConfigurationSection configuration)
        {
            var triggerConfiguration = configuration.GetSection("Trigger");
            Trigger = TriggerFactory.Instance.GetTrigger(triggerConfiguration["TriggerType"], triggerConfiguration);
            Trigger.EventTriggered += (sender, args) => { this.Collect(); };

            //The input configuration is not treated in the base consructor, as it may have specific configuration for each collector
            
            var transformConfiguration = configuration.GetSection("Transform");
            Transform = TransformFactory.Instance.GetTransform(transformConfiguration["TransformType"], 
                transformConfiguration);

            var outputConfiguration = configuration.GetSection("Output");
            Output = OutputFactory.Instance.GetOutput(outputConfiguration["OutputType"], 
                outputConfiguration);
        }

        public abstract void Collect();

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

        public ITransform Transform { get; set; }
        public IOutput Output { get; set; }
        public ITrigger Trigger { get; set; }
    }
}