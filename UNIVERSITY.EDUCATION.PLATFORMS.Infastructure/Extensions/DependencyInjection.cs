using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UNIVERSITY.EDUCATION.PLATFORMS.Infrastructure.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // connection string
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            // DbContext
            //services.AddDbContext<AppDbContext>(options =>
            //    options.UseSqlServer(connectionString));

            // Repository 
            // services.AddScoped<IStudentRepository, StudentRepository>();
            // services.AddScoped<ICourseRepository, CourseRepository>();

            return services;
        }
    }
}
