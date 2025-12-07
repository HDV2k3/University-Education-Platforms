using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UNIVERSITY.EDUCATION.PLATFORMS.SERVICE.Implementation;
using UNIVERSITY.EDUCATION.PLATFORMS.SERVICE.Interface;
using UNIVERSITY.EDUCATION.PLATFORMS.SERVICE.Mappings;

namespace UNIVERSITY.EDUCATION.PLATFORMS.SERVICE.Extensions
{
    public static class DependencyInjection
    {
        public static void ServiceDescriptors(this IServiceCollection services, IConfiguration configuration)
        {
            // Register AutoMapper 
            services.AddAutoMapper(typeof(GeneralProfile));

            // Register Domain Services
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRefreshTokenService, RefreshTokenService>();
            services.AddScoped<ILoginService, LoginService>();
            services.AddScoped<IJwtTokenService, JwtTokenService>();
            services.AddScoped<IBCryptEncryptionService, BCryptEncryptionService>();


        }
    }
}
