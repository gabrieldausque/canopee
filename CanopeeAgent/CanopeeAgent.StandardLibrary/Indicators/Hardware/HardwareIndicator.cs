using System.Collections.Generic;
using System.Composition;
using System.Net.NetworkInformation;
using CanopeeAgent.Common;
using CanopeeAgent.Core.Indicators;
using CanopeeAgent.StandardIndicators.Factories;
using Microsoft.VisualBasic.CompilerServices;
using Nest;
using Newtonsoft.Json;

namespace CanopeeAgent.StandardIndicators.Indicators.Hardware
{
    [Export("Hardware",typeof(IIndicator))]
    public class HardwareIndicator : BaseIndicator
    {
        private readonly IHardwareInfosEventCollector _currentCollector;
        public HardwareIndicator()
        {
            _currentCollector =
                HardwareInfosEventCollectorFactory.Instance.GetCollectorByPlatform(GetCurrentPlatform());
        }

        public override ICollection<ICollectedEvent> InternalCollect()
        {
            var collectedEvents = new List<ICollectedEvent>();
            HardwareInfos infos = _currentCollector.Collect();
            collectedEvents.Add(infos);
            collectedEvents.AddRange(infos.Disks);
            collectedEvents.AddRange(infos.GraphicalCards);
            collectedEvents.AddRange(infos.Displays);
            return collectedEvents;
        }
    }
}