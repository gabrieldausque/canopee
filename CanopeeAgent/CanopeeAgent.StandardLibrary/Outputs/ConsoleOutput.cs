using System;
using System.Collections.Generic;
using System.Composition;
using CanopeeAgent.Common;
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

        public void Initialize(Dictionary<string, string> configurationOutput)
        {
            //nothing to do for console
        }
    }
}