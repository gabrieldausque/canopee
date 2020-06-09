using System.Collections.Generic;
using System.Composition;
using Canopee.Common;
using Canopee.Common.Logging;
using Canopee.Common.Pipelines;
using Canopee.Common.Pipelines.Events;
using Canopee.Core.Pipelines;
using Microsoft.Extensions.Configuration;

namespace Canopee.StandardLibrary.Transforms
{
    /// <summary>
    /// The default transform, that does nothing.
    /// </summary>
    [Export("Default", typeof(ITransform))]
    public class NoTransform : BaseTransform
    {
        /// <summary>
        /// Return the collectedEvent without any transformation
        /// </summary>
        /// <param name="collectedEventToTransform">the <see cref="ICollectedEvent"/></param>
        /// <returns></returns>
        public override ICollectedEvent Transform(ICollectedEvent collectedEventToTransform)
        {
            return collectedEventToTransform;
        }

        /// <summary>
        /// Initialize the logger
        /// </summary>
        /// <param name="transformConfiguration">the <see cref="ITransform"/> configuration</param>
        /// <param name="loggingConfiguration">the <see cref="ICanopeeLogger"/></param>
        public override void Initialize(IConfigurationSection transformConfiguration, IConfigurationSection loggingConfiguration)
        {
            base.Initialize(transformConfiguration, loggingConfiguration);
            Logger.LogDebug("No transformation");
        }
    }
}