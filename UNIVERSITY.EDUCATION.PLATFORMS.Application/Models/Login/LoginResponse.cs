using UNIVERSITY.EDUCATION.PLATFORMS.Application.Models.Users;

namespace UNIVERSITY.EDUCATION.PLATFORMS.Application.Models.Login
{
    public class LoginResponse
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public UserDetailResponse User { get; set; }
    }

}
