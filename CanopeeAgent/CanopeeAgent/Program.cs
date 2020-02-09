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
            var exitEvent = new ManualResetEvent(false);
            var configuration = new ContainerConfiguration();
            foreach (var assemblyPath in Directory.EnumerateFiles(@"./Indicators","*.dll"))
            {
                var fullPath = Path.GetFullPath(assemblyPath);
                var assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(fullPath);
                configuration.WithAssembly(assembly);
            }

            var container = configuration.CreateContainer();
            var hardwareIndicator = container.GetExport<IIndicator>("Hardware");

            Console.CancelKeyPress += (sender, eventArgs) =>
            {
                eventArgs.Cancel = true;
                Console.WriteLine("");
                exitEvent.Set();
            };
            
            exitEvent.WaitOne();
            Console.WriteLine("Exiting ...");

        }
    }
}