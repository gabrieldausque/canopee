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
            services.AddSingleton<ICanopeeHost>(provider => new CanopeeElectronHost(configuration));
            var canopeeHost = services.BuildServiceProvider().GetService(typeof(ICanopeeHost)) as CanopeeElectronHost;
            if (canopeeHost == null)
            {
                throw new NullReferenceException("The CanopeeHost instance is null, please check the assemblies you are using.");
            }
            canopeeHost.Run();
            services.AddSingleton<ITrigger>(canopeeHost.HostTrigger);
            return services;
        }
    }
}