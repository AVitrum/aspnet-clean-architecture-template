using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyProject.Application.Interfaces.Persistence;
using MyProject.Application.Interfaces.Repositories;
using MyProject.Application.Interfaces.Security;
using MyProject.Infrastructure.Persistence;
using MyProject.Infrastructure.Repositories;
using MyProject.Infrastructure.Security;

namespace MyProject.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Persistence
        services.AddDbContext<AppDbContext>(opt => 
            opt.UseNpgsql(Environment.GetEnvironmentVariable("DB_CONNECTION")));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IDbMigrator, DbMigrator>();

        // Repositories
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<IUserRepository, UserRepository>();

        // Security
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();

        return services;
    }
}