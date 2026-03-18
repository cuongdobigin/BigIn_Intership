using Microsoft.AspNetCore.Mvc;
using webApi.Domain.Exceptions;
using webApi.Dto.Response;

namespace webApi.configuration;

public static class ValidationConfig
{
    public static IServiceCollection AddValidationResponse(
        this IServiceCollection services)
    {
        services.AddControllers()
            .ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errorMessage = context.ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .FirstOrDefault() ?? "UNKNOWN";

                    if (Enum.TryParse<ErrorCode>(errorMessage, out var errorCode))
                    {
                        var detail = ErrorDetails.Get(errorCode);
                        return new BadRequestObjectResult(
                            ApiResponse<object>.Fail(detail.Message, detail.Code));
                    }

                    return new BadRequestObjectResult(
                        ApiResponse<object>.Fail(errorMessage, 9999));
                };
            });

        return services;
    }
}