using Microsoft.EntityFrameworkCore;
using UNIVERSITY.EDUCATION.PLATFORMS.Application.Services.Interface;
using UNIVERSITY.EDUCATION.PLATFORMS.Application.Services.User;
using UNIVERSITY.EDUCATION.PLATFORMS.Infrastructure.Persistence;
using UNIVERSITY.EDUCATION.PLATFORMS.Infrastructure.Persistence.Configurations;
using UNIVERSITY.EDUCATION.PLATFORMS.Infrastructure.Services.Authenticated;
using UNIVERSITY.EDUCATION.PLATFORMS.Infrastructure.Services.Encryption;
using UNIVERSITY.EDUCATION.PLATFORMS.Infrastructure.Services.Jwt;

namespace UNIVERSITY.EDUCATION.PLATFORMS.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection ServiceDescriptors(this IServiceCollection services, IConfiguration configuration)
        {
            var envConnection = Environment.GetEnvironmentVariable("DB_CONNECTION");
            var connectionString = envConnection ??
                                   configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrWhiteSpace(connectionString))
                throw new Exception("Connection string is missing. Please set DB_CONNECTION environment variable.");

            services.AddDbContext<UEPContext>(options =>
            {
                options.UseSqlServer(connectionString,
                    sqlOptions => sqlOptions.MigrationsAssembly(
                        typeof(UEPContext).Assembly.FullName));
            });

            services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<UEPContext>());
            services.AddScoped<IDatabaseService<UEPContext>, DatabaseService<UEPContext>>();
            services.AddScoped<IAuthenticatedUserService, AuthenticatedService>();
            services.AddScoped<IJwtTokenService, JwtTokenService>();
            services.AddScoped<IBCryptEncryptionService, BCryptEncryptionService>();
            services.AddScoped<IUserService, UserService>();

            return services;
        }
    }
}
