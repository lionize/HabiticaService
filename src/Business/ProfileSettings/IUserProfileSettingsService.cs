using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;

namespace TIKSN.Lionize.HabiticaTaskProviderService.Business.ProfileSettings
{
    public interface IUserProfileSettingsService
    {
        Task CreateAsync(Guid userId, UserProfileSettingsUpdateModel model, CancellationToken cancellationToken);

        Task<UserProfileSettingsCredentialModel> GetCredentialAsync(BigInteger id, CancellationToken cancellationToken);

        Task<IReadOnlyCollection<UserProfileSettingsRetrievalModel>> ListAsync(Guid userId, CancellationToken cancellationToken);

        Task UpdateAsync(BigInteger id, Guid curentUserId, UserProfileSettingsUpdateModel updateModel, CancellationToken cancellationToken);
    }
}