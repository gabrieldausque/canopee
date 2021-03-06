using CanopeeServer.Datas;
using CanopeeServer.Repository;
using CanopeeServer.Repository.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CanopeeServer.Helpers
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddCanopeeServerDbContext(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<CanopeeServerDbSettings>(configuration.GetSection("Canopee:Db"));
            services.AddSingleton<CanopeeServerDbContext>();
            services.AddSingleton<IGroupRepository, GroupRepository>();
            services.AddSingleton<ICanopeeConfigurationRepository, CanopeeConfigurationRepository>();
            return services;
        } 
        
        
    }
}