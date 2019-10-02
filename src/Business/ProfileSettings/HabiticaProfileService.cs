using System;
using System.Threading;
using System.Threading.Tasks;
using TIKSN.Habitica.Rest;
using TIKSN.Lionize.HabiticaTaskProviderService.Business.Models;
using TIKSN.Lionize.HabiticaTaskProviderService.Business.Settings;

namespace TIKSN.Lionize.HabiticaTaskProviderService.Business.ProfileSettings
{
    public class HabiticaProfileService : IHabiticaProfileService
    {
        private readonly ICredentialSettingsStore _credentialSettingsStore;
        private readonly IHabiticaClient _habiticaClient;

        public HabiticaProfileService(ICredentialSettingsStore credentialSettingsStore, IHabiticaClient habiticaClient)
        {
            _credentialSettingsStore = credentialSettingsStore ?? throw new ArgumentNullException(nameof(credentialSettingsStore));
            _habiticaClient = habiticaClient ?? throw new ArgumentNullException(nameof(habiticaClient));
        }

        public async Task<HabiticaProfileModel> GetAsync(string userId, string apiKey, CancellationToken cancellationToken)
        {
            _credentialSettingsStore.Store(userId, apiKey);

            var profile = await _habiticaClient.GetUserProfileAsync(cancellationToken);

            return new HabiticaProfileModel
            {
                FullName = profile.Data.Profile.Name,
                Username = profile.Data.Auth.Local.Username
            };
        }
    }
}
