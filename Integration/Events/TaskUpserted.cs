namespace TIKSN.Lionize.HabiticaTaskProviderService.Integration.Events
{
    public interface TaskUpserted
    {
        bool Completed { get; }

        string Text { get; }
    }
}