using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Canopee.Common;
using Canopee.Common.Pipelines.Events;
using Canopee.Core.Pipelines;
using Canopee.StandardLibrary.Inputs.Batch;
using Microsoft.Extensions.Configuration;

namespace Canopee.StandardLibrary.Inputs.Hardware
{
    public abstract class BaseHardwareInfosInput : BatchInput
    {
        protected BaseHardwareInfosInput()
        {
            UnitsRepository = new Dictionary<string, string>()
            {
                {"T","Tb" },
                {"Ti","Tb" },
                {"G", "Gb"},
                {"Gi", "Gb"},
                {"M", "Mb"},
                {"Mi", "Mb"}
            };
        }

        public override ICollection<ICollectedEvent> Collect(TriggerEventArgs fromTriggerEventArgs)
        {
            Logger.LogDebug("Starting collecting hardware informations");
            try
            {
                Thread.Sleep(5000);
                var result = new List<ICollectedEvent>();
                var infos = new HardwareInfos(AgentId);
                SetCpuInfos(infos);
                SetMemoryInfos(infos);
                SetDiskInfos(infos);
                SetDisplayInfos(infos);
                SetUsbPeripherals(infos);
                result.Add(infos);
                result.AddRange(infos.Disks);
                result.AddRange(infos.Displays);
                result.AddRange(infos.GraphicalCards);
                result.AddRange(infos.USBPeripherals);
                return result;
            }
            catch (Exception ex)
            {
                Logger.LogError($"Error while collecting hardware information {ex}");
                throw;
            }
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

        protected string GetSizeUnit(string customUnit)
        {
            if (UnitsRepository.ContainsKey(customUnit))
                return UnitsRepository[customUnit];
            return customUnit;
        }
        
        protected abstract void SetCpuInfos(HardwareInfos infos);
        protected abstract void SetMemoryInfos(HardwareInfos infos);
        protected abstract void SetDiskInfos(HardwareInfos infos);
        protected abstract void SetDisplayInfos(HardwareInfos infos);
        protected abstract void SetUsbPeripherals(HardwareInfos infos);
    }
}