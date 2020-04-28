using System;
using System.Collections.Generic;
using System.Composition;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Canopee.Common.Pipelines;
using Canopee.Common.Pipelines.Events;
using Canopee.Core.Pipelines;
using Microsoft.Extensions.Configuration;

namespace Canopee.StandardLibrary.Inputs.Batch
{
    /// <summary>
    /// This <see cref="IInput"/> collect each line of the output of a batch command.
    /// By default it manage :
    /// - dosshell command for windows
    /// - bash command for linux
    /// You can override the shell executor by configuration :
    /// <example>
    /// For a powershell command you will need this configuration for input in a pipeline :
    /// <code>
    /// {
    ///     ...
    ///     "Input": {
    ///         "InputType": "Batch",
    ///         "CommandLine": "Get-ItemProperty -ErrorAction SilentlyContinue -Path HKLM:\SYSTEM\CurrentControlSet\Enum\USBSTOR\*\* | Select-Object FriendlyName | format-table -hidetableheaders"
    ///         "ShellExecutor": "powershell.exe"
    ///         "Arguments": "-Command"
    ///     },
    /// }
    /// </code>
    /// </example>
    /// </summary>
    [Export("Batch",typeof(IInput))]
    public class BatchInput : BaseInput
    {
        /// <summary>
        /// Internal repository used for unit conversion (bytes to Kb, etc ...)
        /// </summary>
        protected Dictionary<string, string> UnitsRepository;
        
        /// <summary>
        /// The shell executable to use 
        /// </summary>
        protected string ShellExecutor;
        
        /// <summary>
        /// The arguments used with the shell executable, excluding the command line to be executed
        /// </summary>
        protected string Arguments;
        
        /// <summary>
        /// The command line to be executed in the shell executor
        /// </summary>
        protected string CommandLine;

        /// <summary>
        /// Default constructor
        /// </summary>
        public BatchInput()
        {
            UnitsRepository = new Dictionary<string, string>();
        }

        /// <summary>
        /// Collect each line of the command line output.
        /// </summary>
        /// <param name="fromTriggerEventArgs">Trigger contextual event arg</param>
        /// <returns>a collection of <see cref="CollectedEvent"/> with the <see cref="CollectedEvent.Raw"/> property filled with a line of the command line output, excluding empty lines</returns>
        public override ICollection<ICollectedEvent> Collect(TriggerEventArgs fromTriggerEventArgs)
        {
            var collectedEvents = new List<ICollectedEvent>();
            foreach (var outputLine in GetBatchOutput(CommandLine))
            {
                if (!string.IsNullOrWhiteSpace(outputLine))
                {
                    collectedEvents.Add(new CollectedEvent()
                    {
                        Raw = outputLine,
                        AgentId = this.AgentId
                    });    
                }
            }
            return collectedEvents;
        }

        /// <summary>
        /// Initialize the current input using the passed IConfigurationSection for the input and the logger.
        /// Set default executor by os if not specified in configuration. Override default by the configuration.
        /// </summary>
        /// <param name="configurationInput">the <see cref="IInput"/> configuration</param>
        /// <param name="loggingConfiguration">the logger configuration</param>
        /// <param name="agentId">the AgentId to set in collected event</param>
        public override void Initialize(IConfigurationSection configurationInput,IConfigurationSection loggingConfiguration, string agentId)
        {
            base.Initialize(configurationInput, loggingConfiguration, agentId);
            SetExecutorByOs();
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

        /// <summary>
        /// Get the current platform 
        /// </summary>
        /// <returns>a OSPlatform corresponding to the current platform</returns>
        /// <exception cref="NotSupportedException">If the platform is not Linux, Windows, FreeBSD or OSX</exception>
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
        /// Set the default executor by OS. Today only Windows and linux are supported
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
        /// Execute the batch in external process and get batch output
        /// </summary>
        /// <param name="commandLine">the command line to be executed with the current ShellExecutor combined with Arguments</param>
        /// <returns>the batch output converted as a string array</returns>
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