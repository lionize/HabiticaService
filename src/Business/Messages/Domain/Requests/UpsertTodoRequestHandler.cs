using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using TIKSN.Habitica.Models;
using TIKSN.Integration.Correlation;
using TIKSN.Lionize.HabiticaTaskProviderService.Business.IdentityGenerator;
using TIKSN.Lionize.HabiticaTaskProviderService.Data.Entities;
using TIKSN.Lionize.HabiticaTaskProviderService.Data.Repositories;
using TIKSN.Lionize.Messaging.Services;
using TIKSN.Time;

namespace TIKSN.Lionize.HabiticaTaskProviderService.Business.Messages.Domain.Requests
{
    public class UpsertTodoRequestHandler : IRequestHandler<UpsertTodoRequest>
    {
        private readonly IIdentityGenerator<BigInteger> _identityGenerator;
        private readonly IMapper _mapper;
        private readonly IProfileTodoRepository _profileTodoRepository;
        private readonly IPublisherService _publisherService;
        private readonly ITimeProvider _timeProvider;
        private readonly ICorrelationService _correlationService;

        public UpsertTodoRequestHandler(
            IProfileTodoRepository profileTodoRepository,
            IIdentityGenerator<BigInteger> identityGenerator,
            IPublisherService publisherService,
            ITimeProvider timeProvider,
            ICorrelationService correlationService,
            IMapper mapper)
        {
            _profileTodoRepository = profileTodoRepository ?? throw new ArgumentNullException(nameof(profileTodoRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _identityGenerator = identityGenerator ?? throw new ArgumentNullException(nameof(identityGenerator));
            _publisherService = publisherService ?? throw new ArgumentNullException(nameof(publisherService));
            _timeProvider = timeProvider ?? throw new ArgumentNullException(nameof(timeProvider));
            _correlationService = correlationService ?? throw new ArgumentNullException(nameof(correlationService));
        }

        public async Task<Unit> Handle(UpsertTodoRequest request, CancellationToken cancellationToken)
        {
            var entity = await _profileTodoRepository.GetOrDefaultAsync(request.Data.Id, cancellationToken);

            if (entity == null)
            {
                entity = new ProfileTodoEntity
                {
                    ProviderUniformID = _identityGenerator.Generate(),
                    ProviderProfileID = request.ProfileID,
                    ProviderUserID = request.UserID,
                    Checklist = new List<ProfileTodoEntity.ChecklistItemModel>()
                };
            }
            var oldChecklist = entity.Checklist.ToArray();

            entity = _mapper.Map(request.Data, entity);

            SyncSubtasks(request.Data.Checklist, entity, oldChecklist);

            await _profileTodoRepository.AddOrUpdateAsync(entity, cancellationToken).ConfigureAwait(false);

            await _publisherService.ProduceAsync(new global::Lionize.IntegrationMessages.TaskUpserted
            {
                Completed = entity.Completed.GetValueOrDefault(false),
                CreatedAt = new global::Lionize.IntegrationMessages.Moment
                {
                    Value = _timeProvider.GetCurrentTime().ToUnixTimeSeconds()
                },
                Description = entity.Notes,
                ID = new global::Lionize.IntegrationMessages.BigInteger
                {
                    Value = entity.ProviderUniformID.ToByteArray()
                },
                Title = entity.Text
            }, _correlationService.Generate(), cancellationToken).ConfigureAwait(false);

            return Unit.Value;
        }

        private void SyncSubtasks(List<ChecklistItem> checklist, ProfileTodoEntity entity, ProfileTodoEntity.ChecklistItemModel[] oldChecklist)
        {
            entity.Checklist.RemoveAll(item => !checklist.Any(x => x.Id == item.Id));

            foreach (var checklistItem in checklist)
            {
                var oldChecklistItem = oldChecklist.SingleOrDefault(x => x.Id == checklistItem.Id);
                var newChecklistItem = entity.Checklist.Single(x => x.Id == checklistItem.Id);

                if (oldChecklistItem == null)
                {
                    newChecklistItem.ProviderUniformID = _identityGenerator.Generate();
                }
                else
                {
                    newChecklistItem.ProviderUniformID = oldChecklistItem.ProviderUniformID;
                }
            }
        }
    }
}