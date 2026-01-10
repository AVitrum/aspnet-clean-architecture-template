using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Application.Interfaces.Persistence;
using Application.Interfaces.Repositories;
using Application.Interfaces.Security;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Infrastructure.Security;

namespace Infrastructure;

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