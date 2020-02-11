using System;
using System.Collections.Generic;
using System.Composition;
using System.Diagnostics;
using CanopeeAgent.Common;
using CanopeeAgent.Core.Configuration;
using CanopeeAgent.Core.Indicators;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace CanopeeAgent.StandardIndicators.Indicators
{
    [Export("Hardware",typeof(IIndicator))]
    public class HardwareIndicator : IIndicator
    {
        public void Initialize(IndicatorConfiguration configuration)
        {
            //Create the trigger
            Trigger = TriggerFactory.Instance.GetTrigger(configuration.Input["TriggerType"],
                configuration.Input);
            Trigger.EventTriggered += (sender, args) => { this.Collect(); };
            
            //TODO : create the transform object
            
            //TODO : create the output object
        }

        public void Collect()
        {
            //TODO : prepare the collect based on the OS
            //get cpu info
            
            //get memory info
            
            //get disk infos
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ||
                RuntimeInformation.IsOSPlatform(OSPlatform.FreeBSD))
            {
                //cpu info ....
                var results = new Dictionary<string, string>();
                var psi = new ProcessStartInfo
                {
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    FileName = "/bin/bash",
                    Arguments = "-c \"lscpu \""
                };
                var p = Process.Start(psi);
                var processOutput = p.StandardOutput.ReadToEnd();
                var lines = processOutput.Split("\n");
                var regex = new Regex(".+:[ ]+(?<CpuModel>.+)");
                var matches = regex.Match(lines[13]);
                results.Add("CpuModel", matches.Groups["CpuModel"].Value);
                
                psi = new ProcessStartInfo
                {
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    FileName = "/bin/bash",
                    Arguments = "-c \"lsblk -P -o KNAME,SIZE,MODEL \""
                };
                p = Process.Start(psi);
                Console.WriteLine($"{JsonConvert.SerializeObject(results)}");    
            }
            
        }

        public void Run()
        {
            Trigger.Start();
        }

        public void Stop()
        {
            Trigger.Stop();
        }

        public ITransform Transform { get; set; }
        public IOutput Output { get; set; }
        public ITrigger Trigger { get; set; }

        public void Dispose()
        {
        
        }
    }
}