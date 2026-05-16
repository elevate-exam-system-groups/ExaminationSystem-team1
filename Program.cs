
using Autofac;
using Autofac.Extensions.DependencyInjection;
using ExaminationSystem.Configurations;
using ExaminationSystem.Contracts.Commands;
using ExaminationSystem.Controllers.Shared.Middlewares;
using ExaminationSystem.Domain.Data;
using ExaminationSystem.Extensions.Logging;
using ExaminationSystem.Features;
using ExaminationSystem.Features.Consumers;
using MassTransit;
using Microsoft.Extensions.Logging.Console;

namespace ExaminationSystem
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Logging.AddSimpleConsole(options =>
            {
                options.TimestampFormat = "yyyy-MM-dd HH:mm:ss.fff ";
                options.SingleLine = true;
            });
            builder.Logging.AddFilter<ConsoleLoggerProvider>("MassTransit", LogLevel.None);
            builder.Logging.AddFilter<ConsoleLoggerProvider>("MassTransit.EntityFrameworkCoreIntegration", LogLevel.None);
            builder.Logging.AddProvider(new OutboxConsoleLoggerProvider());

            builder.Services.AddMassTransit(x =>
            {
                //x.AddConsumers(Assembly.GetExecutingAssembly());
                //x.AddConsumer<DiplomaCreatedConsumer>();
                x.AddConsumer<DeleteDiplomaCommandConsumer>();
                x.AddEntityFrameworkOutbox<Context>(o =>
                {
                    o.UseSqlServer();
                    o.UseBusOutbox();
                });
                x.AddConfigureEndpointsCallback((context, name, cfg) =>
                {
                    cfg.UseEntityFrameworkOutbox<Context>(context);
                });
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host("localhost", "/", h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });

                    cfg.ConfigureEndpoints(context);

                    //cfg.ReceiveEndpoint("Diploma-creation-queue", e =>
                    //{
                    //    e.UseMessageRetry(r => r.Intervals(
                    //        TimeSpan.FromSeconds(1),
                    //        TimeSpan.FromSeconds(5),
                    //        TimeSpan.FromSeconds(10)));
                    //    e.UseInMemoryOutbox();
                    //    e.ConfigureConsumer<DiplomaCreatedConsumer>(context);
                    //});

                    //cfg.ReceiveEndpoint("diploma-delete-command-queue", e =>
                    //{
                    //    e.UseMessageRetry(r => r.Intervals(
                    //        TimeSpan.FromSeconds(1),
                    //        TimeSpan.FromSeconds(5),
                    //        TimeSpan.FromSeconds(10)));
                    //    e.UseInMemoryOutbox();
                    //    e.ConfigureConsumer<DeleteDiplomaCommandConsumer>(context);
                    //});
                });
            });

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
            app.MapAllEndpoints();
            #endregion


            app.Run();
        }
    }
}
