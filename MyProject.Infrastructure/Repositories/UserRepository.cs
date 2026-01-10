using Microsoft.EntityFrameworkCore;
using MyProject.Application.Interfaces.Repositories;
using MyProject.Domain.Entities;
using MyProject.Infrastructure.Persistence;

namespace MyProject.Infrastructure.Repositories;

public sealed class UserRepository(AppDbContext context) : GenericRepository<User>(context), IUserRepository
{
    public async Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        return await DbSet.FirstOrDefaultAsync(u => u.Username == username, cancellationToken);
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await DbSet.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
    }

    public async Task<bool> ExistsByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        return await DbSet.AnyAsync(u => u.Username == username, cancellationToken);
    }

    public async Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await DbSet.AnyAsync(u => u.Email == email, cancellationToken);
    }
}