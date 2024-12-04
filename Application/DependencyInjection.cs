using Application.Books;
using Application.Common.Interfaces;
using Application.Files;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(DependencyInjection).Assembly);

        services.AddScoped<IBookService, BookService>();
        services.AddScoped<IFileService, FileService>();
        return services;
    }
}