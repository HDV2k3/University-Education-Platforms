using Microsoft.OpenApi;
using UNIVERSITY.EDUCATION.PLATFORMS.Swagger;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();

builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "University Education Platforms v1.0", Version = "v1.0" });
    c.OperationFilter<SwaggerHeaderFilter>();
});
// 1. ServiceDefaults
builder.AddServiceDefaults();
// 2. Application layer

var app = builder.Build();

app.MapDefaultEndpoints();

app.MapGet("/", () => "Hello World!");
app.Run();
