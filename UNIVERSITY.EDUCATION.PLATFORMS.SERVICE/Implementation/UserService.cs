using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UNIVERSITY.EDUCATION.PLATFORMS.Application.Common.Infrastructure;
using UNIVERSITY.EDUCATION.PLATFORMS.Application.Models.Users;
using UNIVERSITY.EDUCATION.PLATFORMS.Application.Service;
using UNIVERSITY.EDUCATION.PLATFORMS.Application.Services.GenericService;
using UNIVERSITY.EDUCATION.PLATFORMS.Domain.Entities;
using UNIVERSITY.EDUCATION.PLATFORMS.Infrastructure.Persistence;
using UNIVERSITY.EDUCATION.PLATFORMS.Service.Interface;

namespace UNIVERSITY.EDUCATION.PLATFORMS.Service.Implementation
{
    public class UserService: AppBaseService<Users, Guid, CreateUserRequest, UpdateUserRequest, UserDto>, IUserService
    {

        public UserService(IMapper mapper,IDatabaseService<UEPContext> unitOfWork,IAuthenticatedUserService authenticatedUserService): base(mapper, unitOfWork, authenticatedUserService)
        {
        }

        public async Task<UserDto> GetByUserNameAsync(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
                throw new ArgumentNullException(nameof(userName));

            var context = _unitOfWork.GetContext();   

            var user = await context.ToQueryable<Users>()
                .Include(x => x.UserType)
                .FirstOrDefaultAsync(x => x.Code == userName && !x.IsDeleted);

            if (user == null)
                throw new Exception("User not found.");

            var dto = mapper.Map<UserDto>(user);
            dto.UserTypeName = user.UserType?.Name ?? string.Empty;

            return dto;
        }



    }
}
