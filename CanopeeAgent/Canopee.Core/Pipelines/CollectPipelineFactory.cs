using Canopee.Common;
using Microsoft.Extensions.Configuration;

namespace Canopee.Core.Pipelines
{
    internal class CollectPipelineFactory : FactoryFromDirectoryBase
    {
        public CollectPipelineFactory(string directoryCatalog = @"./Pipelines") : base(directoryCatalog)
        {
        }

        public ICollectPipeline GetPipeline(IConfigurationSection pipelineConfiguration)
        {
            var type = string.IsNullOrWhiteSpace(pipelineConfiguration["Type"])?"Default": pipelineConfiguration["Type"];
            var pipeline = Container.GetExport<ICollectPipeline>(type);
            pipeline?.Initialize(pipelineConfiguration);
            return pipeline;
        }
    }
}