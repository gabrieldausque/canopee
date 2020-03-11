using Canopee.Common;
using System;
using System.Collections.Generic;
using System.Composition;
using System.Text.RegularExpressions;

namespace Canopee.StandardLibrary.Inputs.Hardware
{
    [Export("HardwareWINDOWS", typeof(IInput))]
    public class WindowsHardwareInfosInput : BaseHardwareInfosInput
    {
        private static readonly Dictionary<int, string> GraphicalCardTypes = new Dictionary<int, string>()
        {
            {0,"unknown" },
            {1,"other" },
            {2,"2D" },
            {3,"3D" }
        };

        public WindowsHardwareInfosInput()
        {
            ShellExecutor = "cmd";
            Arguments = "/c";
        }

        protected override void SetCpuInfos(HardwareInfos infos)
        {
            var output = GetBatchOutput("echo architecture=%PROCESSOR_ARCHITECTURE% model=%PROCESSOR_IDENTIFIER% ");
            var regex = new Regex("architecture=(?<architecture>.+)[ ]+model=(?<model>.+)");
            var match = regex.Match(output[0]);
            infos.CpuArchitecture = match.Groups["architecture"].Value;
            infos.CpuModel = match.Groups["model"].Value;
            
            output = GetBatchOutput("wmic cpu get numberoflogicalprocessors /value ");
            foreach(var  line in output)
            {
                if (line.Contains("NumberOfLogicalProcessors"))
                {
                    infos.CpusAvailable = int.Parse(line.Split("=")[1]);
                    break;
                }
            }
        }

        protected override void SetMemoryInfos(HardwareInfos infos)
        {
            var output = GetBatchOutput("wmic computersystem get TotalPhysicalMemory /value");
            foreach (var line in output)
            {
                if (line.Contains("TotalPhysicalMemory"))
                {
                    var currentMemorySize = GetOptimizedSizeAndUnit(long.Parse(line.Split("=")[1]), out var unit);
                    infos.MemorySize = currentMemorySize;
                    infos.MemoryUnit = unit;
                    break;
                }
            }
        }

        protected override void SetDiskInfos(HardwareInfos infos)
        {
            var output = GetBatchOutput("wmic volume get Name,Capacity,\"Free Space\",DriveType");
            for(int lineIndex = 1; lineIndex < output.Length; lineIndex++)
            {
                var regex = new Regex("(?<capacity>[0-9]+)[ ]+(?<driveType>[0-9]+)[ ]+(?<freeSpace>[0-9]+)[ ]+(?<name>[^ ]+)");
                var match = regex.Match(output[lineIndex]);
                var name = match.Groups["name"].Value.Trim();
                int.TryParse(match.Groups["driveType"].Value, out var driveType);
                float.TryParse(match.Groups["freeSpace"].Value, out var freeSpace);
                float.TryParse(match.Groups["capacity"].Value, out var capacity);
                
                if(driveType == 3)
                {
                    var diskInfos = new DiskInfos(this.AgentId)
                    {
                        EventId = infos.EventId,
                        EventDate = infos.EventDate,
                        Name = name
                    };
                    diskInfos.Size = GetOptimizedSizeAndUnit(capacity, out var unit);
                    diskInfos.SizeUnit = unit;
                    diskInfos.SpaceAvailable = GetOptimizedSizeAndUnit(freeSpace, out unit);
                    diskInfos.SpaceAvailableUnit = unit;
                    infos.AddDiskInfos(diskInfos);
                }
            }
        }

        protected override void SetDisplayInfos(HardwareInfos infos)
        {
            //getting monitor infos
            var output = GetBatchOutput("wmic desktopmonitor get name,screenheight,screenwidth");
            for (int lineIndex = 1; lineIndex < output.Length; lineIndex++)
            {
                var regex = new Regex("(?<name>.+)[ ]+(?<height>[0-9]*)[ ]+(?<width>[0-9]*)");
                var match = regex.Match(output[lineIndex]);
                var name = match.Groups["name"].Value.Trim();
                var height = string.IsNullOrWhiteSpace(match.Groups["height"].Value) ? "?" : match.Groups["height"].Value;
                var width = string.IsNullOrWhiteSpace(match.Groups["width"].Value) ? "?" : match.Groups["width"].Value;
                var displayInfos = new DisplayInfos(this.AgentId)
                {
                    EventId = infos.EventId,
                    EventDate = infos.EventDate,
                    Resolution = $"{width} x {height}",
                    Name = name
                };
                infos.AddDisplayInfos(displayInfos);
            }

            //getting video controller infos
            output = GetBatchOutput("wmic path Win32_VideoController get name,AcceleratorCapabilities");
            for (int lineIndex = 1; lineIndex < output.Length; lineIndex++)
            {
                var regex = new Regex("(?<graphicType>[0-9]*)[ ]+(?<name>.+)");
                var match = regex.Match(output[lineIndex]);
                int.TryParse(match.Groups["graphicType"].Value, out var graphicalType);
                var graphicInfos = new GraphicalCardInfos(this.AgentId)
                {
                    EventId = infos.EventId,
                    EventDate = infos.EventDate,
                    GraphicalCardModel = match.Groups["name"].Value.Trim(),
                    GraphicalCardType = GetGraphicalCardtype(graphicalType)
                };
                infos.AddGraphicalCardInfos(graphicInfos);
            }
        }

        protected override void SetUsbPeripherals(HardwareInfos infos)
        {
            
        }

        private string GetGraphicalCardtype(int graphicalCardType)
        {
            if (GraphicalCardTypes.ContainsKey(graphicalCardType))
            {
                return GraphicalCardTypes[graphicalCardType];
            }
            return GraphicalCardTypes[0];
        }
    }
}