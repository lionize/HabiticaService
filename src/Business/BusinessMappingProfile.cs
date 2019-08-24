using AutoMapper;
using Microsoft.AspNetCore.DataProtection;

namespace TIKSN.Lionize.HabiticaTaskProviderService.Business
{
    public class BusinessMappingProfile : Profile
    {
        public BusinessMappingProfile(IDataProtectionProvider provider)
        {
            var protector = provider.CreateProtector("Habitica");
        }
    }
}