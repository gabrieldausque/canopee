using System;
using System.Collections.Generic;
using System.Composition;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Canopee.Common;
using Canopee.Common.Pipelines;
using Canopee.Common.Pipelines.Events;
using Canopee.Core.Pipelines;
using Microsoft.Extensions.Configuration;

namespace Canopee.StandardLibrary.Inputs.Batch
{
    [Export("Batch",typeof(IInput))]
    public class BatchInput : BaseInput
    {
        protected Dictionary<string, string> UnitsRepository;
        protected string ShellExecutor;
        protected string Arguments;
        protected string CommandLine;

        public BatchInput()
        {
            UnitsRepository = new Dictionary<string, string>();
            SetExecutorByOs();
        }

        public override ICollection<ICollectedEvent> Collect(TriggerEventArgs fromTriggerEventArgs)
        {
            var collectedEvents = new List<ICollectedEvent>();
            foreach (var outputLine in GetBatchOutput(CommandLine))
            {
                collectedEvents.Add(new CollectedEvent(){Raw = outputLine});
            }
            return collectedEvents;
        }

        public override void Initialize(IConfiguration configurationInput, string agentId)
        {
            base.Initialize(configurationInput, agentId);
            if (!string.IsNullOrWhiteSpace(configurationInput["CommandLine"]))
            {
                CommandLine = configurationInput["CommandLine"];
            }
            if (!string.IsNullOrWhiteSpace(configurationInput["ShellExecutor"]))
            {
                ShellExecutor = configurationInput["ShellExecutor"];
            }
            if (!string.IsNullOrWhiteSpace(configurationInput["Arguments"]))
            {
                Arguments = configurationInput["Arguments"];
            }
            
        }

        protected OSPlatform GetCurrentPlatform()
        {
            if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return OSPlatform.Linux;
            }

            if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return OSPlatform.Windows;
            }

            if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.FreeBSD))
            {
                return OSPlatform.FreeBSD;
            }

            if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                return OSPlatform.OSX;
            }
            throw new NotSupportedException("The current OS is not Supported");
        }
        
        protected void SetExecutorByOs()
        {
            var OS = GetCurrentPlatform();
            Logger.LogDebug($"Set executor for OS {OS} ");
            if (OS == OSPlatform.Linux)
            {
                ShellExecutor = "/bin/bash";
                Arguments = "-c";
            } else if (OS == OSPlatform.Windows)
            {
                ShellExecutor = "cmd";
                Arguments = "/c";
            }
            else
            {
                throw new NotSupportedException($"OS {OS.ToString()} not supported yet. Please contact your administrator" );
            }
        }

        protected virtual string[] GetBatchOutput(string commandLine)
        {
            Logger.LogDebug($"Starting the command {commandLine}");
            Stopwatch watcher = Stopwatch.StartNew();
            var psi = new ProcessStartInfo
            {
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                FileName = ShellExecutor,
                Arguments = $"{Arguments} {commandLine} "
            };
            var p = Process.Start(psi);
            watcher.Stop();
            Logger.LogDebug($"The command {commandLine} took {watcher.Elapsed} of execution.");
            var processOutput = p.StandardOutput.ReadToEnd();
            return processOutput.Split("\n");
        }
    }
}