using System;
using System.Threading;
using Canopee.Core.Pipelines;
using Microsoft.Extensions.Logging;

namespace Canopee.Core.Hosting.Console
{
    /// <summary>
    /// The Canopee pipeline processing host for console application
    /// </summary>
    public class ConsoleCanopeeHost : BaseCanopeeHost
    {
        /// <summary>
        /// The ManualResetEvent used to exit the process on user input
        /// </summary>
        private readonly ManualResetEvent _exitEvent;
        
        /// <summary>
        /// The pipelines manager
        /// </summary>
        private readonly CollectPipelineManager _collectPipelineManager;

        /// <summary>
        /// Default constructor
        /// </summary>
        public ConsoleCanopeeHost()
        {
            _exitEvent = new ManualResetEvent(false);
            _collectPipelineManager = new CollectPipelineManager();
        }

        /// <summary>
        /// Start the console and wait for [Ctrl+C] to stop agent
        /// </summary>
        public override void Start()
        {
            base.Start();
            Logger.LogInfo("Start the collector");
            System.Console.CancelKeyPress += this.StopFromInput;
            Logger.LogInfo("Press [CTRL+C] to close agent");
            if (CanRun)
            {
                _collectPipelineManager.Run();
                _exitEvent.WaitOne();
            }
            else
            {
                Logger.LogWarning("Host can't run ! Exiting ...");
            }
        }

        /// <summary>
        /// Execute stop method on User input
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StopFromInput(object sender, ConsoleCancelEventArgs e)
        {
            e.Cancel = true;
            this.Stop();
        }

        /// <summary>
        /// Stop all collect 
        /// </summary>
        public override void Stop()
        {
            Logger.LogInfo("");
            Logger.LogInfo("Clearing collector instance");
            _collectPipelineManager.Stop();
            Logger.LogInfo("Exiting the host");
            _exitEvent.Set();
        }

        /// <summary>
        /// The internal disposed flag
        /// </summary>
        private bool _disposed = false;
        
        /// <summary>
        /// Dispose the current host and suppress finalization
        /// </summary>
        public override void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// If the host is not already disposed, dispose pipeline manager and all needed component.
        /// </summary>
        /// <param name="disposing"></param>
        private void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                _exitEvent?.Dispose();
                _collectPipelineManager?.Dispose();
                _disposed = true;
            }
        }
    }
}