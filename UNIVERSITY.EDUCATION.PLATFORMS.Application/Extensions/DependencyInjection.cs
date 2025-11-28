using Microsoft.Extensions.DependencyInjection;

namespace UNIVERSITY.EDUCATION.PLATFORMS.Application.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            return services;
        }
    }
}
