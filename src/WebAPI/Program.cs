using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using System;
using System.Threading.Tasks;
using TIKSN.Lionize.Messaging.Services;

namespace TIKSN.Lionize.HabiticaTaskProviderService.WebAPI
{
    public class Program
    {
        public static IHostBuilder CreateWebHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .UseStartup<Startup>()
                        .UseSerilog((context, configuration) =>
                        {
                            configuration
                                .MinimumLevel.Verbose()
                                //.MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                                //.MinimumLevel.Override("System", LogEventLevel.Warning)
                                //.MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
                                .Enrich.FromLogContext()
                                .Enrich.WithProperty("ApplictionName", "HabiticaTaskProviderService")
                                .Enrich.WithProperty("ApplictionInstance", Guid.NewGuid())
                                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}", theme: AnsiConsoleTheme.Literate);

                            var baseAddress = context.Configuration.GetSection("Logs").GetSection("Seq").GetValue<string>("BaseAddress");
                            if (!string.IsNullOrEmpty(baseAddress))
                            {
                                configuration.WriteTo.Seq(baseAddress);
                            }
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