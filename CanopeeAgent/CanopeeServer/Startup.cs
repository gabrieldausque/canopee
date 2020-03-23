using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Canopee.Common;
using Canopee.Core.Hosting;
using Canopee.Core.Hosting.Web;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CanopeeServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        private IServiceCollection _services;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            _services = services;
            var canopeeCoreAssembly = typeof(CollectedEventController).Assembly;
            services.AddCanopeeHost(Configuration);
            services.AddHttpContextAccessor();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("CanopeeSpec", new Microsoft.OpenApi.Models.OpenApiInfo()
                {
                    Title = "Canopee Api",
                    Version = "0.1"
                });
                options.IncludeXmlComments("Canopee.Core.xml");
            });
            services.AddControllers()
                .PartManager.ApplicationParts.Add(new AssemblyPart(canopeeCoreAssembly));
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/CanopeeSpec/swagger.json", "Canopee API");
                options.RoutePrefix = "api";
            });
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            var canopeeHost = app.ApplicationServices.GetService(typeof(ICanopeeHost)) as ICanopeeHost;
            canopeeHost.Run();
        }
    }
}