using System;
using Canopee.Common;
using Canopee.Common.Hosting;
using Canopee.Common.Pipelines;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Canopee.Core.Hosting.Web
{
    /// <summary>
    /// The IServicesCollection extension that allow adding CanopeeHost to AspNet Core services collection
    /// </summary>
    public static class CanopeeIServicesCollectionExtensions
    {
        /// <summary>
        /// Add AspNetCanopeeHost, and its ITrigger to the AspNet services collection
        /// </summary>
        /// <param name="services">The aspnet core services collection</param>
        /// <param name="configuration">The configuration of the current aspnet core server</param>
        /// <returns></returns>
        public static IServiceCollection AddCanopeeHost(this IServiceCollection services, IConfiguration configuration)
        {
            //ensure the canopee host is instantiated by the dependency injection engine
            //that will ensure the dispose method will be called at the end of the application
            services.TryAddSingleton<IConfiguration>(configuration);
            var canopeeHost = new ASPNetCanopeeHost(configuration.GetSection("Canopee"));
            services.AddSingleton<ICanopeeHost>(canopeeHost);
            services.AddSingleton<ITrigger>(canopeeHost.HostTrigger);
            return services;
        }
    }
}