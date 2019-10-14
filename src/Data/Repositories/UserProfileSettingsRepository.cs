using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using TIKSN.Data.Mongo;
using TIKSN.Lionize.HabiticaTaskProviderService.Data.Entities;

namespace TIKSN.Lionize.HabiticaTaskProviderService.Data.Repositories
{
    public class UserProfileSettingsRepository : MongoRepository<UserProfileSettingsEntity, BigInteger>, IUserProfileSettingsRepository
    {
        public UserProfileSettingsRepository(IMongoDatabaseProvider mongoDatabaseProvider) : base(mongoDatabaseProvider, "UserProfileSettings")
        {
        }

        public async Task<bool> ExistsHabiticaUserIdAsync(string habiticaUserID, CancellationToken cancellationToken)
        {
            var cursor = await collection.FindAsync(Builders<UserProfileSettingsEntity>.Filter.Eq(item => item.HabiticaUserID, habiticaUserID), options: null, cancellationToken);
            return await cursor.AnyAsync(cancellationToken);
        }

        public async Task<IReadOnlyCollection<UserProfileSettingsEntity>> ListAsync(Guid userId, CancellationToken cancellationToken)
        {
            var cursor = await collection.FindAsync(Builders<UserProfileSettingsEntity>.Filter.Eq(item => item.UserID, userId), options: null, cancellationToken);
            return await cursor.ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyCollection<Guid>> ListUserIdsAsync(CancellationToken cancellationToken)
        {
            var cursor = await collection.DistinctAsync(item => item.UserID, Builders<UserProfileSettingsEntity>.Filter.Empty, null, cancellationToken);
            return await cursor.ToListAsync(cancellationToken);
        }
    }
}