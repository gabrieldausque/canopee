using CanopeeAgent.Core.Configuration;

namespace CanopeeAgent.StandardIndicators.Indicators.Hardware
{
    public abstract class BaseHardwareInfosCollector : IHardwareInfosEventCollector
    {
        protected string AgentId;

        public virtual HardwareInfos Collect()
        {
            AgentId = ConfigurationService.Instance.AgentId;
            var infos = new HardwareInfos(AgentId);
            SetCpuInfos(infos);
            SetMemoryInfos(infos);
            SetDiskInfos(infos);
            SetDisplayInfos(infos);
            return infos;
        }

        protected abstract void SetCpuInfos(HardwareInfos infos);
        protected abstract void SetMemoryInfos(HardwareInfos infos);
        protected abstract void SetDiskInfos(HardwareInfos infos);
        protected abstract void SetDisplayInfos(HardwareInfos infos);
        
    }
}