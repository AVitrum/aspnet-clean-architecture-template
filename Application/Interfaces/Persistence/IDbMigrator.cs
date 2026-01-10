namespace Application.Interfaces.Persistence;

public interface IDbMigrator
{
    Task MigrateAsync(CancellationToken cancellationToken = default);
}

