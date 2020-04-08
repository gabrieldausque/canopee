using System;
using System.Collections.Generic;
using System.Composition;
using Canopee.Common;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using Canopee.Common.Pipelines;
using Canopee.Common.Pipelines.Events;
using Canopee.Core.Pipelines;

namespace Canopee.StandardLibrary.Outputs
{
    [Export("Console", typeof(IOutput))]
    [Export("Default", typeof(IOutput))]
    public class ConsoleOutput : BaseOutput
    {
        public override void SendToOutput(ICollectedEvent collectedEvent)
        {
            Console.WriteLine(JsonSerializer.Serialize(collectedEvent,collectedEvent.GetType()));
        }
    }
}