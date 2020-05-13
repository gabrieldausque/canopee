using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Canopee.Common;
using Canopee.Common.Pipelines;
using Canopee.Core.Pipelines;
using Microsoft.Extensions.Configuration;

namespace Canopee.StandardLibrary.Transforms
{
    /// <summary>
    /// Base class for <see cref="ITransform"/> that will add field from a batch output.
    /// Will be OSSpecific
    /// </summary>
    public abstract class BatchTransform : BaseTransform
    {
        /// <summary>
        /// Return the current OSPlatform 
        /// </summary>
        /// <returns>the OSPlatform</returns>
        /// <exception cref="NotSupportedException">If OSPlatform is not supported (not a Linux, Windows, FreeBSD, OSX)</exception>
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
        
        /// <summary>
        /// Set default executor and arguments depending on OS. Today only Linux (with bash) and windows (with cmd) are supported.
        /// </summary>
        /// <exception cref="NotSupportedException"></exception>
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

        /// <summary>
        /// Arguments used by the <see cref="ShellExecutor"/> to launch a command
        /// </summary>
        public string Arguments { get; set; }

        /// <summary>
        /// The shell executor
        /// </summary>
        public string ShellExecutor { get; set; }

        /// <summary>
        /// Launch the specified command line with the shell executor
        /// </summary>
        /// <param name="commandLine">The command line to execute</param>
        /// <returns>an array of string for each line of the output</returns>
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
   
        /// <summary>
        /// Initialize this <see cref="ITransform"/> with configurations. Add all defined <see cref="TransformFieldMapping"/> from the configuration.
        /// </summary>
        /// <param name="transformConfiguration">The transform configuration</param>
        /// <param name="loggingConfiguration">The logger configuration</param>
        public override void Initialize(IConfigurationSection transformConfiguration, IConfigurationSection loggingConfiguration)
        {
            base.Initialize(transformConfiguration, loggingConfiguration);
            SetExecutorByOs();
        }

    }
}