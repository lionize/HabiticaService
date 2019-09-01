using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TIKSN.Lionize.HabiticaTaskProviderService.Business.Messages.Domain.Requests
{
    public class UpsertTodoRequestHandler : IRequestHandler<UpsertTodoRequest>
    {
        public async Task<Unit> Handle(UpsertTodoRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}