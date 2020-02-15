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
            HardwareInfos infos = _currentCollector.Collect();
            
            //send event to output
            Output.SendToOutput(infos);

            foreach (var diskInfos in infos.Disks)
            {
                Output.SendToOutput(diskInfos);
            }

            foreach (var displayInfos in infos.Displays)
            {
                Output.SendToOutput(displayInfos);
            }
        }
    }
}