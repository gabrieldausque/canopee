using System.Collections.Generic;
using System.Diagnostics;
using CanopeeAgent.Core.Configuration;

namespace CanopeeAgent.StandardIndicators.Indicators.Hardware
{
    public abstract class BaseHardwareInfosCollector : IHardwareInfosEventCollector
    {
        private Dictionary<string, string> _unitsRepository;
        
        protected string AgentId;

        protected string _shellExecutor;
        protected string _arguments;

        protected virtual string[] GetBatchOutput(string commandLine)
        {
            var psi = new ProcessStartInfo
            {
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                FileName = _shellExecutor,
                Arguments = $"{_arguments} {commandLine} "
            };
            var p = Process.Start(psi);
            var processOutput = p.StandardOutput.ReadToEnd();
            return processOutput.Split("\n");
        }

        protected float GetOptimizedSizeAndUnit(float originalSize, out string unit)
        {
            if (originalSize > 1000000000000f)
            {
                unit = "Tb";
                return originalSize / 1000000000000f;                
            }
            else if (originalSize > 1000000000f)
            {
                unit = "Gb";
                return originalSize / 1000000000f;
            }
            else if (originalSize > 1000000f)
            {
                unit = "Mb";
                return originalSize / 1000000f;
            }
            else if (originalSize > 1000f)
            {
                unit = "Kb";
                return originalSize / 1000f;
            }
            else
            {
                unit = "b";
                return originalSize;
            };
        }

        protected BaseHardwareInfosCollector()
        {
            _unitsRepository = new Dictionary<string, string>()
            {
                {"T","Tb" },
                {"Ti","Tb" },
                {"G", "Gb"},
                {"Gi", "Gb"},
                {"M", "Mb"},
                {"Mi", "Mb"}
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