using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using webApi.Dto.Response;

namespace webApi.configuration;

public static class JwtConfig
{
    public static IServiceCollection AddJwtAuthentication(
        this IServiceCollection services, IConfiguration config)
    {
        var key = config["Jwt:SecretKey"]
                  ?? throw new InvalidOperationException("Jwt:SecretKey not found");

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuers = new[] { "http://localhost:5052" },
    
                    ValidateAudience = true,
                    ValidAudiences = new[] { "http://localhost:5052" },
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(key))
                };

                options.Events = new JwtBearerEvents
                {
                    OnChallenge = async context =>
                    {
                        context.HandleResponse();
                        context.Response.ContentType = "application/json";
                        context.Response.StatusCode = 401;
                        await context.Response.WriteAsJsonAsync(
                            ApiResponse<object>.Fail("Unauthorized", 3001));
                    },
                    OnForbidden = async context =>
                    {
                        context.Response.ContentType = "application/json";
                        context.Response.StatusCode = 403;
                        await context.Response.WriteAsJsonAsync(
                            ApiResponse<object>.Fail("Forbidden", 3002));
                    }
                };
            });

        return services;
    }
}