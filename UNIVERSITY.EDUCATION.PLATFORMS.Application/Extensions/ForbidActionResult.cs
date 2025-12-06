using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace UNIVERSITY.EDUCATION.PLATFORMS.Application.Extensions
{
    public class ForbidActionResult : ObjectResult
    {
        private const string DefaultMessage = "User is not allowed to enter this page.";

        public ForbidActionResult(int statusCode = (int)HttpStatusCode.Forbidden, string? errorMessage = null)
            : base(errorMessage ?? DefaultMessage)
        {
            StatusCode = statusCode;
        }

        public ForbidActionResult(string? errorMessage = null)
            : base(errorMessage ?? DefaultMessage)
        {
            StatusCode = (int)HttpStatusCode.Forbidden;
        }

        public override async Task ExecuteResultAsync(ActionContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            await base.ExecuteResultAsync(context);
        }
    }
}
