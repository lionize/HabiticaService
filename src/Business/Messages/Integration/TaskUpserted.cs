using System;

namespace TIKSN.Lionize.HabiticaTaskProviderService.Business.Messages.Integration
{
    public interface TaskUpserted
    {
        bool Completed { get; }
        DateTimeOffset CreatedAt { get; }
        string Description { get; }
        Guid ID { get; }
        Subtask[] Subtasks { get; }
        string Title { get; }
    }
}