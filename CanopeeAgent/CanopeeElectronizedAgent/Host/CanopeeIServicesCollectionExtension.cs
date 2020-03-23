using System;
using Canopee.Common;
using Canopee.Core.Hosting.Web;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CanopeeElectronizedAgent.Host
{
    public static class CanopeeIServicesCollectionExtension
    {
        public static IServiceCollection AddElectronHost(this IServiceCollection services, IConfiguration configuration)
        {
            services.TryAddSingleton<IConfiguration>(configuration);
            var canopeeHost = new CanopeeElectronHost(configuration);
            services.AddSingleton<ICanopeeHost>(canopeeHost);
            services.AddSingleton<ITrigger>(canopeeHost.HostTrigger);
            return services;
        }
    }
}