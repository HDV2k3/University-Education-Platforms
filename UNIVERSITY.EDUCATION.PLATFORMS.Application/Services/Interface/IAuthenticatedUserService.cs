namespace UNIVERSITY.EDUCATION.PLATFORMS.Application.Services.Interface
{
    public interface IAuthenticatedUserService
    {
        Guid UserId { get; }
        string Email { get; }
        List<string> Roles { get; }
        string IpAddress { get; }
        string Platform { get; }
        string FullName { get; }
        string DeviceId { get; }
        List<string> Permissions { get; }

        bool HaveAllPermission(List<string> funcs);

        bool HaveAnyPermission(List<string> funcs);

        bool HavePermission(string func);
    }
}
