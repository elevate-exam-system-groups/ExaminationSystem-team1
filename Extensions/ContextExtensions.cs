using ExaminationSystem.Domain.Data;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Diagnostics;

namespace ExaminationSystem.Extensions
{
    public static class DbContextExtensions
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<Domain.Data.IUnitOfWork>(opt =>
                opt.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
                   .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                   .LogTo(log => Debug.WriteLine(log), LogLevel.Information)
                   .EnableSensitiveDataLogging()
            );

            services.AddScoped<IDbConnection>(_ =>
            new SqlConnection(configuration.GetConnectionString("DefaultConnection")));

            return services;
        }
    }
}
