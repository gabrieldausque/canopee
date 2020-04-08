using System;

namespace Canopee.Common.Hosting
{
    public interface ICanopeeHost : IDisposable
    {
        public void Run();

        public void Stop();
        
    }
}