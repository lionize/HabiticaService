using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using TIKSN.Data.Mongo;
using TIKSN.Lionize.HabiticaTaskProviderService.Data.Entities;

namespace TIKSN.Lionize.HabiticaTaskProviderService.Data.Repositories
{
    public interface IUserProfileSettingsRepository : IMongoRepository<UserProfileSettingsEntity, BigInteger>
    {
        Task<bool> ExistsHabiticaUserIdAsync(string habiticaUserID, CancellationToken cancellationToken);

        Task<IReadOnlyCollection<UserProfileSettingsEntity>> ListAsync(Guid userId, CancellationToken cancellationToken);

        Task<IReadOnlyCollection<Guid>> ListUserIdsAsync(CancellationToken cancellationToken);
    }
}