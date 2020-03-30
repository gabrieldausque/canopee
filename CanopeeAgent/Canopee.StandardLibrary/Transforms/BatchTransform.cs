using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Canopee.Common;
using Canopee.Core.Pipelines;
using Microsoft.Extensions.Configuration;

namespace Canopee.StandardLibrary.Transforms
{
    public abstract class BatchTransform : BaseTransform
    {
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

        public string Arguments { get; set; }

        public string ShellExecutor { get; set; }

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
   
        public override void Initialize(IConfigurationSection transformConfiguration)
        {
            SetExecutorByOs();
        }

    }
}