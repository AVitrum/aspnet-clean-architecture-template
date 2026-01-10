using Microsoft.AspNetCore.Diagnostics;
using Application;
using Infrastructure;

namespace WebApi.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddWebApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        services.AddApplication();
        services.AddInfrastructure(configuration);
        
        services.AddSingleton<IExceptionHandler, Middleware.GlobalExceptionHandler>();

        return services;
    }
}

