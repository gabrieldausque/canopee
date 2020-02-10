using System;
using System.Composition.Hosting;
using System.IO;
using System.Net.Mime;
using System.Runtime.Loader;
using System.Threading;
using CanopeeAgent.Common;

namespace CanopeeAgent
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = new ConsoleHost();
            host.Run();
            
            Console.WriteLine("Exiting ...");

        }
    }
}