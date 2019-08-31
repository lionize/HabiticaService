using AutoMapper;
using Lionize.HabiticaTaskProvider.ApiModels.V1;
using TIKSN.Lionize.HabiticaTaskProviderService.Business.ProfileSettings;
using TIKSN.Lionize.HabiticaTaskProviderService.Data.Entities;

namespace TIKSN.Lionize.HabiticaTaskProviderService.WebAPI
{
    public class WebApiMappingProfile : Profile
    {
        public WebApiMappingProfile()
        {
            CreateMap<UserProfileSettingsRetrievalModel, SettingsGetterItem>();
            CreateMap<SettingsSetterRequest, UserProfileSettingsUpdateModel>();

            CreateMap<Habitica.Models.TaskData, ProfileTodoEntity>()
                .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ProviderProfileID, opt => opt.Ignore())
                .ForMember(dest => dest.ProviderUserID, opt => opt.Ignore());

            CreateMap<TIKSN.Habitica.Models.ChecklistItem, ProfileTodoEntity.ChecklistItemModel>();
            CreateMap<TIKSN.Habitica.Models.Repeat, ProfileTodoEntity.RepeatModel>();
        }
    }
}