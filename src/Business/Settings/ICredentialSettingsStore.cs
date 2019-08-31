using System;
using TIKSN.Habitica.Settings;

namespace TIKSN.Lionize.HabiticaTaskProviderService.Business.Settings
{
    public interface ICredentialSettingsStore : ICredentialSettings, IDisposable
    {
        void Store(string userId, string apiKey);
    }
}