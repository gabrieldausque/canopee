using System;

namespace Canopee.Common.Configuration.Events
{
    /// <summary>
    /// Args for the new configuration event. When a new configuration is detected by the <see cref="ICanopeeConfigurationSynchronizer"/>, it raise <see cref="ICanopeeConfigurationSynchronizer.OnNewConfiguration"/> event.
    /// </summary>
    public class NewConfigurationEventArg : EventArgs
    {
        /// <summary>
        /// The new configuration obtained
        /// </summary>
        public JsonObject NewConfiguration { get; set; }
    }
}