using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using UNIVERSITY.EDUCATION.PLATFORMS.Application.Models.Settings;
using UNIVERSITY.EDUCATION.PLATFORMS.Application.Models.Users;
using UNIVERSITY.EDUCATION.PLATFORMS.Application.Services.Interface;

namespace UNIVERSITY.EDUCATION.PLATFORMS.Infrastructure.Services.Jwt
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly JWTSettings _jwtSettings;

        public JwtTokenService(IOptions<JWTSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value ?? throw new ArgumentNullException(nameof(jwtSettings));
        }


        // ======================================================================
        // 1. Generate Access Token
        // ======================================================================
        public async Task<string> GenerateAccessTokenAsync(UserDetailResponse user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            // ===== Claims cơ bản =====
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
                new Claim("FullName", user.FullName ?? string.Empty),
                new Claim("UserTypeId", user.UserTypeId.ToString()),
                new Claim("UserTypeName", user.UserTypeName.ToString()),
                new Claim("Code", user.Code ?? string.Empty)
            };

            // ===== Roles =====
            if (user.RoleNames != null)
            {
                foreach (var role in user.RoleNames)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }
            }

            // ===== Permissions =====
            if (user.PermissionCodes != null)
            {
                foreach (var permission in user.PermissionCodes)
                {
                    claims.Add(new Claim("Permission", permission));
                }
            }

            // ===== Signing Key =====
            var keyBytes = Encoding.UTF8.GetBytes(_jwtSettings.Key);
            var signingKey = new SymmetricSecurityKey(keyBytes);
            var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            // ===== JWT Token =====
            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(60),
                signingCredentials: credentials
            );

            return await Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
        }

        // ======================================================================
        // 2. Generate Refresh Token
        // ======================================================================
        public string GenerateRefreshToken()
        {
            var buffer = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(buffer);
            return Convert.ToBase64String(buffer);
        }

        // ======================================================================
        // 3. Validate Token (expired = false)
        // ======================================================================
        public bool ValidateToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return false;

            var tokenHandler = new JwtSecurityTokenHandler();
            var keyBytes = Encoding.UTF8.GetBytes(_jwtSettings.Key);

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = _jwtSettings.Issuer,
                    ValidAudience = _jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                }, out _);

                return true;
            }
            catch
            {
                return false;
            }
        }

        // ======================================================================
        // 4. Lấy ClaimsPrincipal từ token đã hết hạn
        // ======================================================================
        public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var keyBytes = Encoding.UTF8.GetBytes(_jwtSettings.Key);

            try
            {
                var principal = tokenHandler.ValidateToken(
                    token,
                    new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidIssuer = _jwtSettings.Issuer,
                        ValidAudience = _jwtSettings.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
                        // Cho phép token hết hạn
                        ValidateLifetime = false
                    },
                    out SecurityToken securityToken
                );

                if (securityToken is JwtSecurityToken)
                    return principal;

                return null;
            }
            catch
            {
                return null;
            }
        }

        // ======================================================================
        // 5. Parse token
        // ======================================================================
        public JwtSecurityToken? ReadToken(string token)
        {
            return new JwtSecurityTokenHandler().ReadToken(token) as JwtSecurityToken;
        }

        // ======================================================================
        // 6. Get expiration
        // ======================================================================
        public DateTime GetExpirationDate(JwtSecurityToken token)
        {
            return token.ValidTo;
        }
    }
}
