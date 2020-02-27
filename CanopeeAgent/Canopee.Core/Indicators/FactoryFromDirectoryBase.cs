using System;
using System.Composition.Hosting;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Loader;

namespace Canopee.Core.Indicators
{
    public class FactoryFromDirectoryBase
    {
        protected CompositionHost Container;

        public FactoryFromDirectoryBase(string directoryCatalog)
        {
            var containerConfiguration = new ContainerConfiguration();
            foreach (var assemblyPath in Directory.EnumerateFiles(directoryCatalog, "*.dll"))
            {
                var fullPath = Path.GetFullPath(assemblyPath);
                var assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(fullPath);
                containerConfiguration.WithAssembly(assembly);
            }
            Container = containerConfiguration.CreateContainer();
        }

        protected OSPlatform GetCurrentPlatform()
        {
            if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return OSPlatform.Linux;
            }

            if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return OSPlatform.Windows;
            }

            if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.FreeBSD))
            {
                return OSPlatform.FreeBSD;
            }

            if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                return OSPlatform.OSX;
            }
            throw new NotSupportedException("The current OS is not Supported");
        }
    }
}