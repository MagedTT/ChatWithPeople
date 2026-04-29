using System.Text;
using Contracts;
using Entities.ConfigurationModels;
using Entities.Models;
using LoggerService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Repository;
using Service;
using Service.Contracts;

namespace ChatWithPeople.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureSQLConnection(this IServiceCollection services)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(Environment.GetEnvironmentVariable("ChatWithPeople__ConnectionString"), b => b.MigrationsAssembly("Repository"));
        });
    }

    public static void ConfigureLoggerManager(this IServiceCollection services)
    {
        services.AddScoped<ILoggerManager, LoggerManager>();
    }

    public static void ConfigureServiceManager(this IServiceCollection services)
    {
        services.AddScoped<IServiceManager, ServiceManager>();
    }

    public static void ConfigureRepositoryManager(this IServiceCollection services)
    {
        services.AddScoped<IRepositoryManager, RepositoryManager>();
    }

    public static void ConfigureIdentity(this IServiceCollection services)
    {
        services.AddIdentity<User, IdentityRole<Guid>>(options =>
        {
            options.Password.RequiredLength = 3;
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
        })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();
    }

    public static void ConfigureJWT(this IServiceCollection services, IConfiguration configuration)
    {
        IConfigurationSection jwtSettings = configuration.GetSection("JwtSettings");
        string key = Environment.GetEnvironmentVariable("ChatWithPeople__SecretKey")!;

        services.AddAuthentication(authenticationOptions =>
        {
            authenticationOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            authenticationOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ClockSkew = TimeSpan.Zero,
                ValidIssuer = jwtSettings["ValidIssuer"],
                ValidAudience = jwtSettings["ValidAudience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
            };

            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    string? token = context.Request.Query["access_token"];
                    foreach (var x in context.Request.Query.ToList())
                    {
                        Console.WriteLine($"==> {x.Key}: {x.Value}");
                    }
                    Console.WriteLine($"Token: {token}");
                    string path = context.HttpContext.Request.Path;
                    Console.WriteLine($"======================> {path}");
                    if (!string.IsNullOrEmpty(token) && path.StartsWith("/hubs"))
                        context.Token = token;

                    return Task.CompletedTask;
                }
            };
        });
    }

    public static void AddJwtConfiguration(this IServiceCollection services, IConfiguration configuration)
        => services.Configure<JwtConfiguration>(configuration.GetSection("JwtSettings"));

    public static void ConfigureCORS(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", config =>
            {
                config
                    .WithOrigins("http://localhost:4200")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });
    }
}