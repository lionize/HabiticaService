namespace TIKSN.Lionize.HabiticaTaskProviderService.Business.Messages.Integration
{
    public interface Subtask
    {
        bool Completed { get; }
        int ID { get; }
        string Title { get; }
    }
}