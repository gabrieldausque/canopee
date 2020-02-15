using System;
using System.Collections.Generic;
using System.Composition;
using CanopeeAgent.Common;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace CanopeeAgent.StandardIndicators.Outputs
{
    [Export("Console", typeof(IOutput))]
    public class ConsoleOutput : IOutput
    {
        public void SendToOutput(ICollectedEvent collectedEvent)
        {
            Console.WriteLine(JsonConvert.SerializeObject(collectedEvent));
        }

        public void Initialize(IConfiguration configurationOutput)
        {
            //nothing to do for console
        }
    }
}