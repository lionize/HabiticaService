using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Numerics;
using TIKSN.Data;
using TIKSN.Lionize.HabiticaTaskProviderService.Data.Serializers;

namespace TIKSN.Lionize.HabiticaTaskProviderService.Data.Entities
{
    public class UserProfileSettingsEntity : IEntity<BigInteger>
    {
        public string HabiticaApiTokenProtected { get; set; }

        public string HabiticaUserID { get; set; }

        [BsonId]
        [BsonSerializer(typeof(BigIntegerSerializer))]
        public BigInteger ID { get; set; }

        public Guid UserID { get; set; }
    }
}