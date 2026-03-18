namespace webApi.Infrastructure.Middleware;

using System.Text.Json;
using webApi.Domain.Exceptions;
using webApi.Dto.Response;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (AppException ex)
        {
            await HandleAsync(context, ex.HttpStatusCode, ex.Message, ex.Code);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Invalid operation");
            await HandleAsync(context, 400, ex.Message, 1003);
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogError(ex, "Not found");
            await HandleAsync(context, 404, ex.Message, 1004);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogError(ex, "Unauthorized");
            await HandleAsync(context, 401, ex.Message, 1005);
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, "Argument error");
            await HandleAsync(context, 400, ex.Message, 1006);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error");
            await HandleAsync(context, 500, "Internal server error", 9999);
        }
    }

    private static async Task HandleAsync(
        HttpContext context, int httpStatus, string message, int code)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = httpStatus;

        var response = ApiResponse<object>.Fail(message, code);

        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}