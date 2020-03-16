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
        private readonly ManualResetEvent _exitEvent;
        private readonly CollectPipelineManager _collectPipelineManager;

        public ConsoleCanopeeHost()
        {
            _exitEvent = new ManualResetEvent(false);
            _collectPipelineManager = new CollectPipelineManager();
        }

        public override void Run()
        {
            Logger.LogInfo("Start the collector");
            System.Console.CancelKeyPress += this.Stop;
            Logger.LogInfo("Press [CTRL+C] to close agent");
            _collectPipelineManager.Run();
            _exitEvent.WaitOne();
        }

        private void Stop(object sender, ConsoleCancelEventArgs e)
        {
            e.Cancel = true;
            this.Stop();
        }

        public override void Stop()
        {
            Logger.LogInfo("");
            Logger.LogInfo("Clearing collector instance");
            _collectPipelineManager.Stop();
            Logger.LogInfo("Exiting the host");
            _exitEvent.Set();
        }

        private bool _disposed = false;
        public override void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

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