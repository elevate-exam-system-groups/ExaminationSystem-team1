using ExaminationSystem.Features.Common.Services;

namespace ExaminationSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<ApplicationDbContext>(opt =>
                opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
            );

            builder.Services.AddIdentity<User, IdentityRole>(options => 
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireDigit = true;

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
                options.Lockout.MaxFailedAccessAttempts = 5;
            })
              .AddEntityFrameworkStores<ApplicationDbContext>();


            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddScoped<IEmailService, EmailService>();

            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));



            builder.Services.AddAuthentication(options => 
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options => 
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = builder.Configuration["JWTOptions:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["JWTOptions:Audience"],
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                        builder.Configuration["JWTOptions:SecretKey"]))
                };
            });

            #endregion

            var app = builder.Build();

            #region Configure the HTTP request pipeline.

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            #endregion           

            app.Run();
        }
    }
}
