using System.Collections.Generic;
using System.Composition;
using Canopee.Common;
using Microsoft.Extensions.Configuration;

namespace Canopee.StandardLibrary.Transforms
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