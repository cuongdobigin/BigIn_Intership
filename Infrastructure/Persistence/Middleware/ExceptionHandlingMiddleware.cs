// Infrastructure/Middleware/ExceptionHandlingMiddleware.cs
namespace webApi.Infrastructure.Middleware;

using System.Net;
using System.Text.Json;
using webApi.Domain.Exceptions;

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
            await HandleAsync(context, ex.HttpStatusCode, ex.Code, ex.Message);
        }
        catch (Exception ex) // ← bắt lỗi không mong muốn
        {
            _logger.LogError(ex, "Unexpected error");
            await HandleAsync(context, 500, 9999, "Internal server error");
        }
    }

    private static async Task HandleAsync(
        HttpContext context, int httpStatus, int code, string message)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = httpStatus;

        var response = new
        {
            code = code,
            message = message,
            statusCode = httpStatus,
            timestamp = DateTime.UtcNow
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}