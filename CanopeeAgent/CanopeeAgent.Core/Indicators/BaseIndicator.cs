using System;
using System.Runtime.InteropServices;
using CanopeeAgent.Common;
using CanopeeAgent.Core.Configuration;
using CanopeeAgent.StandardIndicators.Indicators.Hardware;

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

        public virtual void Initialize(IndicatorConfiguration configuration)
        {
            Trigger = TriggerFactory.Instance.GetTrigger(configuration.Input["TriggerType"],
                configuration.Input);
            Trigger.EventTriggered += (sender, args) => { this.Collect(); };
            
            Transform = TransformFactory.Instance.GetTransform(configuration.Transform["TransformType"], 
                configuration.Transform);
            
            Output = OutputFactory.Instance.GetOutput(configuration.Output["OutputType"], configuration.Output);
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