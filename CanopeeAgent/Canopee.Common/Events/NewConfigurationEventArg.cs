using System;

namespace Canopee.Common.Events
{
    public class NewConfigurationEventArg : EventArgs
    {
        public JsonObject NewConfiguration { get; set; }
    }
}