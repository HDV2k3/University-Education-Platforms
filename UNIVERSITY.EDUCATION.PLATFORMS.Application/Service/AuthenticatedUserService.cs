using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace UNIVERSITY.EDUCATION.PLATFORMS.Application.Service
{
    public class AuthenticatedUserService : IAuthenticatedUserService
    {
        private readonly IHttpContextAccessor _http;
        private ClaimsPrincipal? User => _http.HttpContext?.User;

        public AuthenticatedUserService(IHttpContextAccessor http)
        {
            _http = http ?? throw new ArgumentNullException(nameof(http));
        }

        private string? GetClaim(string type)
            => User?.FindFirst(type)?.Value;

        private List<string> GetClaims(string type)
            => User?.Claims.Where(c => c.Type == type).Select(c => c.Value).ToList() ?? new();



        public string Email
            => GetClaim(ClaimTypes.Email) ?? string.Empty;

        public string FullName
            => GetClaim("full_name") ?? string.Empty;

        public List<string> Roles
            => GetClaims(ClaimTypes.Role);

        public List<string> Permissions
            => GetClaims("permission");

        public string IpAddress
            => _http.HttpContext?.Connection.RemoteIpAddress?.ToString() ?? string.Empty;

        public string Platform
            => GetClaim("platform") ?? string.Empty;

        public string DeviceId
            => GetClaim("device_id") ?? string.Empty;


        public Guid UserId
           => Guid.TryParse(GetClaim(ClaimTypes.NameIdentifier), out var id) ? id : Guid.Empty;

        public bool HavePermission(string func)
            => Permissions.Contains(func);

        public bool HaveAnyPermission(List<string> funcs)
            => funcs.Any(f => Permissions.Contains(f));

        public bool HaveAllPermission(List<string> funcs)
            => funcs.All(f => Permissions.Contains(f));
    }
}
