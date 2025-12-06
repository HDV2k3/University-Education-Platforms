

namespace UNIVERSITY.EDUCATION.PLATFORMS.Application.Models.Login
{
    public class RefreshTokenResponse
    {
        public string AccessToken { get; set; } = string.Empty;

        public string RefreshToken { get; set; } = string.Empty;

        public DateTime Expiration { get; set; }

    }
}
