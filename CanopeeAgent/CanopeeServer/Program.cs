using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CanopeeServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine(
                $"ASPNETCORE_ENVIRONMENT = {Environment.ExpandEnvironmentVariables("%ASPNETCORE_ENVIRONMENT%")}");
            Console.WriteLine(
                $"CANOPEE_ENVIRONMENT = {Environment.ExpandEnvironmentVariables("%CANOPEE_ENVIRONMENT%")}");
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var hostingFileName =
                (!string.IsNullOrWhiteSpace(Environment.ExpandEnvironmentVariables("%CANOPEE_ENVIRONMENT%")))
                    ? $"hosting.{Environment.ExpandEnvironmentVariables("%CANOPEE_ENVIRONMENT%")}.json"
                    : $"hosting.json";  
            Console.WriteLine($"Loading hosting files : {hostingFileName}");
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(hostingFileName, true)
                .AddCommandLine(args)
                .Build();
            
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    if (!string.IsNullOrWhiteSpace(config["Urls"]))
                    {
                        Console.WriteLine($"Changing Urls to : {config["Urls"]}");
                        webBuilder.UseUrls(config["Urls"]);
                    }
                    else
                    {
                        Console.WriteLine("No hosting configuration for Urls");
                    }
                });

        }
    }
}