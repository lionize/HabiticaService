namespace TIKSN.Lionize.HabiticaTaskProviderService.Business.Messages.Integration
{
    public interface TaskUpserted
    {
        bool Completed { get; }

        string Text { get; }
    }
}