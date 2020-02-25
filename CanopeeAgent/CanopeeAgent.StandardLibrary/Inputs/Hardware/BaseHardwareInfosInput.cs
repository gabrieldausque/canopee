using System.Collections.Generic;
using System.Diagnostics;
using CanopeeAgent.Common;
using CanopeeAgent.Core.Configuration;
using CanopeeAgent.Core.Indicators;
using Microsoft.Extensions.Configuration;

namespace CanopeeAgent.StandardIndicators.Inputs.Hardware
{
    public abstract class BaseHardwareInfosInput : BaseInput
    {
        private Dictionary<string, string> _unitsRepository;
        
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

        protected BaseHardwareInfosInput()
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

        public override ICollection<ICollectedEvent> Collect()
        {
            var result = new List<ICollectedEvent>();
            var infos = new HardwareInfos(AgentId);
            SetCpuInfos(infos);
            SetMemoryInfos(infos);
            SetDiskInfos(infos);
            SetDisplayInfos(infos);
            result.Add(infos);
            result.AddRange(infos.Disks);
            result.AddRange(infos.Displays);
            result.AddRange(infos.GraphicalCards);
            return result;
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