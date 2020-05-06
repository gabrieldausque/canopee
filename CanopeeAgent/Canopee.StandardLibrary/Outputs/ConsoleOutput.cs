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
    /// <summary>
    /// Default <see cref="IOutput"/>. Send the json string that represent a <see cref="ICollectedEvent"/>
    ///
    /// The configuration will be :
    ///
    /// <example>
    ///
    /// <code>
    ///     {
    ///         ...
    ///         "Canopee": {
    ///             ...
    ///                 "Pipelines": [
    ///                  ...   
    ///                   {
    ///                     "Name": "OS",
    ///                     ...
    ///                     "Outputs" : {
    ///                         "OutputType": "Console",
    ///                    }
    ///                  ...
    ///                 }
    ///                 ...   
    ///                 ]
    ///             ...
    ///         }
    ///     }
    /// </code>
    /// 
    /// </example>
    /// 
    /// </summary>
    [Export("Console", typeof(IOutput))]
    [Export("Default", typeof(IOutput))]
    public class ConsoleOutput : BaseOutput
    {
        /// <summary>
        /// Send the <see cref="ICollectedEvent"/> serialized as JSON string to the console.
        /// </summary>
        /// <param name="collectedEvent">the collected event to display in console</param>
        public override void SendToOutput(ICollectedEvent collectedEvent)
        {
            Console.WriteLine(JsonSerializer.Serialize(collectedEvent,collectedEvent.GetType(), new  JsonSerializerOptions()
            {
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            }));
        }
    }
}