using Microsoft.AspNetCore.Diagnostics;
using MyProject.Application;
using MyProject.Infrastructure;

namespace MyProject.WebApi.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddWebApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        services.AddSwaggerGen();
        services.AddApplication();
        services.AddInfrastructure(configuration);
        
        services.AddSingleton<IExceptionHandler, Middleware.GlobalExceptionHandler>();

        return services;
    }
}

