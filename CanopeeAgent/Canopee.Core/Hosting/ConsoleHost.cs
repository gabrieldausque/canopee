using System;
using System.Threading;
using Canopee.Core.Pipelines;

namespace Canopee.Core.Hosting
{
    public class ConsoleHost
    {
        private ManualResetEvent _exitEvent;
        private CollectPipelineManager _collectPipelineManager;

        public ConsoleHost()
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
            Console.WriteLine("");
            Console.WriteLine("Clearing collector instance");
            _collectPipelineManager.Dispose();
            Console.WriteLine("Exiting the host");
            _exitEvent.Set();
        }
    }
}