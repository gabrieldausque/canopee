using System.Composition.Hosting;
using System.IO;
using System.Runtime.Loader;
using CanopeeAgent.Common;
using CanopeeAgent.Core.Configuration;

namespace CanopeeAgent.Core.Indicators
{
    internal class IndicatorFactory : FactoryFromDirectoryBase
    {
        public IndicatorFactory() : base(@"./Indicators")
        {
        }

        public IIndicator GetIndicator(string indicatorType, IndicatorConfiguration configuration)
        {
            var indicator = Container.GetExport<IIndicator>(indicatorType);
            indicator?.Initialize(configuration);
            return indicator;
        }
    }
}