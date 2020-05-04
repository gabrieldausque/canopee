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
    /// <summary>
    /// This is the base class for hardware collect informations. As the collect is specific for each supported OS, this class needs to be implemented for each OS.
    /// The current library contains two implementations :
    ///
    /// - windows <see cref="WindowsHardwareInfosInput"/>
    ///
    /// - linux <see cref="LinuxHardwareInfosInput"/>
    /// 
    /// This base class is responsible for the global sequence which is the same for each OS :
    ///
    /// - collect Cpu
    /// - collect memory
    /// - collect disks
    /// - collect display info (graphical cards and display)
    /// - collect usb peripherals
    ///
    /// Configuration for this input will be :
    ///
    /// <example>
    /// <code>
    ///     {
    ///         ...
    ///         "Canopee": {
    ///             ...
    ///                 "Pipelines": [
    ///                  ...   
    ///                   {
    ///                     "Name": "Products",
    ///                     ...
    ///                     "Input": {
    ///                        "InputType": "Hardware",
    ///                        "OSSpecific": true
    ///                     },
    ///                  ...
    ///                 }
    ///                 ...   
    ///                 ]
    ///             ...
    ///         }
    ///     }
    /// </code>
    /// </example>
    ///
    /// the InputType is Hardware
    /// The OSSpecific argument must be set to true.
    /// 
    /// </summary>
    public abstract class BaseHardwareInfosInput : BatchInput
    {
        /// <summary>
        /// Default constructor. Initialize the units mapping repository.
        /// </summary>
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

        /// <summary>
        /// Collect one <see cref="HardwareInfos"/> to be treated in the executing pipeline 
        /// </summary>
        /// <param name="fromTriggerEventArgs"><see cref="TriggerEventArgs"/> sent by the trigger that has raised the executing pipeline</param>
        /// <returns></returns>
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

        /// <summary>
        /// Get a human readable bytes value in the optimized unit.
        /// </summary>
        /// <param name="originalSize">a size in bytes</param>
        /// <param name="unit">the optimized unit </param>
        /// <returns>the value in the unit out value</returns>
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

        /// <summary>
        /// Map a customUnit to a standard unit. Used to standardize unit used in some batch. 
        /// </summary>
        /// <param name="customUnit">the custom unit to standardize</param>
        /// <returns>the standardize unit</returns>
        protected string GetSizeUnit(string customUnit)
        {
            if (UnitsRepository.ContainsKey(customUnit))
                return UnitsRepository[customUnit];
            return customUnit;
        }
        
        /// <summary>
        /// Set cpu infos in the <see cref="HardwareInfos.CpuArchitecture"/>,<see cref="HardwareInfos.CpuModel"/> and <see cref="HardwareInfos.CpusAvailable"/> in the <see cref="HardwareInfos"/> arg 
        /// </summary>
        /// <param name="infos">the <see cref="HardwareInfos"/> to enrich</param>
        protected abstract void SetCpuInfos(HardwareInfos infos);
        
        /// <summary>
        /// Set memory infos in the <see cref="HardwareInfos.MemorySize"/> and <see cref="HardwareInfos.MemoryUnit"/>
        /// </summary>
        /// <param name="infos">the <see cref="HardwareInfos"/> to enrich</param>
        protected abstract void SetMemoryInfos(HardwareInfos infos);
        
        /// <summary>
        /// Set one or more <see cref="DiskInfos"/> in the <see cref="HardwareInfos.Disks"/> in the <see cref="HardwareInfos"/> arg
        /// </summary>
        /// <param name="infos">the <see cref="HardwareInfos"/> to enrich</param>
        protected abstract void SetDiskInfos(HardwareInfos infos);
        
        /// <summary>
        /// Set one or more <see cref="DisplayInfos"/> in the <see cref="HardwareInfos.Displays"/> and one or more <see cref="GraphicalCardInfos"/> in the <see cref="HardwareInfos.GraphicalCards"/> in the <see cref="HardwareInfos"/> arg
        /// </summary>
        /// <param name="infos">the <see cref="HardwareInfos"/> to enrich</param>
        protected abstract void SetDisplayInfos(HardwareInfos infos);
        
        /// <summary>
        /// Set one or more <see cref="UsbPeripheralInfos"/> in the <see cref="HardwareInfos.USBPeripherals"/> in the <see cref="HardwareInfos"/> arg
        /// </summary>
        /// <param name="infos">the <see cref="HardwareInfos"/> to enrich</param>
        protected abstract void SetUsbPeripherals(HardwareInfos infos);
    }
}