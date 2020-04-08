using Canopee.Common.Pipelines.Events;
using Microsoft.Extensions.Configuration;

namespace Canopee.Common.Pipelines
{
    public interface IOutput
    {
        void SendToOutput(ICollectedEvent collectedEvent);
        void Initialize(IConfiguration configurationOutput);
    }
}