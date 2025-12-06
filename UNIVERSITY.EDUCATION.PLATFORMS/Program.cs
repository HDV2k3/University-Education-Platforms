using Microsoft.OpenApi;
using UNIVERSITY.EDUCATION.PLATFORMS.Extensions;
using UNIVERSITY.EDUCATION.PLATFORMS.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Register Layers
builder.Services.ServiceDescriptors(builder.Configuration);

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

app.UseStaticFiles();

app.UseRouting();

app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true) // allow any origin
    .AllowCredentials());

// global error handler
app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


app.MapGet("/health", () =>
    Results.Ok(new { status = "Healthy", timestamp = DateTime.UtcNow }));

app.Run();
