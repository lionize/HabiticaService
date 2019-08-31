using Autofac;

namespace TIKSN.Lionize.HabiticaTaskProviderService.Integration
{
    public class IntegrationAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<EndpointAddressProvider>()
                .As<IEndpointAddressProvider>()
                .SingleInstance();
        }
    }
}