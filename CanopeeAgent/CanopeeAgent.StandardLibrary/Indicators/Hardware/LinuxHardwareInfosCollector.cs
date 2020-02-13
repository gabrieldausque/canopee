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
        protected override void SetCpuInfos(HardwareInfosEvent infosEvent)
        {
            var lines = GetBatchOutput("lscpu");
            var regex = new Regex(".+:[ ]+(.+)");
            var matches = regex.Match(lines[0]);
            infosEvent.CpuArchitecture = matches.Groups[1].Value;
            matches = regex.Match(lines[4]);
            infosEvent.CpusAvailable = int.Parse(matches.Groups[1].Value);
            matches = regex.Match(lines[13]);
            infosEvent.CpuModel = matches.Groups[1].Value;
        }

        protected override void SetMemoryInfos(HardwareInfosEvent infosEvent)
        {
            var lines = GetBatchOutput("free -h");
            var regex = new Regex(".+:[ ]+([0-9a-zA-Z]+)[ ]+.*");
            var matches = regex.Match(lines[1]);
            infosEvent.MemorySize = matches.Groups[1].Value;
        }

        protected override void SetDiskInfos(HardwareInfosEvent infosEvent)
        {
            var lines = GetBatchOutput("df -h --output=source,size,avail ");
            var regex = new Regex(@"/dev/(?<volumeName>sd[a-z]+[0-9]+)[ ]+(?<size>[0-9\.,A-Z]+)[ ]+(?<spaceAvailable>[0-9\.,A-Z]+)");
            foreach (var line in lines)
            {
                var matches = regex.Match(line);
                var volumeName = matches.Groups["volumeName"].Value;
                var size = matches.Groups["size"].Value;
                var spaceAvailable = matches.Groups["spaceAvailable"].Value;
                if (string.IsNullOrEmpty(volumeName))
                    continue;
                var diskInfo = new DiskInfos()
                {
                    Name = volumeName,
                    Size = size,
                    SpaceAvailable = spaceAvailable
                };
                infosEvent.AddDiskInfos(diskInfo);
            }
        }

        protected override void SetDisplayInfos(HardwareInfosEvent infosEvent)
        {
            var lines = GetBatchOutput("lspci");
            foreach (var line in lines)
            {
                if (line.Contains("VGA") || line.Contains("3D"))
                {
                    var displayInfos = new DisplayInfos();
                    var fields = line.Split(":");
                    var regex = new Regex(@"[0-9]+[\.,]+[0-9]+[ ]+(.*)");
                    displayInfos.DisplayType = regex.Match(fields[1]).Groups[1].Value;
                    displayInfos.DisplayModel = fields[2].Trim();
                    infosEvent.AddDisplayInfos(displayInfos);
                }
            }
        }
    }
}