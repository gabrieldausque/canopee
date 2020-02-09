using System;
using System.Composition.Hosting;
using System.IO;
using System.Runtime.Loader;
using CanopeeAgent.Common;

namespace CanopeeAgent
{
    class Program
    {
        static void Main(string[] args)
        {
            var configuration = new ContainerConfiguration();
            foreach (var assemblyPath in Directory.EnumerateFiles(@"./Indicators","*.dll"))
            {
                var fullPath = Path.GetFullPath(assemblyPath);
                var assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(fullPath);
                configuration.WithAssembly(assembly);
            }

            var container = configuration.CreateContainer();
            var hardwareIndicator = container.GetExport<IIndicator>("Hardware");
            
        }
    }
}