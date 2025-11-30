using Microsoft.Extensions.DependencyInjection;
using UNIVERSITY.EDUCATION.PLATFORMS.Application.Services.Impl;
using UNIVERSITY.EDUCATION.PLATFORMS.Application.Services.Interface;

namespace UNIVERSITY.EDUCATION.PLATFORMS.Application.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();

            return services;
        }
    }
}
