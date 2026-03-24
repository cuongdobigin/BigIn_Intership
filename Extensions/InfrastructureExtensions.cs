using System.Reflection;
using Microsoft.EntityFrameworkCore;
using webApi.Infrastructure.Persistence;

namespace webApi.Extensions;

public static class InfrastructureExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(config.GetConnectionString("DefaultConnection")));

        services.AddAutoMapper(cfg => { }, Assembly.GetExecutingAssembly());

        return services;
    }
}