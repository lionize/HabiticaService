using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TIKSN.Data.Mongo;
using TIKSN.Lionize.HabiticaTaskProviderService.Data.Entities;

namespace TIKSN.Lionize.HabiticaTaskProviderService.Data.Repositories
{
    public class UserProfileSettingsRepository : MongoRepository<UserProfileSettingsEntity, Guid>
    {
        public UserProfileSettingsRepository(IMongoDatabaseProvider mongoDatabaseProvider) : base(mongoDatabaseProvider, "UserProfileSettings")
        {
        }

        public async Task<IReadOnlyCollection<UserProfileSettingsEntity>> ListAsync(Guid userId, CancellationToken cancellationToken)
        {
            var cursor = await collection.FindAsync(Builders<UserProfileSettingsEntity>.Filter.Eq(item => item.UserID, userId), options: null, cancellationToken);
            return await cursor.ToListAsync(cancellationToken);
        }
    }
}