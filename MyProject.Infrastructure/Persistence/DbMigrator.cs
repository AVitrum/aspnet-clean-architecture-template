using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyProject.Application.Interfaces.Persistence;

namespace MyProject.Infrastructure.Persistence;

public sealed class DbMigrator : IDbMigrator
{
    private readonly AppDbContext _context;
    private readonly ILogger<DbMigrator> _logger;

    public DbMigrator(AppDbContext context, ILogger<DbMigrator> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task MigrateAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var pendingMigrations = await _context.Database.GetPendingMigrationsAsync(cancellationToken);
            if (pendingMigrations.Any())
            {
                _logger.LogInformation("Applying {Count} pending migrations...", pendingMigrations.Count());
                await _context.Database.MigrateAsync(cancellationToken);
                _logger.LogInformation("Database migrations applied successfully.");
            }
            else
            {
                _logger.LogInformation("No pending migrations found.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while migrating the database.");
            throw;
        }
    }
}

