using System;
using System.IO;
using Canopee.Common;
using Canopee.Core.Logging;
using Microsoft.Extensions.Configuration;

namespace Canopee.Core.Hosting
{
    public abstract class BaseCanopeeHost : ICanopeeHost
    {
        protected ICanopeeLogger Logger = null;

        public BaseCanopeeHost()
        {
            var configuration = Configuration.ConfigurationService.Instance.GetCanopeeConfiguration().GetSection("Logging");
            Logger = CanopeeLoggerFactory.Instance().GetLogger(configuration, this.GetType());   
        }
        
        public abstract void Dispose();
        public abstract void Run();
        public abstract void Stop();
    }
}