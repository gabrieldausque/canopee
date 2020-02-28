using System;
using System.Composition.Hosting;
using System.IO;
using System.Net.Mime;
using System.Runtime.Loader;
using System.Threading;
using Canopee.Common;
using Canopee.Core.Hosting;

namespace CanopeeAgent
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var host = new ConsoleCanopeeHost();
                host.Run();
                Console.WriteLine("Exiting ...");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}