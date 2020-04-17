using Canopee.Common.Pipelines.Events;
using Microsoft.Extensions.Configuration;

namespace Canopee.Common.Pipelines
{
    /// <summary>
    /// The interface of the object that is in charge of enrich one or more event in a <see cref="ICollectPipeline"/> collect process after the extraction of it by a <see cref="IInput"/> object
    /// </summary>
    public interface ITransform
    {
        /// <summary>
        /// Transform a <see cref="ICollectedEvent"/> to another <see cref="ICollectedEvent"/> implementation or simply enrich it by adding properties to the <see cref="ICollectedEvent.ExtractedFields"/> property
        /// </summary>
        /// <param name="input">The <see cref="ICollectedEvent"/> to transform or enrich</param>
        /// <returns></returns>
        ICollectedEvent Transform(ICollectedEvent input);
        void Initialize(IConfigurationSection transformConfiguration);
    }
}