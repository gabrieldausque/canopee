using System.ComponentModel.DataAnnotations;
using System.Composition;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;

namespace CanopeeAgent.StandardIndicators.Indicators.Hardware
{
    [Export("LINUX", typeof(IHardwareInfosEventCollector))]
    [Export("FREEBSD", typeof(IHardwareInfosEventCollector))]
    public class LinuxHardwareInfosCollector : BaseHardwareInfosCollector
    {
        private string[] GetBatchOutput(string commandLine)
        {
            var psi = new ProcessStartInfo
            {
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                FileName = "/bin/bash",
                Arguments = $"-c \"{commandLine} \""
            };
            var p = Process.Start(psi);
            var processOutput = p.StandardOutput.ReadToEnd();
            return processOutput.Split("\n");
        }
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

        protected override void SetMemoryInfos(HardwareInfos infos)
        {
            var lines = GetBatchOutput("free -h");
            var regex = new Regex(".+:[ ]+(?<size>[0-9\\.,]+)(?<unit>[a-zA-Z]+)[ ]+.*");
            var matches = regex.Match(lines[1]);
            infos.MemorySize = int.Parse(matches.Groups["size"].Value);
            infos.MemoryUnit = GetSizeUnit(matches.Groups["unit"].Value);
        }

        protected override void SetDiskInfos(HardwareInfos infos)
        {
            var lines = GetBatchOutput("df -h --output=source,size,avail ");
            var regex = new Regex(@"/dev/(?<volumeName>sd[a-z]+[0-9]+)[ ]+(?<size>[0-9\.,]+)(?<sizeUnit>[a-zA-Z]+)[ ]+(?<spaceAvailable>[0-9\.,]+)(?<spaceAvailableUnit>[A-Z]+)");
            foreach (var line in lines)
            {
                var matches = regex.Match(line);
                var volumeName = matches.Groups["volumeName"].Value;
                int.TryParse(matches.Groups["size"].Value, out var size);
                int.TryParse(matches.Groups["spaceAvailable"].Value, out var spaceAvailable);
                var spaceAvailableUnit = matches.Groups["spaceAvailableUnit"].Value;
                var sizeUnit = matches.Groups["sizeUnit"].Value;
                if (string.IsNullOrEmpty(volumeName))
                    continue;
                var diskInfo = new DiskInfos(this.AgentId)
                {
                    EventDate = infos.EventDate,
                    EventId = infos.EventId,
                    Name = volumeName,
                    Size = size,
                    SizeUnit = GetSizeUnit(sizeUnit),
                    SpaceAvailable = spaceAvailable,
                    SpaceAvailableUnit = spaceAvailableUnit
                };
                infos.AddDiskInfos(diskInfo);
            }
        }

        protected override void SetDisplayInfos(HardwareInfos infos)
        {
            var lines = GetBatchOutput("xrandr");
            foreach (var line in lines)
            {
                if (!line.Contains("Screen")) continue;
                var regex = new Regex(@"Screen (?<screenId>[0-9]+): minimum [0-9]+ x [0-9]+, current (?<resolution>[0-9]+ x [0-9]+), maximum [0-9]+ x [0-9]+");
                var macthes = regex.Match(line);
                var displayInfos = new DisplayInfos(AgentId)
                {
                    EventDate = infos.EventDate,
                    EventId = infos.EventId,
                    Resolution = macthes.Groups["resolution"].Value
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
    }
}