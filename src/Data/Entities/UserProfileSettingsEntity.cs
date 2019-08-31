using MongoDB.Bson.Serialization.Attributes;
using System;
using TIKSN.Data;

namespace TIKSN.Lionize.HabiticaTaskProviderService.Data.Entities
{
    public class UserProfileSettingsEntity : IEntity<Guid>
    {
        public string HabiticaApiTokenProtected { get; set; }

        public string HabiticaUserID { get; set; }

        [BsonId]
        public Guid ID { get; set; }

        public Guid UserID { get; set; }
    }
}