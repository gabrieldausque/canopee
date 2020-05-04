using Canopee.Common;
using System;
using System.Collections.Generic;
using System.Composition;
using System.Text.RegularExpressions;
using System.Web;
using Canopee.Common.Pipelines;

namespace Canopee.StandardLibrary.Inputs.Hardware
{
    /// <summary>
    /// This class is the <see cref="BaseHardwareInfosInput"/> for all Windows OS
    /// </summary>
    [Export("HardwareWINDOWS", typeof(IInput))]
    public class WindowsHardwareInfosInput : BaseHardwareInfosInput
    {
        /// <summary>
        /// The graphical cards repository. 0 is unknown, 1 is other, 2 is for 2D, 3 is for 3D
        /// </summary>
        private static readonly Dictionary<int, string> GraphicalCardTypes = new Dictionary<int, string>()
        {
            {0,"unknown" },
            {1,"other" },
            {2,"2D" },
            {3,"3D" }
        };

        /// <summary>
        /// Default constructor. Set the shell executor to cmd.
        /// </summary>
        public WindowsHardwareInfosInput()
        {
            ShellExecutor = "cmd";
            Arguments = "/c";
        }

        /// <summary>
        /// Get cpus infos. use echo and wmic command.
        /// </summary>
        /// <param name="infos"></param>
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

        /// <summary>
        /// Set memory infos. use wmic command
        /// </summary>
        /// <param name="infos"></param>
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

        /// <summary>
        /// Get all <see cref="DiskInfos"/>. use wmic command
        /// </summary>
        /// <param name="infos"></param>
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
                    diskInfos.SizeInByte = capacity;
                    diskInfos.Size = GetOptimizedSizeAndUnit(capacity, out var unit);
                    diskInfos.SizeUnit = unit;
                    diskInfos.SpaceAvailableInByte = freeSpace;
                    diskInfos.SpaceAvailable = GetOptimizedSizeAndUnit(freeSpace, out unit);
                    diskInfos.SpaceAvailableUnit = unit;
                    infos.AddDiskInfos(diskInfos);
                }
            }
        }

        /// <summary>
        /// Get all <see cref="DisplayInfos"/>. Use wmic command.
        /// </summary>
        /// <param name="infos"></param>
        protected override void SetDisplayInfos(HardwareInfos infos)
        {
            //getting monitor infos
            var output = GetBatchOutput("wmic desktopmonitor get name,screenheight,screenwidth");
            for (int lineIndex = 1; lineIndex < output.Length; lineIndex++)
            {
                var regex = new Regex("(?<name>.+)[ ]+(?<height>[0-9]*)[ ]+(?<width>[0-9]*)");
                var match = regex.Match(output[lineIndex]);
                var name = string.IsNullOrWhiteSpace(match.Groups["name"].Value)?"Screen" : match.Groups["name"].Value.Trim();
                var height = string.IsNullOrWhiteSpace(match.Groups["height"].Value) ? "?" : match.Groups["height"].Value;
                var width = string.IsNullOrWhiteSpace(match.Groups["width"].Value) ? "?" : match.Groups["width"].Value;
                if(width == "?" || height == "?")
                {
                    var videomodeDescription = GetBatchOutput("wmic path win32_videocontroller get videomodedescription")[1];
                    var regexVideoMode = new Regex("(?<width>[0-9]+) x (?<height>[0-9]+) x.+");
                    var matchVideoMode = regexVideoMode.Match(videomodeDescription);
                    width = matchVideoMode.Groups["width"].Value;
                    height = matchVideoMode.Groups["height"].Value;
                }
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

        /// <summary>
        /// Get all <see cref="UsbPeripheralInfos"/>. Use wmic command.
        /// </summary>
        /// <param name="infos"></param>
        protected override void SetUsbPeripherals(HardwareInfos infos)
        {
            var usbDevicesIdLines = GetBatchOutput("\"wmic path Win32_UsbControllerDevice get Dependent\"");
            var PnpDeviceInfosLines = GetBatchOutput("\"wmic path Win32_PnPEntity get Name,PnpDeviceId /value\"");
            var usbDeviceIds = new List<string>();
            foreach (var line in usbDevicesIdLines)
            {
                var fields = line.Split('=');
                if(fields.Length > 1)
                {
                    usbDeviceIds.Add(line.Split('=')[1].Replace("\"", "").Trim());
                }
            }

            var deviceName = string.Empty;
            var deviceId = string.Empty;
            foreach(var line in PnpDeviceInfosLines)
            {
                if (line.Contains("Name"))
                {
                    deviceName = line.Split('=')[1].Trim();
                } else if(line.Contains("PNPDeviceID")) {
                    deviceId = HttpUtility.HtmlDecode(line.Split('=')[1].Trim());
                }

                if(!string.IsNullOrWhiteSpace(deviceName) && !string.IsNullOrWhiteSpace(deviceId))
                {
                    if (usbDeviceIds.Contains(deviceId))
                    {
                        var usbPeripheralInfo = new UsbPeripheralInfos(AgentId)
                        {
                            DeviceId = deviceId,
                            DeviceName = deviceName
                        };
                        infos.AddUsbPeripherals(usbPeripheralInfo);
                    }

                    deviceName = string.Empty;
                    deviceId = string.Empty;
                }
            }
        }

        /// <summary>
        /// Get the graphical card type label for the current graphical card type
        /// </summary>
        /// <param name="graphicalCardType">0,1,2 or 3</param>
        /// <returns></returns>
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