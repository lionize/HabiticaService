using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace TIKSN.Lionize.HabiticaTaskProviderService.WebAPI.BackgroundServices
{
    public class PullTodosBackgroundService : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Delay(TimeSpan.FromHours(1)); //TODO: Get From Configuration
        }
    }
}