using UNIVERSITY.EDUCATION.PLATFORMS.Application.Services.Interface;

namespace UNIVERSITY.EDUCATION.PLATFORMS.Infrastructure.Services.Authenticated
{
    public class AuthenticatedService : IAuthenticatedUserService
    {
        public Guid UserId => throw new NotImplementedException();

        public string Email => throw new NotImplementedException();

        public List<string> Roles => throw new NotImplementedException();

        public string IpAddress => throw new NotImplementedException();

        public string Platform => throw new NotImplementedException();

        public string FullName => throw new NotImplementedException();

        public string DeviceId => throw new NotImplementedException();

        public List<string> Permissions => throw new NotImplementedException();

        public bool HaveAllPermission(List<string> funcs)
        {
            throw new NotImplementedException();
        }

        public bool HaveAnyPermission(List<string> funcs)
        {
            throw new NotImplementedException();
        }

        public bool HavePermission(string func)
        {
            throw new NotImplementedException();
        }
    }
}
