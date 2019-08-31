using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using TIKSN.Lionize.HabiticaTaskProviderService.Business.ProfileSettings;
using TIKSN.Lionize.HabiticaTaskProviderService.Business.Settings;
using TIKSN.Lionize.HabiticaTaskProviderService.Data.Repositories;

namespace TIKSN.Lionize.HabiticaTaskProviderService.WebAPI.BackgroundServices
{
    public class PullTodosBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IUserProfileSettingsRepository _userProfileSettingsRepository;
        private readonly IUserProfileSettingsService _userProfileSettingsService;

        public PullTodosBackgroundService(
            IServiceProvider serviceProvider,
            IUserProfileSettingsRepository userProfileSettingsRepository,
            IUserProfileSettingsService userProfileSettingsService)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _userProfileSettingsRepository = userProfileSettingsRepository ?? throw new ArgumentNullException(nameof(userProfileSettingsRepository));
            _userProfileSettingsService = userProfileSettingsService ?? throw new ArgumentNullException(nameof(userProfileSettingsService));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Constants retrieved from here https://habitica.fandom.com/wiki/Guidance_for_Comrades

            var userIds = await _userProfileSettingsRepository.ListUserIdsAsync(stoppingToken);

            foreach (var userId in userIds)
            {
                var profiles = await _userProfileSettingsRepository.ListAsync(userId, stoppingToken);
                using (var scope = _serviceProvider.CreateScope())
                using (var credentialSettings = scope.ServiceProvider.GetRequiredService<ICredentialSettingsStore>())
                {
                    foreach (var profile in profiles)
                    {
                        var credentials = await _userProfileSettingsService.GetCredentialAsync(profile.ID, stoppingToken);
                        credentialSettings.Store(credentials.HabiticaUserID, credentials.HabiticaApiToken);

                        await Task.Delay(TimeSpan.FromSeconds(30)); //TODO: Get From Configuration
                    }
                }
            }

            await Task.Delay(TimeSpan.FromHours(1)); //TODO: Get From Configuration
        }
    }
}