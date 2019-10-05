using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using System.Threading.Tasks;
using TIKSN.DependencyInjection;
using TIKSN.Habitica;
using TIKSN.Lionize.Messaging.Services;

namespace TIKSN.Lionize.HabiticaTaskProviderService.WebAPI
{
    public class Program
    {
        public static IHostBuilder CreateWebHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    services.AddFrameworkPlatform();
                    services.AddHabitica();
                })
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .UseStartup<Startup>()
                        .UseSerilog((context, configuration) =>
                        {
                            configuration
                                .MinimumLevel.Debug()
                                //.MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                                //.MinimumLevel.Override("System", LogEventLevel.Warning)
                                //.MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
                                .Enrich.FromLogContext()
                                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}", theme: AnsiConsoleTheme.Literate);
                        });
                });
        }

        public static async Task Main(string[] args)
        {
            var webHost = CreateWebHostBuilder(args).Build();
            var publisherInitializerService = webHost.Services.GetRequiredService<IPublisherInitializerService>();
            await publisherInitializerService.InitializeAsync(default).ConfigureAwait(false);
            await webHost.RunAsync(default).ConfigureAwait(false);
        }
    }
}