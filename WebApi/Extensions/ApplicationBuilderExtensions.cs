using Microsoft.AspNetCore.Diagnostics;
using Application.Interfaces.Persistence;
using WebApi.Middleware;

namespace WebApi.Extensions;

public static class ApplicationBuilderExtensions
{
    public static async Task MigrateDatabaseAsync(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var migrator = scope.ServiceProvider.GetRequiredService<IDbMigrator>();
        await migrator.MigrateAsync();
    }
    
    public static void ConfigurePipeline(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseExceptionHandler("/error");
        app.UseHttpsRedirection();
        app.UseRouting();
    }
    
    public static void MapApplicationEndpoints(this WebApplication app)
    {
        app.UseMiddleware<JwtMiddleware>();
        app.MapControllers();

        app.Map("/error", async (HttpContext context, IExceptionHandler exceptionHandler) =>
        {
            var exceptionFeature = context.Features.Get<IExceptionHandlerFeature>();
            if (exceptionFeature?.Error != null)
            {
                await exceptionHandler.TryHandleAsync(context, exceptionFeature.Error, context.RequestAborted);
            }
        });
    }
}

