using System.Composition.Hosting;
using System.IO;
using System.Runtime.Loader;

namespace CanopeeAgent.Core.Indicators
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
    }
}