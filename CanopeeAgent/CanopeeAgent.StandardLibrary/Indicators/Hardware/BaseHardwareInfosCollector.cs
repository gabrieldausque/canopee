using CanopeeAgent.Core.Configuration;

namespace CanopeeAgent.StandardIndicators.Indicators.Hardware
{
    public abstract class BaseHardwareInfosCollector : IHardwareInfosEventCollector
    {
        public virtual HardwareInfosEvent Collect()
        {
            var infos = new HardwareInfosEvent(ConfigurationService.Instance.AgentId);
            SetCpuInfos(infos);
            SetMemoryInfos(infos);
            SetDiskInfos(infos);
            SetDisplayInfos(infos);
            return infos;
        }

        protected abstract void SetCpuInfos(HardwareInfosEvent infosEvent);
        protected abstract void SetMemoryInfos(HardwareInfosEvent infosEvent);
        protected abstract void SetDiskInfos(HardwareInfosEvent infosEvent);
        protected abstract void SetDisplayInfos(HardwareInfosEvent infosEvent);
        
    }
}