using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using System.Threading.Tasks;
using TIKSN.Lionize.Messaging.Services;

namespace TIKSN.Lionize.HabiticaTaskProviderService.WebAPI
{
    public class Program
    {
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
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

        public static async Task Main(string[] args)
        {
            var webHost = CreateWebHostBuilder(args).Build();
            var publisherInitializerService = webHost.Services.GetRequiredService<IPublisherInitializerService>();
            await publisherInitializerService.InitializeAsync(default).ConfigureAwait(false);
            await webHost.RunAsync(default).ConfigureAwait(false);
        }
    }
}