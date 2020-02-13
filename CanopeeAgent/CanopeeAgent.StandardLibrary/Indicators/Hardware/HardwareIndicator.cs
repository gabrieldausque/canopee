using System;
using System.Composition;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using CanopeeAgent.Common;
using CanopeeAgent.Core.Configuration;
using CanopeeAgent.Core.Indicators;
using CanopeeAgent.StandardIndicators.Factories;
using Microsoft.VisualBasic.CompilerServices;
using Newtonsoft.Json;

namespace CanopeeAgent.StandardIndicators.Indicators.Hardware
{
    [Export("Hardware",typeof(IIndicator))]
    public class HardwareIndicator : IIndicator
    {
        private IHardwareInfosEventCollector _currentCollector;
        public HardwareIndicator()
        {
            _currentCollector =
                HardwareInfosEventCollectorFactory.Instance.GetCollectorByPlatform(GetCurrentPlatform());
        }
        
        public void Initialize(IndicatorConfiguration configuration)
        {
            //Create the trigger
            
            Trigger = TriggerFactory.Instance.GetTrigger(configuration.Input["TriggerType"],
                configuration.Input);
            Trigger.EventTriggered += (sender, args) => { this.Collect(); };
            
            Transform = TransformFactory.Instance.GetTransform(configuration.Transform["TransformType"], 
                configuration.Transform);
            
            //TODO : create the output object
        }

        public void Collect()
        {
            //get the event
            ICollectedEvent infos = _currentCollector.Collect();
            
            //enrich event
            infos = Transform.Transform(infos);
            
            //send event to output
            Console.WriteLine($"{JsonConvert.SerializeObject(infos)}");    
        }

        private OSPlatform GetCurrentPlatform()
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

        public void Run()
        {
            Trigger.Start();
        }

        public void Stop()
        {
            Trigger.Stop();
        }

        public ITransform Transform { get; set; }
        public IOutput Output { get; set; }
        public ITrigger Trigger { get; set; }

        public void Dispose()
        {
        
        }
    }
}