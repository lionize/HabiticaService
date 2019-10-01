using AutoMapper;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using TIKSN.Lionize.HabiticaTaskProviderService.Business.IdentityGenerator;
using TIKSN.Lionize.HabiticaTaskProviderService.Data.Entities;
using TIKSN.Lionize.HabiticaTaskProviderService.Data.Repositories;

namespace TIKSN.Lionize.HabiticaTaskProviderService.Business.ProfileSettings
{
    public class UserProfileSettingsService : IUserProfileSettingsService
    {
        private readonly IMapper _mapper;
        private readonly IUserProfileSettingsRepository _userProfileSettingsRepository;
        private readonly IIdentityGenerator<BigInteger> _identityGenerator;
        private readonly IHabiticaProfileService _habiticaProfileService;

        public UserProfileSettingsService(
            IMapper mapper,
            IUserProfileSettingsRepository userProfileSettingsRepository,
            IHabiticaProfileService habiticaProfileService,
            IIdentityGenerator<BigInteger> identityGenerator)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _userProfileSettingsRepository = userProfileSettingsRepository ?? throw new ArgumentNullException(nameof(userProfileSettingsRepository));
            _identityGenerator = identityGenerator ?? throw new ArgumentNullException(nameof(identityGenerator));
            _habiticaProfileService = habiticaProfileService ?? throw new ArgumentNullException(nameof(habiticaProfileService));
        }

        public async Task CreateAsync(Guid userId, UserProfileSettingsUpdateModel model, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<UserProfileSettingsEntity>(model);

            entity.UserID = userId;
            entity.ID = _identityGenerator.Generate();

            await _habiticaProfileService.GetAsync(model.HabiticaUserID, model.HabiticaApiToken, cancellationToken);

            await _userProfileSettingsRepository.AddAsync(entity, cancellationToken);
        }

        public async Task<UserProfileSettingsCredentialModel> GetCredentialAsync(BigInteger id, CancellationToken cancellationToken)
        {
            var entity = await _userProfileSettingsRepository.GetAsync(id, cancellationToken);

            return _mapper.Map<UserProfileSettingsCredentialModel>(entity);
        }

        public async Task<IReadOnlyCollection<UserProfileSettingsRetrievalModel>> ListAsync(Guid userId, CancellationToken cancellationToken)
        {
            var entities = await _userProfileSettingsRepository.ListAsync(userId, cancellationToken);
            var results = new List<UserProfileSettingsRetrievalModel>();

            foreach (var entity in entities)
            {
                var credential = _mapper.Map<UserProfileSettingsCredentialModel>(entity);
                var profile = await _habiticaProfileService.GetAsync(credential.HabiticaUserID, credential.HabiticaApiToken, cancellationToken);
                var result = _mapper.Map<UserProfileSettingsRetrievalModel>(entity);
                result = _mapper.Map(profile, result);
            }

            return results;
        }

        public async Task UpdateAsync(BigInteger id, Guid curentUserId, UserProfileSettingsUpdateModel updateModel, CancellationToken cancellationToken)
        {
            var entity = await _userProfileSettingsRepository.GetAsync(id, cancellationToken);

            if (entity.UserID != curentUserId)
            {
                throw new InvalidOperationException("User can't update other user's records");
            }

            entity = _mapper.Map(updateModel, entity);

            await _userProfileSettingsRepository.UpdateAsync(entity, cancellationToken);
        }
    }
}