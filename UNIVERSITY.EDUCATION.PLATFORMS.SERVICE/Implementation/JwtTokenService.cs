using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using UNIVERSITY.EDUCATION.PLATFORMS.Application.Models.Settings;
using UNIVERSITY.EDUCATION.PLATFORMS.Application.Models.Users;
using UNIVERSITY.EDUCATION.PLATFORMS.Service.Interface;
namespace UNIVERSITY.EDUCATION.PLATFORMS.Service.Implementation
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly JWTSettings _jwtSettings;
        private readonly JwtSecurityTokenHandler _tokenHandler = new();

        public JwtTokenService(IOptions<JWTSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value ?? throw new ArgumentNullException(nameof(jwtSettings));
        }


        public async Task<string> GenerateAccessTokenAsync(UserDetailResponse user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()),
            };

            // Email
            if (!string.IsNullOrWhiteSpace(user.Email))
                claims.Add(new Claim(ClaimTypes.Email, user.Email));

            // Full name
            if (!string.IsNullOrWhiteSpace(user.FullName))
                claims.Add(new Claim("full_name", user.FullName));

            // Roles
            if (user.RoleNames != null)
            {
                foreach (var role in user.RoleNames)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }
            }

            // Permissions
            if (user.PermissionCodes != null)
            {
                foreach (var permission in user.PermissionCodes)
                {
                    claims.Add(new Claim("permission", permission));
                }
            }

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes),
                signingCredentials: credentials
            );

            return await Task.FromResult(_tokenHandler.WriteToken(token));
        }


        public string GenerateRefreshToken()
        {
            var buffer = new byte[64];
            RandomNumberGenerator.Fill(buffer);
            return Convert.ToBase64String(buffer);
        }

        public bool ValidateToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return false;

            try
            {
                _tokenHandler.ValidateToken(token, GetValidationParameters(true), out _);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
        {
            try
            {
                var principal = _tokenHandler.ValidateToken(token, GetValidationParameters(false), out var validatedToken);

                if (validatedToken is not JwtSecurityToken jwtToken)
                    return null;

                return principal;
            }
            catch
            {
                return null;
            }
        }


        public JwtSecurityToken? ReadToken(string token)
        {
            return _tokenHandler.ReadToken(token) as JwtSecurityToken;
        }

        public DateTime GetExpirationDate(JwtSecurityToken token)
        {
            return token.ValidTo;
        }

        private TokenValidationParameters GetValidationParameters(bool validateLifetime)
        {
            return new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = _jwtSettings.Issuer,
                ValidAudience = _jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key)),
                ValidateIssuerSigningKey = true,
                ValidateLifetime = validateLifetime,
                ClockSkew = TimeSpan.Zero
            };
        }
    }
}
