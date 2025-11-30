using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using UNIVERSITY.EDUCATION.PLATFORMS.Application.Models.Users;

namespace UNIVERSITY.EDUCATION.PLATFORMS.Common
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string MessageErrorUnauthorized = "Phiên đăng nhập hết hạn";
        private readonly string ErrorCodeUnauthorized = "error_unauthorized";
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var account = (AuthenticationResponse)context.HttpContext.Items["Account"];
            if (account == null)
            {
                // not logged in
                context.Result = new JsonResult(new
                {
                    succeeded = false,
                    message = MessageErrorUnauthorized,
                    errorCode = ErrorCodeUnauthorized
                })
                { StatusCode = StatusCodes.Status401Unauthorized };
            }
            else
            {
                if (account.LockoutEnabled == false)
                    context.Result = new JsonResult(new
                    {
                        succeeded = false,
                        message = "Tài khoản đã bị khóa",
                        errorCode = "error_deactivate_account"
                    })
                    { StatusCode = StatusCodes.Status401Unauthorized };
                else
                {
                    var deviceId = (string)context.HttpContext.Items["DeviceId"];
                    if (account.DeviceId != deviceId)
                    {
                        context.Result = new JsonResult(new
                        {
                            succeeded = false,
                            message = MessageErrorUnauthorized,
                            errorCode = ErrorCodeUnauthorized
                        })
                        { StatusCode = StatusCodes.Status401Unauthorized };
                    }
                }
            }
        }
    }
}
