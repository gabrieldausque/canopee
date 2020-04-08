using Canopee.Common.Pipelines.Events;
using Microsoft.Extensions.Configuration;

namespace Canopee.Common.Pipelines
{
    public interface ITransform
    {
        ICollectedEvent Transform(ICollectedEvent input);
        void Initialize(IConfigurationSection transformConfiguration);
    }
}