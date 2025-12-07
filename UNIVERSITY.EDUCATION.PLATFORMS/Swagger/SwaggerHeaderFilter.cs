using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;
using UNIVERSITY.EDUCATION.PLATFORMS.Common;

namespace UNIVERSITY.EDUCATION.PLATFORMS.Swagger
{
    public class SwaggerHeaderFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            //  Nếu action có [AllowAnonymous] → KHÔNG thêm Authorization
            var allowAnonymous = context.MethodInfo
                .GetCustomAttributes(true)
                .OfType<Microsoft.AspNetCore.Authorization.AllowAnonymousAttribute>()
                .Any();

            if (allowAnonymous)
                return;

            //  Nếu controller hoặc action không có [Authorize] → KHÔNG thêm Authorization
            var hasAuthorize =
                context.MethodInfo.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any() ||
                context.MethodInfo.DeclaringType!.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any();

            if (!hasAuthorize)
                return;

            //  Chỉ những API có Authorize mới thêm header AuthorizationSwagger
            operation.Parameters ??= new List<IOpenApiParameter>();

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "AuthorizationSwagger",
                In = ParameterLocation.Header,
                Required = true,
                Schema = new OpenApiSchema { Type = JsonSchemaType.String }
            });
        }
    }
}
