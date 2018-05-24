using AutoMapper;

namespace RightOnBoard.Security.Api.ViewModels.Mappings
{
    public class ViewModelToEntityMappingProfile : Profile
    {
        public ViewModelToEntityMappingProfile()
        {
            //CreateMap<RegistrationViewModel, ApplicationUser1>().ForMember(au => au.UserName, map => map.MapFrom(vm => vm.Email));
        }
    }
}
