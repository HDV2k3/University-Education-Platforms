using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UNIVERSITY.EDUCATION.PLATFORMS.Application.Models.Login;
using UNIVERSITY.EDUCATION.PLATFORMS.Application.Service;
using UNIVERSITY.EDUCATION.PLATFORMS.Service.Interface;
using AuthorizeAttribute = UNIVERSITY.EDUCATION.PLATFORMS.Common.AuthorizeAttribute;

namespace UNIVERSITY.EDUCATION.PLATFORMS.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [ControllerName("authentication")]
    [Route("api/v{version:apiVersion}/authentication")]
    public class AuthenticationController : ControllerBase
    {
        private readonly ILoginService _loginService;
        private readonly IAuthenticatedUserService _authenticatedUserService;

        public AuthenticationController(
            ILoginService loginService,
            IAuthenticatedUserService authenticatedUserService)
        {
            _loginService = loginService;
            _authenticatedUserService = authenticatedUserService;
        }

        // ===============================
        // LOGIN
        // ===============================
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var result = await _loginService.LoginAsync(request);
            return Ok(result);
        }

        // ===============================
        // REFRESH TOKEN
        // ===============================
        [HttpPost("refresh-token")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken(RefreshTokenRequest request)
        {
            var result = await _loginService.RefreshTokenAsync(request);
            return Ok(result);
        }

        // ===============================
        // LOGOUT
        // ===============================
        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var userId = _authenticatedUserService.UserId.ToString();
            var result = await _loginService.LogoutAsync(userId);
            return Ok(result);
        }

        // ===============================
        // ME
        // ===============================
        [Authorize]
        [HttpGet("me")]
        public IActionResult Me()
        {
            return Ok(new
            {
                id = _authenticatedUserService.UserId,
                email = _authenticatedUserService.Email,
                fullName = _authenticatedUserService.FullName,
                roles = _authenticatedUserService.Roles,
                permissions = _authenticatedUserService.Permissions
            });
        }
    }
}
