using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace Canopee.Common
{
    public interface ITransform
    {
        ICollectedEvent Transform(ICollectedEvent input);
        void Initialize(IConfigurationSection transformConfiguration);
    }
}