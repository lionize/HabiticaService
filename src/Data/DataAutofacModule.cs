using Autofac;
using TIKSN.Lionize.HabiticaTaskProviderService.Data.Repositories;

namespace TIKSN.Lionize.HabiticaTaskProviderService.Data
{
    public class DataAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UserProfileSettingsRepository>()
                .As<IUserProfileSettingsRepository>()
                .SingleInstance();
        }
    }
}