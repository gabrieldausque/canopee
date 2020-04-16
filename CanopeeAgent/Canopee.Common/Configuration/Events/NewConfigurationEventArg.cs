using System;

namespace Canopee.Common.Configuration.Events
{
    /// <summary>
    /// Args for the new configuration event. When a new configuration is detected by the <see cref="IConfigurationSynchronizer"/>, it raise <see cref="IConfigurationSynchronizer.OnNewConfiguration"/> event.
    /// </summary>
    public class NewConfigurationEventArg : EventArgs
    {
        /// <summary>
        /// The new configuration obtained
        /// </summary>
        public JsonObject NewConfiguration { get; set; }
    }
}