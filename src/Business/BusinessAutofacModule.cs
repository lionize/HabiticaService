using Autofac;
using TIKSN.Lionize.HabiticaTaskProviderService.Business.ProfileSettings;

namespace TIKSN.Lionize.HabiticaTaskProviderService.Business
{
    public class BusinessAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UserProfileSettingsService>()
                .As<IUserProfileSettingsService>()
                .SingleInstance();
        }
    }
}