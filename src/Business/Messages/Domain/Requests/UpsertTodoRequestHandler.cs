using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TIKSN.Lionize.HabiticaTaskProviderService.Data.Entities;
using TIKSN.Lionize.HabiticaTaskProviderService.Data.Repositories;

namespace TIKSN.Lionize.HabiticaTaskProviderService.Business.Messages.Domain.Requests
{
    public class UpsertTodoRequestHandler : IRequestHandler<UpsertTodoRequest>
    {
        private readonly IProfileTodoRepository _profileTodoRepository;

        public UpsertTodoRequestHandler(IProfileTodoRepository profileTodoRepository)
        {
            _profileTodoRepository = profileTodoRepository ?? throw new ArgumentNullException(nameof(profileTodoRepository));
        }

        public async Task<Unit> Handle(UpsertTodoRequest request, CancellationToken cancellationToken)
        {
            var entity = await _profileTodoRepository.GetOrDefaultAsync(request.Data.Id, cancellationToken);

            if(entity == null)
            {
                entity = new ProfileTodoEntity
                {
                    ProviderUniformID = Guid.NewGuid(),
                    ProviderProfileID = request.ProfileID,
                    ProviderUserID = request.UserID
                };

                await _profileTodoRepository.AddAsync(entity, cancellationToken);
            }
            throw new NotImplementedException();
        }
    }
}