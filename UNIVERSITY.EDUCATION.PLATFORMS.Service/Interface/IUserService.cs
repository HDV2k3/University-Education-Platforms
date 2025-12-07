using UNIVERSITY.EDUCATION.PLATFORMS.Application.Models.Users;
using UNIVERSITY.EDUCATION.PLATFORMS.Domain.Entities;

namespace UNIVERSITY.EDUCATION.PLATFORMS.Service.Interface
{
    public interface IUserService : IAppBaseService<Users, Guid, CreateUserRequest, UpdateUserRequest, UserDto>
    {
        Task<UserDto> GetByUserNameAsync(string userName);
    }
}
