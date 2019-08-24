using AutoMapper;
using Microsoft.AspNetCore.DataProtection;
using TIKSN.Lionize.HabiticaTaskProviderService.Business.Models;
using TIKSN.Lionize.HabiticaTaskProviderService.Data.Entities;

namespace TIKSN.Lionize.HabiticaTaskProviderService.Business
{
    public class BusinessMappingProfile : Profile
    {
        public BusinessMappingProfile(IDataProtectionProvider provider)
        {
            var protector = provider.CreateProtector("Habitica");

            CreateMap<UserProfileSettingsEntity, UserProfileSettingsModel>()
                .ForMember(dest => dest.HabiticaApiToken, opt => opt.MapFrom(src => protector.Unprotect(src.HabiticaApiTokenProtected)));
        }
    }
}