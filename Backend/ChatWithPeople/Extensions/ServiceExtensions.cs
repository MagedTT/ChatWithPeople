using Contracts;
using Entities.ConfigurationModels;
using Entities.Models;
using LoggerService;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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

    public static void AddJwtConfiguration(this IServiceCollection services, IConfiguration configuration)
        => services.Configure<JwtConfiguration>(configuration.GetSection("JwtSettings"));
}