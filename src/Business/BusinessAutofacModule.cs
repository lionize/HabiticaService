using Autofac;
using TIKSN.Habitica.Settings;
using TIKSN.Lionize.HabiticaTaskProviderService.Business.ProfileSettings;
using TIKSN.Lionize.HabiticaTaskProviderService.Business.Settings;

namespace TIKSN.Lionize.HabiticaTaskProviderService.Business
{
    public class BusinessAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UserProfileSettingsService>()
                .As<IUserProfileSettingsService>()
                .SingleInstance();

            builder.RegisterType<CredentialSettingsStore>()
                .As<ICredentialSettingsStore>()
                .As<ICredentialSettings>()
                .InstancePerLifetimeScope();

            builder.RegisterType<EndpointAddressProvider>()
                .As<IEndpointAddressProvider>()
                .SingleInstance();
        }
    }
}