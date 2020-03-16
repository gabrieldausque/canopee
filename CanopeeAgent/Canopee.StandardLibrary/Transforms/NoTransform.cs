using System.Collections.Generic;
using System.Composition;
using Canopee.Common;
using Canopee.Core.Pipelines;
using Microsoft.Extensions.Configuration;

namespace Canopee.StandardLibrary.Transforms
{
    [Export("Default", typeof(ITransform))]
    public class NoTransform : BaseTransform
    {
        public override ICollectedEvent Transform(ICollectedEvent input)
        {
            return input;
        }

        public override void Initialize(IConfigurationSection transformConfiguration)
        {
            Logger.LogDebug("No transformation");
        }
    }
}