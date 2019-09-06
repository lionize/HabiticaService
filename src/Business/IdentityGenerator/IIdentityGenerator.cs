namespace TIKSN.Lionize.HabiticaTaskProviderService.Business.IdentityGenerator
{
    public interface IIdentityGenerator<T>
    {
        T Generate();
    }
}