using webApi.Infrastructure.Middleware;

namespace webApi.Extensions;

public static class AppExtensions
{
    public static WebApplication UseAppPipeline(this WebApplication app)
    {
        app.UseMiddleware<ExceptionHandlingMiddleware>();

        app.UseHttpsRedirection();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseCors("AllowVueDev");

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        return app;
    }
}