
using Autofac;
using Autofac.Extensions.DependencyInjection;
using ExaminationSystem.Configurations;
using ExaminationSystem.Controllers.Shared.Middlewares;
using ExaminationSystem.Domain.Data;
using ExaminationSystem.Features.DiplomaFeatures.GetAllDiplomas;

namespace ExaminationSystem
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Add services to the container.

            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                });
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDatabase(builder.Configuration);

            builder.Services.AddMediatR(cfg =>
                  cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
            builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
                containerBuilder.RegisterModule(new AutofacModule()));

            builder.Services.AddIdentityServices();

            builder.Services.AddJwtServices(builder.Configuration);

            #endregion

            var app = builder.Build();

            #region UpdateDatabase

            using var Scope = app.Services.CreateScope(); //Group Of services That has object lifetime scoped
            var Services = Scope.ServiceProvider; // Service itself
            var LoggerFactory = Services.GetRequiredService<ILoggerFactory>();
            try
            {
                //Ask CLR For Creating An Instance From Context Exiplicitly
                var context = Services.GetRequiredService<Context>();

                await context.Database.MigrateAsync();

                var userManager = Services.GetRequiredService<UserManager<User>>();
                var roleManager = Services.GetRequiredService<RoleManager<IdentityRole>>();
                await ContextSeed.SeedAsync(context, userManager, roleManager); // Seed Data
            }
            catch (Exception ex)
            {
                var logger = LoggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "An error occurred during migration");
                // throw;
            }

            #endregion

            #region  Configure the HTTP request pipeline

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseMiddleware<GlobalErrorHandlerMiddelware>();
            // app.UseMiddleware<TransactionMiddleware>();

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMiddleware<UpdateLastActivityMiddleware>();
            app.MapControllers();
            app.MapGetAllDiplomasEndpoint();
            #endregion


            app.Run();
        }
    }
}
