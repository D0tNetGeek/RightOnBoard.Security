using AutoMapper;

namespace RightOnBoard.Security.Api.ViewModels.Mappings
{
    public class ViewModelToEntityMappingProfile : Profile
    {
        public ViewModelToEntityMappingProfile()
        {
            CreateMap<RegistrationOptions, RegistrationOptions>().ForMember(au => au.CompanyId, map => map.MapFrom(vm => vm.CompanyId));
        }
    }
}
