using System;

namespace TIKSN.Lionize.HabiticaTaskProviderService.Business.Settings
{
    public class CredentialSettingsStore : ICredentialSettingsStore
    {
        private string _apiKey;
        private string _userId;

        public string ApiKey { get => _apiKey; set => throw new NotSupportedException(); }

        public bool HasApiKey => !string.IsNullOrEmpty(ApiKey);

        public bool HasUserID => !string.IsNullOrEmpty(UserID);

        public string UserID { get => _userId; set => throw new NotSupportedException(); }

        public void Dispose()
        {
            _apiKey = null;
            _userId = null;
        }

        public void Store(string userId, string apiKey)
        {
            _userId = userId;
            _apiKey = apiKey;
        }
    }
}