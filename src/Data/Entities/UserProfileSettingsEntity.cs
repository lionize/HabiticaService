using System;
using TIKSN.Data;

namespace TIKSN.Lionize.Data.Entities
{
    public class UserProfileSettingsEntity : IEntity<Guid>
    {
        public string HabiticaApiTokenProtected { get; set; }

        public string HabiticaUserID { get; set; }

        public Guid ID { get; set; }

        public Guid UserID { get; set; }
    }
}