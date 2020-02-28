using Canopee.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Canopee.Core.Hosting
{
    public static class CanopeeIServicesCollectionExtensions
    {
        public static void AddCanopeeHost(this IServiceCollection services, IConfiguration configuration)
        {
            var canopeeHost = new ASPNetCanopeeHost(configuration);
            services.AddSingleton<ICanopeeHost>(canopeeHost);
            services.AddSingleton<ITrigger>(canopeeHost.HostTrigger);
        }
    }
}