using System;

namespace Canopee.Common.Configuration.Events
{
    public class NewConfigurationEventArg : EventArgs
    {
        public JsonObject NewConfiguration { get; set; }
    }
}