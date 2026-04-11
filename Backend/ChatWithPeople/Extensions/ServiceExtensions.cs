using Microsoft.EntityFrameworkCore;
using Repository;

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
}