using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Chromely;
using Chromely.Core;
using Chromely.Core.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Grimoire
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var appName = Assembly.GetEntryAssembly()?.GetName().Name;
            var firstProcess = ServerAppUtil.IsMainProcess(args);
            var port = ServerAppUtil.AvailablePort;

            if (firstProcess)
            {
                if (port != -1)
                {
                    var blazorTask = new Task(() => CreateHostBuilder(args, port).Build().Run(), TaskCreationOptions.LongRunning);
                    blazorTask.Start();

                    while (ServerAppUtil.IsPortAvailable(port))
                    {
                        Thread.Sleep(1);
                    }
                }

                ServerAppUtil.SavePort(appName, port);
            }
            else
            {
                port = ServerAppUtil.GetSavedPort(appName);
            }

            if (port != -1)
            {
                var core = typeof(IChromelyConfiguration).Assembly;
                var config = DefaultConfiguration.CreateForRuntimePlatform();
                config.WindowOptions.Title = "Grimoire";
                config.StartUrl = $"http://127.0.0.1:{port}";
                config.DebuggingMode = true;
                config.WindowOptions.RelativePathToIconFile = "icon.ico";

                try
                {
                    var builder = AppBuilder.Create();
                    builder = builder.UseConfiguration<DefaultConfiguration>(config);
                    builder = builder.UseApp<GrimoireChromelyApp>();
                    builder = builder.Build();
                    builder.Run(args);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    throw;
                }
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args, int port) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                   .UseStartup<Startup>()
                   .UseUrls(new[] { $"http://127.0.0.1:{port}" });
                });
    }

    public class GrimoireChromelyApp : ChromelyBasicApp
    {
    }
}