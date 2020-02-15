using System.Composition;

namespace CanopeeAgent.StandardIndicators.Indicators.Hardware
{
    [Export("WINDOWS", typeof(IHardwareInfosEventCollector))]
    public class WindowsHardwareInfosCollector : BaseHardwareInfosCollector
    {
        protected override void SetCpuInfos(HardwareInfos infos)
        {
            throw new System.NotImplementedException();
        }

        protected override void SetMemoryInfos(HardwareInfos infos)
        {
            throw new System.NotImplementedException();
        }

        protected override void SetDiskInfos(HardwareInfos infos)
        {
            throw new System.NotImplementedException();
        }

        protected override void SetDisplayInfos(HardwareInfos infos)
        {
            throw new System.NotImplementedException();
        }
    }
}