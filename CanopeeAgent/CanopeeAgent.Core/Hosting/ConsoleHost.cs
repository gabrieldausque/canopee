using System;
using System.Threading;
using CanopeeAgent.Core.Indicators;

namespace CanopeeAgent.Core.Hosting
{
    public class ConsoleHost
    {
        private ManualResetEvent _exitEvent;
        private Collector _collector;

        public ConsoleHost()
        {
            _exitEvent = new ManualResetEvent(false);
            _collector = new Collector();
        }

        public void Run()
        {
            Console.WriteLine("Start the collector");
            Console.CancelKeyPress += this.Stop;
            _exitEvent.WaitOne();
        }

        private void Stop(object sender, ConsoleCancelEventArgs e)
        {
            e.Cancel = true;
            Console.WriteLine("");
            Console.WriteLine("Clearing collector instance");
            _collector.Dispose();
            Console.WriteLine("Exiting the host");
            _exitEvent.Set();
        }
    }
}