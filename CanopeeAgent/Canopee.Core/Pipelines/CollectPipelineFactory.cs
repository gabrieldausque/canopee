using Canopee.Common;
using Microsoft.Extensions.Configuration;

namespace Canopee.Core.Pipelines
{
    internal class CollectPipelineFactory : FactoryFromDirectoryBase
    {
        public CollectPipelineFactory(string directoryCatalog = @"./Pipelines") : base(directoryCatalog)
        {
        }

        public ICollectPipeline GetIndicator(IConfigurationSection configurationIndicator)
        {
            var type = string.IsNullOrWhiteSpace(configurationIndicator["Type"])?"Default": configurationIndicator["Type"];
            var indicator = Container.GetExport<ICollectPipeline>(type);
            indicator?.Initialize(configurationIndicator);
            return indicator;
        }
    }
}