using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace Canopee.Common
{
    public interface IOutput
    {
        void SendToOutput(ICollectedEvent collectedEvent);
        void Initialize(IConfiguration configurationOutput);
    }
}