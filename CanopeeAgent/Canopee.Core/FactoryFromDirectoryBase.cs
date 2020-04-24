using System;
using System.Composition.Hosting;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Loader;

namespace Canopee.Core
{
    /// <summary>
    /// Base class for all factories that will load a catalog of specific operational contract based on a directory
    /// </summary>
    public class FactoryFromDirectoryBase
    {
        /// <summary>
        /// The MEF container
        /// </summary>
        protected readonly CompositionHost Container;

        /// <summary>
        /// Default constructor that initialize the <see cref="FactoryFromDirectoryBase.Container"/> from a directory by loading all assembly in it
        /// </summary>
        /// <param name="directoryCatalog">the directory from which to load the catalog</param>
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

        /// <summary>
        /// Helper that return the OSPlatform of the current workstation where the hosting process is running
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotSupportedException">If the OS is not a known OS</exception>
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