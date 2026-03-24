using webApi.configuration;

namespace webApi.Extensions;

public static class ConfigExtensions
{
    public static IServiceCollection AddAppConfig(this IServiceCollection services, IConfiguration config)
    {
        services.Configure<JwtOptionConfig>(config.GetSection("Jwt"));

        services.AddJwtAuthentication(config);
        services.AddCorsPolicy();
        services.AddValidationResponse();

        return services;
    }
}