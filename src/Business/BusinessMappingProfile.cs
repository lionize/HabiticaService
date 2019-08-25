using AutoMapper;
using Microsoft.AspNetCore.DataProtection;
using TIKSN.Lionize.HabiticaTaskProviderService.Business.ProfileSettings;
using TIKSN.Lionize.HabiticaTaskProviderService.Data.Entities;

namespace TIKSN.Lionize.HabiticaTaskProviderService.Business
{
    public class BusinessMappingProfile : Profile
    {
        public BusinessMappingProfile(IDataProtectionProvider provider)
        {
            var protector = provider.CreateProtector("Habitica");

            CreateMap<UserProfileSettingsEntity, UserProfileSettingsRetrievalModel>();

            CreateMap<UserProfileSettingsEntity, UserProfileSettingsCredentialModel>()
                .ForMember(dest => dest.HabiticaApiToken, opt => opt.MapFrom(src => protector.Unprotect(src.HabiticaApiTokenProtected)));

            CreateMap<UserProfileSettingsUpdateModel, UserProfileSettingsEntity>()
                .ForMember(dest => dest.HabiticaApiTokenProtected, opt => opt.MapFrom(src => protector.Protect(src.HabiticaApiToken)))
                .ForMember(dest => dest.ID, opt => opt.Ignore())
                .ForMember(dest => dest.UserID, opt => opt.Ignore());
        }
    }
}