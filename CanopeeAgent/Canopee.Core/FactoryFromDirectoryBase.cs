using System;
using System.Composition.Hosting;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Loader;

namespace Canopee.Core
{
    public class FactoryFromDirectoryBase
    {
        protected readonly CompositionHost Container;

        protected FactoryFromDirectoryBase(string directoryCatalog)
        {
            var containerConfiguration = new ContainerConfiguration();
            foreach (var assemblyPath in Directory.EnumerateFiles(directoryCatalog, "*.dll"))
            {
                var fullPath = Path.GetFullPath(assemblyPath);
                var assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(fullPath);
                containerConfiguration.WithAssembly(assembly);
            }

            containerConfiguration.WithAssembly(this.GetType().Assembly);
            containerConfiguration.WithAssembly(Assembly.GetEntryAssembly());
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