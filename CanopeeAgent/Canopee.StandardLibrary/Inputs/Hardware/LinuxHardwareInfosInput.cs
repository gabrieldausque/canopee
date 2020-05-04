using Canopee.Common;
using System.Composition;
using System.Text.RegularExpressions;
using Canopee.Common.Pipelines;
using Microsoft.AspNetCore.Mvc;

namespace Canopee.StandardLibrary.Inputs.Hardware
{
    /// <summary>
    /// This class is the <see cref="BaseHardwareInfosInput"/> for all LINUX OS
    /// </summary>
    [Export("HardwareLINUX", typeof(IInput))]
    public class LinuxHardwareInfosInput : BaseHardwareInfosInput
    {
        /// <summary>
        /// Default constructor. Set the shell executor to bash
        /// </summary>
        public LinuxHardwareInfosInput()
        {
            ShellExecutor = "/bin/bash";
            Arguments = "-c";
        }

        /// <summary>
        /// Set the cpu infos. Use lscpu command.
        /// </summary>
        /// <param name="infos"></param>
        protected override void SetCpuInfos(HardwareInfos infos)
        {
            var lines = GetBatchOutput("lscpu");
            var regex = new Regex(".+:[ ]+(.+)");
            var matches = regex.Match(lines[0]);
            infos.CpuArchitecture = matches.Groups[1].Value;
            matches = regex.Match(lines[4]);
            infos.CpusAvailable = int.Parse(matches.Groups[1].Value);
            matches = regex.Match(lines[13]);
            infos.CpuModel = matches.Groups[1].Value;
        }

        /// <summary>
        /// Set the memory infos. Use free command.
        /// </summary>
        /// <param name="infos"></param>
        protected override void SetMemoryInfos(HardwareInfos infos)
        {
            var lines = GetBatchOutput("\"free -h\"");
            var regex = new Regex(".+:[ ]+(?<size>[0-9\\.,]+)(?<unit>[a-zA-Z]+)[ ]+.*");
            var matches = regex.Match(lines[1]);
            infos.MemorySize = float.Parse(matches.Groups["size"].Value);
            infos.MemoryUnit = GetSizeUnit(matches.Groups["unit"].Value);
        }

        /// <summary>
        /// Set all <see cref="DiskInfos"/>. Use df command.
        /// </summary>
        /// <param name="infos"></param>
        protected override void SetDiskInfos(HardwareInfos infos)
        {
            var lines = GetBatchOutput("\"df --output=source,size,avail \"");
            var regex = new Regex(@"/dev/(?<volumeName>sd[a-z]+[0-9]+)[ ]+(?<size>[0-9\.,]+)[ ]+(?<spaceAvailable>[0-9\.,]+)");
            foreach (var line in lines)
            {
                var matches = regex.Match(line);
                var volumeName = matches.Groups["volumeName"].Value;
                float.TryParse(matches.Groups["size"].Value, out var sizeInByte);
                float.TryParse(matches.Groups["spaceAvailable"].Value, out var spaceAvailableInByte);
                var size = GetOptimizedSizeAndUnit(sizeInByte, out var sizeUnit);
                var spaceAvailable = GetOptimizedSizeAndUnit(spaceAvailableInByte, out var spaceAvailableUnit);
                if (string.IsNullOrWhiteSpace(volumeName))
                    continue;
                var diskInfo = new DiskInfos(this.AgentId)
                {
                    EventDate = infos.EventDate,
                    EventId = infos.EventId,
                    Name = volumeName,
                    SizeInByte = sizeInByte,
                    Size = size,
                    SizeUnit = sizeUnit,
                    SpaceAvailableInByte = spaceAvailableInByte,
                    SpaceAvailable = spaceAvailable,
                    SpaceAvailableUnit = spaceAvailableUnit
                };
                infos.AddDiskInfos(diskInfo);
            }
        }

        /// <summary>
        /// Set all <see cref="DisplayInfos"/>. Use xrandr command.
        /// </summary>
        /// <param name="infos"></param>
        protected override void SetDisplayInfos(HardwareInfos infos)
        {
            var lines = GetBatchOutput("xrandr");
            foreach (var line in lines)
            {
                if (!line.Contains("Screen")) continue;
                var regex = new Regex(@"Screen (?<screenId>[0-9]+): minimum [0-9]+ x [0-9]+, current (?<resolution>[0-9]+ x [0-9]+), maximum [0-9]+ x [0-9]+");
                var matches = regex.Match(line);
                var displayInfos = new DisplayInfos(AgentId)
                {
                    EventDate = infos.EventDate,
                    EventId = infos.EventId,
                    Resolution = matches.Groups["resolution"].Value,
                    Name = $"Screen {matches.Groups["screenId"].Value}" 
                };
                infos.AddDisplayInfos(displayInfos);
            }

            lines = GetBatchOutput("lspci");
            foreach (var line in lines)
            {
                if (!line.Contains("VGA") && !line.Contains("3D")) continue;
                var displayInfos = new GraphicalCardInfos(AgentId)
                {
                    EventDate = infos.EventDate,
                    EventId = infos.EventId
                };
                var fields = line.Split(":");
                var regex = new Regex(@"[0-9]+[\.,]+[0-9]+[ ]+(.*)");
                displayInfos.GraphicalCardType = regex.Match(fields[1]).Groups[1].Value;
                displayInfos.GraphicalCardModel = fields[2].Trim();
                infos.AddGraphicalCardInfos(displayInfos);
            }
        }

        /// <summary>
        /// Set all <see cref="UsbPeripheralInfos"/>. use lsusb command.
        /// </summary>
        /// <param name="infos"></param>
        protected override void SetUsbPeripherals(HardwareInfos infos)
        {
            foreach (var line in GetBatchOutput("lsusb"))
            {
                var usbDeviceRegexp = new Regex("Bus (?<busId>[0-9]{3}) Device (?<deviceNumber>[0-9]{3}): ID (?<deviceId>[0-9a-zA-Z:]{9}) (?<deviceName>.+)");
                if (usbDeviceRegexp.IsMatch(line))
                {
                    var match = usbDeviceRegexp.Match(line);
                    var usbInfo = new UsbPeripheralInfos(AgentId)
                    {
                        BusId = match.Groups["busId"].Value,
                        DeviceNumber = match.Groups["deviceNumber"].Value,
                        DeviceId = match.Groups["deviceId"].Value,
                        DeviceName = match.Groups["deviceName"].Value
                    };
                    infos.AddUsbPeripherals(usbInfo);
                }
            }
        }
    }
}