using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using TIKSN.Habitica.Rest;
using TIKSN.Lionize.HabiticaTaskProviderService.Business.ProfileSettings;
using TIKSN.Lionize.HabiticaTaskProviderService.Business.Settings;
using TIKSN.Lionize.HabiticaTaskProviderService.Data.Entities;
using TIKSN.Lionize.HabiticaTaskProviderService.Data.Repositories;

namespace TIKSN.Lionize.HabiticaTaskProviderService.WebAPI.BackgroundServices
{
    public class PullTodosBackgroundService : BackgroundService
    {
        private readonly IHabiticaClient _habiticaClient;
        private readonly ILogger<PullTodosBackgroundService> _logger;
        private readonly IMapper _mapper;
        private readonly IProfileTodoRepository _profileTodoRepository;
        private readonly IServiceProvider _serviceProvider;
        private readonly IUserProfileSettingsRepository _userProfileSettingsRepository;
        private readonly IUserProfileSettingsService _userProfileSettingsService;

        public PullTodosBackgroundService(
            IServiceProvider serviceProvider,
            IUserProfileSettingsRepository userProfileSettingsRepository,
            IProfileTodoRepository profileTodoRepository,
            IUserProfileSettingsService userProfileSettingsService,
            IHabiticaClient habiticaClient,
            IMapper mapper,
            ILogger<PullTodosBackgroundService> logger)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _userProfileSettingsRepository = userProfileSettingsRepository ?? throw new ArgumentNullException(nameof(userProfileSettingsRepository));
            _userProfileSettingsService = userProfileSettingsService ?? throw new ArgumentNullException(nameof(userProfileSettingsService));
            _habiticaClient = habiticaClient ?? throw new ArgumentNullException(nameof(habiticaClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _profileTodoRepository = profileTodoRepository ?? throw new ArgumentNullException(nameof(profileTodoRepository));
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

                        await PullUserTodosAsync(profile.ID, profile.UserID, stoppingToken);

                        await Task.Delay(TimeSpan.FromSeconds(30)); //TODO: Get From Configuration
                    }
                }
            }

            await Task.Delay(TimeSpan.FromHours(1)); //TODO: Get From Configuration
        }

        private async Task PullUserTodosAsync(Guid profileID, Guid userID, CancellationToken cancellationToken)
        {
            var todos = await _habiticaClient.GetUserToDosAsync(cancellationToken);

            if (todos.Success)
            {
                foreach (var todo in todos.Data)
                {
                    var entity = new ProfileTodoEntity { ProviderProfileID = profileID, ProviderUserID = userID };
                    entity = _mapper.Map(todo, entity);

                    await _profileTodoRepository.AddOrUpdateAsync(entity, cancellationToken);
                }
            }
            else
            {
                _logger.LogError("Failed to get user todos.");
            }
        }
    }
}