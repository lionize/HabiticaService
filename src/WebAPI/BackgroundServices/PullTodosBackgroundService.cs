using AutoMapper;
using MassTransit;
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
using TIKSN.Lionize.HabiticaTaskProviderService.Integration;
using TIKSN.Lionize.HabiticaTaskProviderService.Integration.Events;

namespace TIKSN.Lionize.HabiticaTaskProviderService.WebAPI.BackgroundServices
{
    public class PullTodosBackgroundService : BackgroundService
    {
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
            IMapper mapper,
            ILogger<PullTodosBackgroundService> logger)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _userProfileSettingsRepository = userProfileSettingsRepository ?? throw new ArgumentNullException(nameof(userProfileSettingsRepository));
            _userProfileSettingsService = userProfileSettingsService ?? throw new ArgumentNullException(nameof(userProfileSettingsService));
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
                    var habiticaClient = scope.ServiceProvider.GetRequiredService<IHabiticaClient>();
                    var endpointAddressProvider = scope.ServiceProvider.GetRequiredService<IEndpointAddressProvider>();
                    var sendEndpoint = await scope.ServiceProvider.GetRequiredService<ISendEndpointProvider>().GetSendEndpoint(endpointAddressProvider.GetEndpointAddress("task_upserted_queue"));

                    foreach (var profile in profiles)
                    {
                        var credentials = await _userProfileSettingsService.GetCredentialAsync(profile.ID, stoppingToken);
                        credentialSettings.Store(credentials.HabiticaUserID, credentials.HabiticaApiToken);

                        try
                        {
                            _logger.LogInformation($"Pull todos for profile {profile.ID}");

                            await PullUserTodosAsync(habiticaClient, profile.ID, profile.UserID, sendEndpoint, stoppingToken);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, ex.Message);
                        }

                        await Task.Delay(TimeSpan.FromSeconds(30)); //TODO: Get From Configuration
                    }
                }
            }

            await Task.Delay(TimeSpan.FromHours(1)); //TODO: Get From Configuration
        }

        private async Task PullUserTodosAsync(IHabiticaClient habiticaClient, Guid profileID, Guid userID, ISendEndpoint sendEndpoint, CancellationToken cancellationToken)
        {
            var todos = await habiticaClient.GetUserToDosAsync(cancellationToken);
            var completedTodos = await habiticaClient.GetUserCompletedToDosAsync(cancellationToken);

            await UpdateRecordsAsync(profileID, userID, todos, sendEndpoint, cancellationToken);
            await UpdateRecordsAsync(profileID, userID, completedTodos, sendEndpoint, cancellationToken);
        }

        private async Task UpdateRecordsAsync(Guid profileID, Guid userID, Habitica.Models.UserTaskModel todos, ISendEndpoint sendEndpoint, CancellationToken cancellationToken)
        {
            if (todos.Success)
            {
                foreach (var todo in todos.Data)
                {
                    var entity = new ProfileTodoEntity { ProviderProfileID = profileID, ProviderUserID = userID };
                    entity = _mapper.Map(todo, entity);

                    await _profileTodoRepository.AddOrUpdateAsync(entity, cancellationToken);

                    await sendEndpoint.Send<TaskUpserted>(new
                    {
                        Completed = entity.Completed.GetValueOrDefault(false),
                        entity.Text
                    });
                }
            }
            else
            {
                _logger.LogError("Failed to get user todos.");
            }
        }
    }
}