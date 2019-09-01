using AutoMapper;
using MassTransit;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TIKSN.Lionize.HabiticaTaskProviderService.Business.Messages.Integration;
using TIKSN.Lionize.HabiticaTaskProviderService.Data.Entities;
using TIKSN.Lionize.HabiticaTaskProviderService.Data.Repositories;

namespace TIKSN.Lionize.HabiticaTaskProviderService.Business.Messages.Domain.Requests
{
    public class UpsertTodoRequestHandler : IRequestHandler<UpsertTodoRequest>
    {
        private readonly IEndpointAddressProvider _endpointAddressProvider;
        private readonly IMapper _mapper;
        private readonly IProfileTodoRepository _profileTodoRepository;
        private readonly ISendEndpointProvider _sendEndpointProvider;

        public UpsertTodoRequestHandler(
            IProfileTodoRepository profileTodoRepository,
            ISendEndpointProvider sendEndpointProvider,
            IEndpointAddressProvider endpointAddressProvider,
            IMapper mapper)
        {
            _profileTodoRepository = profileTodoRepository ?? throw new ArgumentNullException(nameof(profileTodoRepository));
            _sendEndpointProvider = sendEndpointProvider ?? throw new ArgumentNullException(nameof(sendEndpointProvider));
            _endpointAddressProvider = endpointAddressProvider ?? throw new ArgumentNullException(nameof(endpointAddressProvider));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<Unit> Handle(UpsertTodoRequest request, CancellationToken cancellationToken)
        {
            var entity = await _profileTodoRepository.GetOrDefaultAsync(request.Data.Id, cancellationToken);

            if (entity == null)
            {
                entity = new ProfileTodoEntity
                {
                    ProviderUniformID = Guid.NewGuid(),
                    ProviderProfileID = request.ProfileID,
                    ProviderUserID = request.UserID
                };
            }

            entity = _mapper.Map(entity, entity);

            await _profileTodoRepository.AddOrUpdateAsync(entity, cancellationToken);

            var sendEndpoint = await _sendEndpointProvider.GetSendEndpoint(_endpointAddressProvider.GetEndpointAddress("task_upserted_queue"));

            await sendEndpoint.Send<TaskUpserted>(new
            {
                Completed = entity.Completed.GetValueOrDefault(false),
                entity.Text
            });

            return Unit.Value;
        }
    }
}