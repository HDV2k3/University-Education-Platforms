using AutoMapper;

using UNIVERSITY.EDUCATION.PLATFORMS.Application.Models.Users;
using UNIVERSITY.EDUCATION.PLATFORMS.Domain.Entities;

namespace UNIVERSITY.EDUCATION.PLATFORMS.Service.Mappings
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            CreateMap<CreateUserRequest, Users>();
            CreateMap<UpdateUserRequest, Users>();
            CreateMap<Users, UserDto>();
            CreateMap<Users, UserDetailResponse>();
        }
    }
}
