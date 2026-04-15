using ExaminationSystem.Domain.Data;
using System.Diagnostics;

namespace ExaminationSystem.Extensions
{
    public static class DbContextExtensions
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<Context>(opt =>
                opt.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
                   .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                   .LogTo(log => Debug.WriteLine(log), LogLevel.Information)
                   .EnableSensitiveDataLogging()
            );
            return services;
        }
    }
}
