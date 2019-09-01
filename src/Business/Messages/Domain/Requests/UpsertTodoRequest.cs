using MediatR;
using System;
using TIKSN.Habitica.Models;

namespace TIKSN.Lionize.HabiticaTaskProviderService.Business.Messages.Domain.Requests
{
    public class UpsertTodoRequest : IRequest
    {
        public UpsertTodoRequest(TaskData data, Guid profileID, Guid userID)
        {
            Data = data ?? throw new ArgumentNullException(nameof(data));
            ProfileID = profileID;
            UserID = userID;
        }

        public TaskData Data { get; }
        public Guid ProfileID { get; }
        public Guid UserID { get; }
    }
}