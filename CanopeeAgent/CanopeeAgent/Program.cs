using System;
using System.Composition.Hosting;
using System.IO;
using System.Net.Mime;
using System.Runtime.Loader;
using System.Threading;
using Canopee.Common;
using Canopee.Core.Configuration;
using Canopee.Core.Hosting;
using Canopee.Core.Hosting.Console;

namespace CanopeeAgent
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine(
                    $"ASPNETCORE_ENVIRONMENT = {Environment.ExpandEnvironmentVariables("%ASPNETCORE_ENVIRONMENT%")}");
                Console.WriteLine(
                    $"CANOPEE_ENVIRONMENT = {Environment.ExpandEnvironmentVariables("%CANOPEE_ENVIRONMENT%")}");
                
                var host = new ConsoleCanopeeHost(ConfigurationService.Instance.GetCanopeeConfiguration());
                host.Start();
                Console.WriteLine("Exiting ...");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}