using Microsoft.OpenApi;
using UNIVERSITY.EDUCATION.PLATFORMS.Application.Extensions;
using UNIVERSITY.EDUCATION.PLATFORMS.Infrastructure.Extensions;
using UNIVERSITY.EDUCATION.PLATFORMS.Swagger;

var builder = WebApplication.CreateBuilder(args);

// CORS
builder.Services.AddCors(o =>
{
    o.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

// Register Layers
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// Controllers
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "University Education Platforms v1.0",
        Version = "v1.0"
    });
    //c.OperationFilter<SwaggerHeaderFilter>();
});

builder.AddServiceDefaults();

var app = builder.Build();

app.MapDefaultEndpoints();

// Swagger Middleware
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "UEP API v1");
    c.RoutePrefix = string.Empty; 
});

// Routing
app.UseAuthorization();
app.MapControllers();

// Test endpoints
app.MapGet("/", () => "UEP API is running (.NET 9)");
app.MapGet("/health", () =>
    Results.Ok(new { status = "Healthy", timestamp = DateTime.UtcNow }));

app.Run();
