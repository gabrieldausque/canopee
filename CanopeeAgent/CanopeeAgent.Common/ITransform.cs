using System.Collections.Generic;

namespace CanopeeAgent.Common
{
    public interface ITransform
    {
        ICollectedEvent Transform(ICollectedEvent input);
        void Initialize(Dictionary<string, string> transformConfiguration);
    }
}