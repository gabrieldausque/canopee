using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Canopee.Common;
using Canopee.Common.Hosting;
using Canopee.Common.Logging;
using Canopee.Core.Configuration;
using Canopee.Core.Hosting.Web;
using Canopee.Core.Logging;
using CanopeeElectronizedAgent.Datas;
using CanopeeElectronizedAgent.Host;
using ElectronNET.API;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CanopeeElectronizedAgent
{
    public class Startup
    {
        private readonly ICanopeeLogger _logger = null;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            _logger = CanopeeLoggerFactory.Instance().GetLogger(ConfigurationService.Instance.GetLoggingConfiguration(), this.GetType());
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            Console.WriteLine("Configuring services ...");
            var canopeeCoreAssembly = typeof(CollectedEventController).Assembly;
            services.AddElectronHost(Configuration)
                .AddSingleton<ILogRepository>(LogRepository.Instance)
                .AddControllersWithViews()
                .AddRazorRuntimeCompilation()
                .PartManager.ApplicationParts.Add(new AssemblyPart(canopeeCoreAssembly));
            services.AddHttpContextAccessor();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            var canopeeHost = app.ApplicationServices.GetService(typeof(ICanopeeHost)) as ICanopeeHost;
            canopeeHost.Start();
        }
    }
}