using System.Net;
using System.Text.Json;
using UNIVERSITY.EDUCATION.PLATFORMS.Application.Extensions;
using UNIVERSITY.EDUCATION.PLATFORMS.Application.Models.BaseResponse;

namespace UNIVERSITY.EDUCATION.PLATFORMS.Middleware
{
    public class ErrorHandlerMiddleware
    {

        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                var responseModel = new Response<ObjectResponse>
                {
                    Succeeded = false,
                    Message = error?.Message ?? "Error",
                };
                switch (error)
                {
                    case AppException e:
                        // application error
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    case KeyNotFoundException e:
                        // not found error
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    default:
                        // unhandled error
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }


                var result = JsonSerializer.Serialize(responseModel);
                await response.WriteAsync(result);
            }
        }
    }
}
