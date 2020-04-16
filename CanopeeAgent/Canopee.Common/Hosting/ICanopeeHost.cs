using System;

namespace Canopee.Common.Hosting
{
    /// <summary>
    /// A host that will contain the Canope pipeline engine
    /// </summary>
    public interface ICanopeeHost : IDisposable
    {
        /// <summary>
        /// Start the host
        /// </summary>
        public void Start();

        /// <summary>
        /// Stop the host
        /// </summary>
        public void Stop();
        
    }
}