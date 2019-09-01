using MediatR;
using System;
using TIKSN.Habitica.Models;

namespace TIKSN.Lionize.HabiticaTaskProviderService.Business.Messages.Domain.Requests
{
    public class UpsertTodoRequest : IRequest
    {
        public UpsertTodoRequest(TaskData data)
        {
            Data = data ?? throw new ArgumentNullException(nameof(data));
        }

        public TaskData Data { get; }
    }
}