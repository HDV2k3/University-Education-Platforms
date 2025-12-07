using UNIVERSITY.EDUCATION.PLATFORMS.Application.Models.BaseResponse;
using UNIVERSITY.EDUCATION.PLATFORMS.Application.Models.Login;

namespace UNIVERSITY.EDUCATION.PLATFORMS.SERVICE.Interface
{
    public interface ILoginService
    {
        Task<Response<LoginResponse>> LoginAsync(LoginRequest request);
        Task<Response<LoginResponse>> RefreshTokenAsync(RefreshTokenRequest request);
        Task<bool> LogoutAsync(string userId);
    }
}
