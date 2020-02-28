using System;

namespace Canopee.Common
{
    public interface ICanopeeHost : IDisposable
    {
        public void Run();

        public void Stop();

    }
}