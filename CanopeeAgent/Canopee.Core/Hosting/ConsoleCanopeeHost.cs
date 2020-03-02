using System;
using System.Threading;
using Canopee.Common;
using Canopee.Core.Pipelines;
using Microsoft.AspNetCore.Authorization;

namespace Canopee.Core.Hosting
{
    /// <summary>
    /// The Canopee pipeline processing host for console application
    /// </summary>
    public class ConsoleCanopeeHost : ICanopeeHost
    {
        private ManualResetEvent _exitEvent;
        private CollectPipelineManager _collectPipelineManager;

        public ConsoleCanopeeHost()
        {
            _exitEvent = new ManualResetEvent(false);
            _collectPipelineManager = new CollectPipelineManager();
        }

        public void Run()
        {
            Console.WriteLine("Start the collector");
            Console.CancelKeyPress += this.Stop;
            Console.WriteLine("Press [CTRL+C] to close agent");
            _collectPipelineManager.Run();
            _exitEvent.WaitOne();
        }

        private void Stop(object sender, ConsoleCancelEventArgs e)
        {
            e.Cancel = true;
            this.Stop();
        }

        public void Stop()
        {
            Console.WriteLine("");
            Console.WriteLine("Clearing collector instance");
            _collectPipelineManager.Stop();
            Console.WriteLine("Exiting the host");
            _exitEvent.Set();
        }

        private bool _disposed = false;
        public void Dispose()
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