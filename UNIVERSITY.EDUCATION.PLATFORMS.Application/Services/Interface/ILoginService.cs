using UNIVERSITY.EDUCATION.PLATFORMS.Application.Models.Login;

namespace UNIVERSITY.EDUCATION.PLATFORMS.Application.Services.Interface
{
    public interface ILoginService
    {
        Task<LoginResponse> LoginAsync(LoginRequest request);
        Task<RefreshTokenResponse> RefreshTokenAsync(RefreshTokenRequest request);
        Task<bool> LogoutAsync(string userId);
    }
}
