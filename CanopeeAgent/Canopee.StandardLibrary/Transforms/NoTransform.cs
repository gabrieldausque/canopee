using System.Collections.Generic;
using System.Composition;
using Canopee.Common;
using Canopee.Common.Pipelines;
using Canopee.Common.Pipelines.Events;
using Canopee.Core.Pipelines;
using Microsoft.Extensions.Configuration;

namespace Canopee.StandardLibrary.Transforms
{
    [Export("Default", typeof(ITransform))]
    public class NoTransform : BaseTransform
    {
        public override ICollectedEvent Transform(ICollectedEvent collectedEventToTransform)
        {
            return collectedEventToTransform;
        }

        public override void Initialize(IConfigurationSection transformConfiguration, IConfigurationSection loggingConfiguration)
        {
            base.Initialize(transformConfiguration, loggingConfiguration);
            Logger.LogDebug("No transformation");
        }
    }
}