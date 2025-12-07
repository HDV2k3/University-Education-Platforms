using Microsoft.AspNetCore.Mvc;
using UNIVERSITY.EDUCATION.PLATFORMS.Application.Constants;
using UNIVERSITY.EDUCATION.PLATFORMS.Application.Models.Users;
using UNIVERSITY.EDUCATION.PLATFORMS.Application.Service;
using UNIVERSITY.EDUCATION.PLATFORMS.Domain.Entities;
using UNIVERSITY.EDUCATION.PLATFORMS.Service.Interface;

namespace UNIVERSITY.EDUCATION.PLATFORMS.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [ControllerName("users")]
    [Route("api/v{version:apiVersion}/users")]
    public class UsersController
        : BaseController<Users, Guid, CreateUserRequest, UpdateUserRequest, UserDto, IUserService>
    {
        public UsersController(IUserService userService,
            IAuthenticatedUserService authenticatedUserService)
            : base(userService, authenticatedUserService)
        {
        }

        protected override string GetPolicyName => CommandCode.VIEW_ROLE.ToString();
        protected override string GetListPolicyName => CommandCode.VIEW_ROLE.ToString();
        protected override string CreatePolicyName => CommandCode.CREATE_ROLE.ToString();
        protected override string DeletePolicyName => CommandCode.DELETE_ROLE.ToString();
        protected override string UpdatePolicyName => CommandCode.UPDATE_ROLE.ToString();
        protected override string CacheKey => "USER_CACHE";
    }
}
