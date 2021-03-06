using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronNET.API;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CanopeeElectronizedAgent
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Launching application...");
                CreateHostBuilder(args).Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            
        }

        public static IWebHost CreateHostBuilder(string[] args)
        {
            Console.WriteLine("Creating host");
            return WebHost.CreateDefaultBuilder(args).UseStartup<Startup>().UseElectron(args).Build();
        }
            
    }
}