using System.Composition;

namespace CanopeeAgent.StandardIndicators.Indicators.Hardware
{
    [Export("WINDOWS", typeof(IHardwareInfosEventCollector))]
    public class WindowsHardwareInfosCollector : BaseHardwareInfosCollector
    {
        protected override void SetCpuInfos(HardwareInfosEvent infosEvent)
        {
            throw new System.NotImplementedException();
        }

        protected override void SetMemoryInfos(HardwareInfosEvent infosEvent)
        {
            throw new System.NotImplementedException();
        }

        protected override void SetDiskInfos(HardwareInfosEvent infosEvent)
        {
            throw new System.NotImplementedException();
        }

        protected override void SetDisplayInfos(HardwareInfosEvent infosEvent)
        {
            throw new System.NotImplementedException();
        }
    }
}