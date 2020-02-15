using System.Collections.Generic;
using System.Composition;
using CanopeeAgent.Common;
using Microsoft.Extensions.Configuration;

namespace CanopeeAgent.StandardIndicators.Transforms
{
    [Export("Default", typeof(ITransform))]
    public class NoTransform : ITransform
    {
        public ICollectedEvent Transform(ICollectedEvent input)
        {
            return input;
        }

        public void Initialize(IConfigurationSection transformConfiguration)
        {
            //Do nothing for no transform
        }
    }
}