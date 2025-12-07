using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi;
using UNIVERSITY.EDUCATION.PLATFORMS.Application.Models.Settings;
using UNIVERSITY.EDUCATION.PLATFORMS.Application.Service;
using UNIVERSITY.EDUCATION.PLATFORMS.Domain.Common.Infrastructure.Persistence.Seed;
using UNIVERSITY.EDUCATION.PLATFORMS.Domain.Extensions;
using UNIVERSITY.EDUCATION.PLATFORMS.Infrastructure.Persistence;
using UNIVERSITY.EDUCATION.PLATFORMS.Middleware;
using UNIVERSITY.EDUCATION.PLATFORMS.Service.Extensions;
using UNIVERSITY.EDUCATION.PLATFORMS.Swagger;

var builder = WebApplication.CreateBuilder(args);

// ======================================================
// Register Application Layers
// ======================================================
builder.Services.ServiceDescriptors(builder.Configuration);
builder.Services.DatabaseDescriptors(builder.Configuration);

builder.Services.Configure<JWTSettings>(
  builder.Configuration.GetSection("JWTSettings"));
// Authenticated User Provider
builder.Services.AddScoped<IAuthenticatedUserService, AuthenticatedUserService>();

// ======================================================
// API Versioning
// ======================================================
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
});

// Required for Swagger Versioning
builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

// Controllers
builder.Services.AddControllers();

// ======================================================
// Swagger
// ======================================================
builder.Services.AddSwaggerGen(c =>
{
    //c.SwaggerDoc("v1", new OpenApiInfo { Title = "UEP v1.0", Version = "v1.0" });
    c.OperationFilter<SwaggerHeaderFilter>();
});

// Swagger + API versioning integration
builder.Services.ConfigureOptions<SwaggerConfig>();

builder.AddServiceDefaults();

var app = builder.Build();

app.MapDefaultEndpoints();

// ======================================================
// Swagger Middleware
// ======================================================
var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    foreach (var desc in provider.ApiVersionDescriptions)
    {
        options.SwaggerEndpoint(
            $"/swagger/{desc.GroupName}/swagger.json",
            $"UEP API {desc.GroupName.ToUpper()}");
    }

    options.RoutePrefix = string.Empty;
});

// ======================================================
// Middlewares
// ======================================================
app.UseStaticFiles();

app.UseRouting();

app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(_ => true)
    .AllowCredentials());

app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Health Check
app.MapGet("/health", () =>
    Results.Ok(new { status = "Healthy", timestamp = DateTime.UtcNow }));

app.Run();
