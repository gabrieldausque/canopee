using Canopee.Common;
using Canopee.Common.Pipelines;
using Microsoft.Extensions.Configuration;

namespace Canopee.Core.Pipelines
{
    /// <summary>
    /// Factory in charge of creating a <see cref="ICollectPipeline"/>
    /// </summary>
    public class CollectPipelineFactory : FactoryFromDirectoryBase
    {
        /// <summary>
        /// Default contstructor
        /// </summary>
        /// <param name="directoryCatalog">
        /// The directory from which the available pipeline catalog will be loaded. Optionnal
        /// Default : ./Pipelines
        /// </param>
        public CollectPipelineFactory(string directoryCatalog = @"./Pipelines") : base(directoryCatalog)
        {
        }

        /// <summary>
        /// Get the pipeline corresponding to the pipeline configuration section passed 
        /// </summary>
        /// <param name="pipelineConfiguration">
        /// The pipeline configuration that defines the wanted pipeline. If no Type of pipeline defined explicitely, use the "Default" type 
        /// </param>
        /// <param name="loggingConfiguration">The logger configuration</param>
        /// <returns>a <see cref="ICollectPipeline"/></returns>
        public ICollectPipeline GetPipeline(IConfigurationSection pipelineConfiguration, IConfigurationSection loggingConfiguration)
        {
            var type = string.IsNullOrWhiteSpace(pipelineConfiguration["Type"])?"Default": pipelineConfiguration["Type"];
            var pipeline = Container.GetExport<ICollectPipeline>(type);
            pipeline?.Initialize(pipelineConfiguration, loggingConfiguration);
            return pipeline;
        }
    }
}