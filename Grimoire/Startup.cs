using System.Threading.Tasks;
using ElectronNET.API;
using ElectronNET.API.Entities;
using Grimoire.Business;
using Grimoire.Domain.Abstraction.Business;
using Grimoire.Domain.Abstraction.Services;
using Grimoire.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Grimoire
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddSingleton<IConfigurationService, ConfigurationService>();
            services.AddSingleton<IGrimoireService, GrimoireService>();
            services.AddSingleton<IGrimoireBusiness, GrimoireBusiness>();
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
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });

            Task.Run(async () => await Electron.WindowManager.CreateWindowAsync(new BrowserWindowOptions()
            {
                Title = "Grimoire",
                Frame = true,
                AutoHideMenuBar = true,
                Width = 600,
                Height = 400,
                MinWidth = 300,
                MinHeight = 400
            }));
        }
    }
}