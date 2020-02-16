using System.Collections.Generic;
using CanopeeAgent.Core.Configuration;

namespace CanopeeAgent.StandardIndicators.Indicators.Hardware
{
    public abstract class BaseHardwareInfosCollector : IHardwareInfosEventCollector
    {
        private Dictionary<string, string> _unitsRepository;
        
        protected string AgentId;

        protected BaseHardwareInfosCollector()
        {
            _unitsRepository = new Dictionary<string, string>()
            {
                {"G", "Gb"},
                {"Gi", "Gb"},
                {"M", "Mb"}
            };
        }

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

        protected string GetSizeUnit(string customUnit)
        {
            if (_unitsRepository.ContainsKey(customUnit))
                return _unitsRepository[customUnit];
            return customUnit;
        }
        
        protected abstract void SetCpuInfos(HardwareInfos infos);
        protected abstract void SetMemoryInfos(HardwareInfos infos);
        protected abstract void SetDiskInfos(HardwareInfos infos);
        protected abstract void SetDisplayInfos(HardwareInfos infos);
    }
}