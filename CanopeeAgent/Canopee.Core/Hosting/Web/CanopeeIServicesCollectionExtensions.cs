using System;
using Canopee.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Canopee.Core.Hosting.Web
{
    public static class CanopeeIServicesCollectionExtensions
    {
        public static void AddCanopeeHost(this IServiceCollection services, IConfiguration configuration)
        {
            //ensure the canopee host is instantiated by the dependency injection engine
            //that will ensure the dispose method will be called at the end of the application
            services.TryAddSingleton<IConfiguration>(configuration);
            services.AddSingleton<ICanopeeHost>(provider => new ASPNetCanopeeHost(configuration));
            var canopeeHost = services.BuildServiceProvider().GetService(typeof(ICanopeeHost)) as ASPNetCanopeeHost;
            if (canopeeHost == null)
            {
                throw new NullReferenceException("The ASPCanopeeHost instance is null, please check the assemblies you are using.");
            }
            services.AddSingleton<ITrigger>(canopeeHost.HostTrigger);
        }
    }
}