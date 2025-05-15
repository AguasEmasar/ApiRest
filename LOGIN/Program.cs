using dotenv.net;
using LOGIN;
using LOGIN.Database;
using LOGIN.Entities;
using Microsoft.AspNetCore.Identity;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

// Configurar Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();
builder.Host.UseSerilog(); // Integración Serilog con la aplicación

// Configurar URL una sola vez
builder.WebHost.UseUrls("http://0.0.0.0:4000");

try
{
    var startup = new Startup(builder.Configuration);
    startup.ConfigureServices(builder.Services);
    
    var app = builder.Build();
    
    // Configurar redirección de la raíz a Swagger
    app.MapGet("/", () => Results.Redirect("/swagger/index.html"));
    
    startup.Configure(app, app.Environment);
    await InitializeDatabaseAsync(app);
    
    DotEnv.Load(options: new DotEnvOptions(probeForEnv: true));
    
    // Middleware para logging de requests
    app.UseSerilogRequestLogging(options =>
    {
        options.MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
    });
    
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "La aplicación se detuvo inesperadamente");
}
finally
{
    Log.CloseAndFlush();
}

async Task InitializeDatabaseAsync(IHost app)
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    var loggerFactory = services.GetRequiredService<ILoggerFactory>();
    try
    {
        var userManager = services.GetRequiredService<UserManager<UserEntity>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var context = services.GetRequiredService<ApplicationDbContext>();
        await ApplicationDbSeeder.InitializeAsync(userManager, roleManager, context, loggerFactory);
    }
    catch (Exception ex)
    {
        Log.Error(ex, "Error al inicializar la base de datos");
        throw;
    }
}
