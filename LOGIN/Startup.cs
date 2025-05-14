using LOGIN.Services.Interfaces;
using LOGIN.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using LOGIN.Services;
using LOGIN.Dtos;
using LOGIN.Dtos.NotificationDtos;

namespace LOGIN
{
    public class Startup
    {
        private readonly string _corsPolicy = "CorsPolicy";

        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var jwtSettings = Configuration.GetSection("JWT");
            var secretKey = jwtSettings["Secret"];

            if (string.IsNullOrWhiteSpace(secretKey))
            {
                throw new InvalidOperationException("JWT secret key is missing from configuration.");
            }

            var connString = Configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseMySql(connString, ServerVersion.AutoDetect(connString),
                    mySqlOptions => mySqlOptions.SchemaBehavior(MySqlSchemaBehavior.Ignore)));

            // Add custom services
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<IComunicateServices, ComunicateServices>();
            services.Configure<ApiSettings>(Configuration.GetSection("ApiSettings"));
            services.AddTransient<IAPiSubscriberServices, APiSubscriberServices>();
            services.AddHttpClient<IAPiSubscriberServices, APiSubscriberServices>();
            services.AddTransient<IReportService, ReportService>();
            services.AddTransient<IBlocksService, BlocksService>();
            services.AddScoped<IDistrictsPointsService, DistrictsPointsService>();
            services.AddTransient<ILinesService, LinesService>();
            services.AddTransient<INeighborhoodsColoniesService, NeighborhoodsColoniesService>();
            services.AddTransient<IRegistrationWaterService, RegistrationWaterService>();
            services.AddTransient<IRegistrationWaterNeighborhoodsColoniesService, RegistrationWaterNeighborhoodsColoniesService>();
            services.AddTransient<IStateService, StateService>();
            //Services notification
            // Agrega después de builder.Services.AddDbContext
            services.Configure<DeviceNotificationRequest>(Configuration.GetSection("Firebase"));
            services.AddScoped<INotificationService, NotificationService>();

            // Asegúrate que el archivo firebase-config.json esté en el directorio correcto
            //var firebaseConfigPath = Path.Combine(Directory.GetCurrentDirectory(), "firebase-config.json");
            //if (!File.Exists(firebaseConfigPath))
            //{
            //    throw new FileNotFoundException("Archivo de configuración de Firebase no encontrado");
            //}

            // Add AutoMapper service
            services.AddAutoMapper(typeof(Startup));

            // Add Identity
            services.AddIdentity<UserEntity, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            // JWT Authentication
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["ValidIssuer"],
                    ValidAudience = jwtSettings["ValidAudience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                };
            });

            // Configure CORS
            services.AddCors(options =>
            {
                options.AddPolicy(_corsPolicy, builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyHeader()
                           .AllowAnyMethod()
                           .WithExposedHeaders("Content-Disposition");
                });
            });

            // Add Swagger
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            // Add Controllers
            services.AddControllers();
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
                          
            app.UseSwagger();
            app.UseSwaggerUI();
            //{
            //    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            //    c.RoutePrefix = string.Empty;
            //});
            //{
            //    app.UseExceptionHandler("/error");
            //    app.UseHsts();
            //}

            app.UseHttpsRedirection();
            app.UseRouting();

            // Use CORS
            app.UseCors(_corsPolicy);

            // Use Authentication
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
