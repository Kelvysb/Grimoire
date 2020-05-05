using Grimoire.Business;
using Grimoire.Domain.Abstraction.Business;
using Grimoire.Domain.Abstraction.Services;
using Grimoire.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Grimoire.ConsoleUI
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var services = ConfigureServices();
            var serviceProvider = services.BuildServiceProvider();
            serviceProvider.GetService<App>().Run();
        }

        private static IServiceCollection ConfigureServices()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddTransient<App>();
            services.AddSingleton<IConfigurationService, ConfigurationService>();
            services.AddSingleton<IGrimoireService, GrimoireService>();
            services.AddSingleton<IGrimoireBusiness, GrimoireBusiness>();
            return services;
        }
    }
}