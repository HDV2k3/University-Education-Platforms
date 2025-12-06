//using System.Security.Claims;
//using UNIVERSITY.EDUCATION.PLATFORMS.Application.Models.Login;
//using UNIVERSITY.EDUCATION.PLATFORMS.Application.Models.Users;
//using UNIVERSITY.EDUCATION.PLATFORMS.Application.Services.Interface;

//namespace UNIVERSITY.EDUCATION.PLATFORMS.Application.Services.Login
//{
//    public class LoginService : ILoginService
//    {
//        private readonly IUserService _userService;
//        private readonly IBCryptEncryptionService _encryptionService;
//        private readonly IJwtTokenService _jwtTokenService;

//        public LoginService(
//            IUserService userService,
//            IBCryptEncryptionService encryptionService,
//            IJwtTokenService jwtTokenService)

//        {
//            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
//            _encryptionService = encryptionService ?? throw new ArgumentNullException(nameof(encryptionService));
//            _jwtTokenService = jwtTokenService ?? throw new ArgumentNullException(nameof(jwtTokenService));
//        }

//        // ======================================================================
//        // 1. LOGIN FLOW
//        // ======================================================================
//        public async Task<LoginResponse> LoginAsync(LoginRequest request)
//        {
//            if (request == null)
//                throw new ArgumentNullException(nameof(request));

//            // 1) Lấy user  
//            var user = await _userService.GetByUserNameAsync(request.Username);
//            if (user == null)
//                throw new Exception("User not found.");

//            if (!user.IsActive)
//                throw new Exception("User is deactivated.");

//            // 2) Verify password bằng BCrypt
//            bool passwordValid = _encryptionService.VerifyPassword(request.Password, user.HashedPassword);
//            if (!passwordValid)
//                throw new Exception("Invalid password.");

//            // 3) Mapping UserDetailResponse
//            var userDetail = new UserDetailResponse
//            {
//                Id = user.Id,
//                FullName = user.FullName,
//                Email = user.Email,
//                PhoneNumber = user.PhoneNumber,
//                Code = user.Code,
//                Address = user.Address,
//                UserTypeId = user.UserTypeId,
//                UserTypeName = user.UserTypeName,
//                Status = user.Status,
//                IsActive = user.IsActive,
//                PermissionCodes = user.PermissionCodes?.ToList() ?? new(),
//                RoleNames = user.RoleNames?.ToList() ?? new()
//            };

//            // 4) Generate access token
//            string accessToken = await _jwtTokenService.GenerateAccessTokenAsync(userDetail);

//            // 5) Generate refresh token
//            string refreshToken = _jwtTokenService.GenerateRefreshToken();

//            // 6) Lưu refresh token
//            await _refreshTokenRepository.SaveTokenAsync(new RefreshTokenEntity
//            {
//                UserId = user.Id,
//                RefreshToken = refreshToken,
//                ExpirationDate = DateTime.UtcNow.AddDays(7),
//                CreatedDate = DateTime.UtcNow
//            });

//            // 7) Return response
//            return new LoginResponse
//            {
//                AccessToken = accessToken,
//                RefreshToken = refreshToken,
//                User = userDetail,
//                Succeeded = true,
//                Message = "Login success."
//            };
//        }

//        // ======================================================================
//        // 2. REFRESH TOKEN FLOW
//        // ======================================================================
//        public async Task<RefreshTokenResponse> RefreshTokenAsync(RefreshTokenRequest request)
//        {
//            if (request == null)
//                throw new ArgumentNullException(nameof(request));

//            // 1) Lấy principal từ access token đã expired
//            var principal = _jwtTokenService.GetPrincipalFromExpiredToken(request.AccessToken);
//            if (principal == null)
//                throw new Exception("Invalid access token.");

//            string userId = principal.FindFirstValue(ClaimTypes.NameIdentifier)
//                                ?? throw new Exception("Invalid token payload.");

//            // 2) Kiểm tra refresh token trong DB
//            var savedToken = await _refreshTokenRepository.GetByTokenAsync(request.RefreshToken);
//            if (savedToken == null || savedToken.UserId.ToString() != userId)
//                throw new Exception("Invalid refresh token.");

//            if (savedToken.ExpirationDate < DateTime.UtcNow)
//                throw new Exception("Refresh token expired.");

//            // 3) Lấy lại user
//            var user = await _userRepository.GetUserByIdAsync(Guid.Parse(userId));
//            if (user == null)
//                throw new Exception("User not found.");

//            // 4) Tạo token mới
//            var userDetail = new UserDetailResponse
//            {
//                Id = user.Id,
//                FullName = user.FullName,
//                Email = user.Email,
//                RoleNames = user.RoleNames.ToList(),
//                PermissionCodes = user.PermissionCodes.ToList()
//            };

//            string newAccessToken = await _jwtTokenService.GenerateAccessTokenAsync(userDetail);
//            string newRefreshToken = _jwtTokenService.GenerateRefreshToken();

//            // 5) Update refresh token trong DB
//            savedToken.RefreshToken = newRefreshToken;
//            savedToken.ExpirationDate = DateTime.UtcNow.AddDays(7);

//            await _refreshTokenRepository.UpdateAsync(savedToken);

//            return new RefreshTokenResponse
//            {
//                AccessToken = newAccessToken,
//                RefreshToken = newRefreshToken,
//                Succeeded = true
//            };
//        }

//        // ======================================================================
//        // 3. LOGOUT
//        // ======================================================================
//        public async Task<bool> LogoutAsync(string userId)
//        {
//            await _refreshTokenRepository.DeleteByUserIdAsync(userId);
//            return true;
//        }
//    }
//}
