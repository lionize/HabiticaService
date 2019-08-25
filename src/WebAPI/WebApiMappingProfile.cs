using AutoMapper;
using Lionize.HabiticaTaskProvider.ApiModels.V1;
using TIKSN.Lionize.HabiticaTaskProviderService.Business.ProfileSettings;

namespace TIKSN.Lionize.HabiticaTaskProviderService.WebAPI
{
    public class WebApiMappingProfile : Profile
    {
        public WebApiMappingProfile()
        {
            CreateMap<UserProfileSettingsRetrievalModel, SettingsGetterItem>();
            CreateMap<SettingsSetterRequest, UserProfileSettingsUpdateModel>();
        }
    }
}