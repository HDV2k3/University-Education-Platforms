
using UNIVERSITY.EDUCATION.PLATFORMS.Application.Models.Users;

namespace UNIVERSITY.EDUCATION.PLATFORMS.Application.Services.Interface
{
    public interface IUserService
    {
        Task<List<UserDto>> GetAllAsync();
        Task<UserDto?> GetByIdAsync(Guid id);
        Task<UserDto> CreateAsync(CreateUserRequest request);
        Task<UserDto> UpdateAsync(Guid id, UpdateUserRequest request);
        Task<bool> DeleteAsync(Guid id);
        Task<UserDto> GetByUserNameAsync(string userName);
    }
}
