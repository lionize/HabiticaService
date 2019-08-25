using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TIKSN.Lionize.HabiticaTaskProviderService.Data.Repositories;

namespace TIKSN.Lionize.HabiticaTaskProviderService.Business.ProfileSettings
{
    public class UserProfileSettingsService
    {
        private readonly IMapper _mapper;
        private readonly IUserProfileSettingsRepository _userProfileSettingsRepository;

        public UserProfileSettingsService(IMapper mapper, IUserProfileSettingsRepository userProfileSettingsRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _userProfileSettingsRepository = userProfileSettingsRepository ?? throw new ArgumentNullException(nameof(userProfileSettingsRepository));
        }

        public async Task<IReadOnlyCollection<UserProfileSettingsRetrievalModel>> ListAsync(Guid userId, CancellationToken cancellationToken)
        {
        }

        public async Task UpdateAsync(Guid id, Guid curentUserId, UserProfileSettingsUpdateModel updateModel, CancellationToken cancellationToken)
        {
            var entity = await _userProfileSettingsRepository.GetAsync(id, cancellationToken);

            if (entity.UserID != curentUserId)
                throw new InvalidOperationException("User can't update other user's records");

            entity = _mapper.Map(updateModel, entity);

            await _userProfileSettingsRepository.UpdateAsync(entity, cancellationToken);
        }
    }
}