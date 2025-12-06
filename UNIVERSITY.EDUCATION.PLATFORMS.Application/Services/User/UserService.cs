using Microsoft.EntityFrameworkCore;
using UNIVERSITY.EDUCATION.PLATFORMS.Application.Models.Users;
using UNIVERSITY.EDUCATION.PLATFORMS.Application.Services.Interface;
using UNIVERSITY.EDUCATION.PLATFORMS.Domain.Entities;
namespace UNIVERSITY.EDUCATION.PLATFORMS.Application.Services.User
{
    public class UserService : IUserService
    {
        private readonly IApplicationDbContext _db;

        public UserService(IApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<UserDto>> GetAllAsync()
        {
            return await _db.ToQueryable<Users>()  // Soft delete filter auto applied
                .Include(u => u.UserType)
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    FullName = u.FullName,
                    Email = u.Email,
                    IsActive = u.IsActive,
                    UserTypeId = u.UserTypeId,
                    UserTypeName = u.UserType.Name
                })
                .ToListAsync();
        }

        public async Task<UserDto?> GetByIdAsync(Guid id)
        {
            return await _db.ToQueryable<Users>()
                .Include(u => u.UserType)
                .Where(u => u.Id == id)
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    FullName = u.FullName,
                    Email = u.Email,
                    IsActive = u.IsActive,
                    UserTypeId = u.UserTypeId,
                    UserTypeName = u.UserType.Name
                })
                .FirstOrDefaultAsync();
        }

        public async Task<UserDto> CreateAsync(CreateUserRequest request)
        {
            var entity = new Users
            {
                Id = Guid.NewGuid(),
                FullName = request.FullName,
                Email = request.Email,
                Password = request.Password,  
                UserTypeId = request.UserTypeId,
                IsActive = true,
                IsDeleted = false
            };

            await _db.AddAsync(entity);

            return await GetByIdAsync(entity.Id)
                   ?? throw new Exception("Create user failed.");
        }

        public async Task<UserDto> UpdateAsync(Guid id, UpdateUserRequest request)
        {
            var user = await _db.FirstOrDefaultAsync<Users>(x => x.Id == id);

            if (user == null)
                throw new Exception("User not found");

            user.FullName = request.FullName;
            user.Email = request.Email;
            user.IsActive = request.IsActive;
            user.UserTypeId = request.UserTypeId;

            await _db.UpdateAsync(user);

            return await GetByIdAsync(id)
                   ?? throw new Exception("Update user failed.");
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var user = await _db.FirstOrDefaultAsync<Users>(x => x.Id == id);
            if (user == null)
                return false;

            user.IsDeleted = true;

            await _db.UpdateAsync(user);
            return true;
        }

        public async Task<UserDto> GetByUserNameAsync(string userName)
        {
            var user = await _db.FirstOrDefaultAsync<Users>(x => x.Code == userName);
            if (user == null)
                throw new Exception("User not found");
            var result = new UserDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                IsActive = user.IsActive,
                UserTypeId = user.UserTypeId,
                UserTypeName = (await _db.FirstOrDefaultAsync<UserType>(x => x.Id == user.UserTypeId))?.Name ?? string.Empty
            };
            return result;
        }
    }
}
