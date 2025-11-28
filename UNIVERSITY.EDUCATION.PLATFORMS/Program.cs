using Microsoft.OpenApi;
using UNIVERSITY.EDUCATION.PLATFORMS.Application.Extensions;
using UNIVERSITY.EDUCATION.PLATFORMS.Infrastructure.Extensions;
using UNIVERSITY.EDUCATION.PLATFORMS.Swagger;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "University Education Platforms v1.0", Version = "v1.0" });
    c.OperationFilter<SwaggerHeaderFilter>();
});
builder.AddServiceDefaults();
var app = builder.Build();

app.MapDefaultEndpoints();

app.MapGet("/", () => "Hello World!");
app.Run();
