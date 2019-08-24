using System;

namespace TIKSN.Lionize.Business.Models
{
    internal class UserProfileSettingsModel
    {
        public string HabiticaApiToken { get; set; }

        public string HabiticaUserID { get; set; }

        public Guid ID { get; set; }

        public Guid UserID { get; set; }
    }
}