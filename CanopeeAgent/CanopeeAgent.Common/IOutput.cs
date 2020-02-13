using System.Collections.Generic;

namespace CanopeeAgent.Common
{
    public interface IOutput
    {
        void SendToOutput(ICollectedEvent collectedEvent);
        void Initialize(Dictionary<string, string> configurationOutput);
    }
}