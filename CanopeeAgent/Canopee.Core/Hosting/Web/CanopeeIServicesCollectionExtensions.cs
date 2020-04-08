using System;
using Canopee.Common;
using Canopee.Common.Hosting;
using Canopee.Common.Pipelines;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Canopee.Core.Hosting.Web
{
    public static class CanopeeIServicesCollectionExtensions
    {
        public static IServiceCollection AddCanopeeHost(this IServiceCollection services, IConfiguration configuration)
        {
            //ensure the canopee host is instantiated by the dependency injection engine
            //that will ensure the dispose method will be called at the end of the application
            services.TryAddSingleton<IConfiguration>(configuration);
            var canopeeHost = new ASPNetCanopeeHost(configuration);
            services.AddSingleton<ICanopeeHost>(canopeeHost);
            services.AddSingleton<ITrigger>(canopeeHost.HostTrigger);
            return services;
        }
    }
}