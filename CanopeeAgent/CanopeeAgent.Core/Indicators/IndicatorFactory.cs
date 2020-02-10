using System.Composition.Hosting;
using System.IO;
using System.Runtime.Loader;
using CanopeeAgent.Common;
using Microsoft.Extensions.Configuration;

namespace CanopeeAgent.Core.Indicators
{
    internal class IndicatorFactory
    {
        private ContainerConfiguration _containerConfiguration;
        private CompositionHost _container;

        public IndicatorFactory()
        {
            _containerConfiguration = new ContainerConfiguration();
            foreach (var assemblyPath in Directory.EnumerateFiles(@"./Indicators","*.dll"))
            {
                var fullPath = Path.GetFullPath(assemblyPath);
                var assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(fullPath);
                _containerConfiguration.WithAssembly(assembly);
            }

            _container = _containerConfiguration.CreateContainer();
        }

        public IIndicator GetIndicator(string indicatorName, IConfiguration configuration)
        {
            //TODO : do injection of component input, transform and output based on configuration
            return null;
        }
    }
}