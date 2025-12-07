using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using UNIVERSITY.EDUCATION.PLATFORMS.Application.Models.Users;

namespace UNIVERSITY.EDUCATION.PLATFORMS.Common
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private const string MessageErrorUnauthorized = "Phiên đăng nhập hết hạn";
        private const string ErrorCodeUnauthorized = "error_unauthorized";

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // ==== Lấy account an toàn ====
            var account = context.HttpContext.Items["Account"] as AuthenticationResponse;

            if (account == null)
            {
                SetUnauthorized(context, MessageErrorUnauthorized, ErrorCodeUnauthorized);
                return;
            }

            // ==== Check lockout ====
            if (account.LockoutEnabled == true)
            {
                SetUnauthorized(context, "Tài khoản đã bị khóa", "error_deactivate_account");
                return;
            }

            // ==== Lấy DeviceId an toàn ====
            var deviceId = context.HttpContext.Items["DeviceId"] as string;

            if (!string.IsNullOrWhiteSpace(account.DeviceId) &&
                !string.IsNullOrWhiteSpace(deviceId) &&
                account.DeviceId != deviceId)
            {
                SetUnauthorized(context, MessageErrorUnauthorized, ErrorCodeUnauthorized);
                return;
            }
        }

        private void SetUnauthorized(AuthorizationFilterContext context, string message, string errorCode)
        {
            context.Result = new JsonResult(new
            {
                succeeded = false,
                message = message,
                errorCode = errorCode
            })
            {
                StatusCode = StatusCodes.Status401Unauthorized
            };
        }
    }
}
