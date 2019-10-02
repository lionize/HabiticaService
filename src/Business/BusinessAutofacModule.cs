using Autofac;
using MathNet.Numerics.Random;
using System;
using System.Numerics;
using TIKSN.Habitica.Settings;
using TIKSN.Lionize.HabiticaTaskProviderService.Business.IdentityGenerator;
using TIKSN.Lionize.HabiticaTaskProviderService.Business.ProfileSettings;
using TIKSN.Lionize.HabiticaTaskProviderService.Business.Settings;
using TIKSN.Serialization.Numerics;

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

            builder.RegisterType<CryptoRandomSource>()
                .As<Random>()
                .SingleInstance();

            builder.RegisterType<UnsignedBigIntegerIdentityGenerator>()
                .As<IIdentityGenerator<BigInteger>>()
                .SingleInstance();

            builder.RegisterType<UnsignedBigIntegerBinaryDeserializer>()
                .SingleInstance();

            builder
                .RegisterType<HabiticaProfileService>()
                .As<IHabiticaProfileService>()
                .InstancePerLifetimeScope();

            builder
                .RegisterDecorator<HabiticaProfileCacheService, IHabiticaProfileService>();
        }
    }
}