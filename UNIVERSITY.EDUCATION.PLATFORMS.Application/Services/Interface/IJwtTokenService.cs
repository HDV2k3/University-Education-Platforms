using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using UNIVERSITY.EDUCATION.PLATFORMS.Application.Models.Users;

namespace UNIVERSITY.EDUCATION.PLATFORMS.Application.Services.Interface
{
    public interface IJwtTokenService
    {
        /// <summary>
        /// Generate Access Token (JWT)
        /// </summary>
        Task<string> GenerateAccessTokenAsync(UserDetailResponse user);

        /// <summary>
        /// Generate Refresh Token (random string)
        /// </summary>
        string GenerateRefreshToken();

        /// <summary>
        /// Validate JWT token signature & expiration
        /// </summary>
        bool ValidateToken(string token);

        /// <summary>
        /// Extract ClaimsPrincipal from token (even expired)
        /// Useful for Refresh Token flow
        /// </summary>
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);

        /// <summary>
        /// Parse token to JwtSecurityToken
        /// </summary>
        JwtSecurityToken? ReadToken(string token);

        /// <summary>
        /// Get expiration timestamp of the token
        /// </summary>
        DateTime GetExpirationDate(JwtSecurityToken token);
    }
}
