using System.Composition.Hosting;
using System.IO;
using System.Runtime.Loader;
using Canopee.Common;
using Canopee.Common.Configuration;
using Microsoft.Extensions.Configuration;

namespace Canopee.Core.Indicators
{
    internal class IndicatorFactory : FactoryFromDirectoryBase
    {
        public IndicatorFactory(string directoryCatalog = @"./Indicators") : base(directoryCatalog)
        {
        }

        public IIndicator GetIndicator(IConfigurationSection configurationIndicator)
        {
            var type = string.IsNullOrWhiteSpace(configurationIndicator["Type"])?"Default": configurationIndicator["Type"];
            var indicator = Container.GetExport<IIndicator>(type);
            indicator?.Initialize(configurationIndicator);
            return indicator;
        }
    }
}