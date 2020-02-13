using System.Composition;
using System.Net.NetworkInformation;
using CanopeeAgent.Common;
using CanopeeAgent.Core.Indicators;
using CanopeeAgent.StandardIndicators.Factories;
using Microsoft.VisualBasic.CompilerServices;
using Newtonsoft.Json;

namespace CanopeeAgent.StandardIndicators.Indicators.Hardware
{
    [Export("Hardware",typeof(IIndicator))]
    public class HardwareIndicator : BaseIndicator
    {
        private IHardwareInfosEventCollector _currentCollector;
        public HardwareIndicator()
        {
            _currentCollector =
                HardwareInfosEventCollectorFactory.Instance.GetCollectorByPlatform(GetCurrentPlatform());
        }
        
        public override void Collect()
        {
            //get the event
            ICollectedEvent infos = _currentCollector.Collect();
            
            //enrich event
            infos = Transform.Transform(infos);
            
            //send event to output
            Output.SendToOutput(infos);    
        }
    }
}