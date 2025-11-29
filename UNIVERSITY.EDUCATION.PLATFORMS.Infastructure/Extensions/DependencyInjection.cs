using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UNIVERSITY.EDUCATION.PLATFORMS.Application.Services.Interface;
using UNIVERSITY.EDUCATION.PLATFORMS.Infrastructure.Persistence;
using UNIVERSITY.EDUCATION.PLATFORMS.Infrastructure.Persistence.Configurations;
using UNIVERSITY.EDUCATION.PLATFORMS.Infrastructure.Services.Impl;

namespace UNIVERSITY.EDUCATION.PLATFORMS.Infrastructure.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
            IConfiguration configuration)
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

            services.AddScoped<IAuthenticatedService, AuthenticatedService>();
            services.AddScoped<IApplicationDbContext>(provider =>
                provider.GetRequiredService<UEPContext>());
            services.AddScoped<ICommandDbContext>(provider =>
                provider.GetRequiredService<UEPContext>());
            services.AddScoped<IDatabaseService<UEPContext>, DatabaseService<UEPContext>>();

            return services;
        }

    }
}
