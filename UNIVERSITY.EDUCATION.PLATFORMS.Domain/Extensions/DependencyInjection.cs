using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UNIVERSITY.EDUCATION.PLATFORMS.Application.Common.Infrastructure;
using UNIVERSITY.EDUCATION.PLATFORMS.Application.Service;
using UNIVERSITY.EDUCATION.PLATFORMS.Infrastructure.Persistence;

namespace UNIVERSITY.EDUCATION.PLATFORMS.Domain.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection DatabaseDescriptors(this IServiceCollection services, IConfiguration configuration)
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
            services.AddHttpContextAccessor();
            services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<UEPContext>());
            services.AddScoped<IDatabaseService<UEPContext>, DatabaseService<UEPContext>>();
         
            return services;
        }
    }
}
