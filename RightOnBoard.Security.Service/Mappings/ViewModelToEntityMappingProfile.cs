using AutoMapper;
using RightOnBoard.Security.Service.Models;
using RightOnBoard.Security.Service.Models.Entities;

namespace RightOnBoard.Security.Service.Mappings
{
    public class ViewModelToEntityMappingProfile : Profile
    {
        public ViewModelToEntityMappingProfile()
        {
            CreateMap<RegistrationModel, ApplicationUser>().ForMember(au => au.UserName, map => map.MapFrom(vm => vm.Email));
        }
    }
}
