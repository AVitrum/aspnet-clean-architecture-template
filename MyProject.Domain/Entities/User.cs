using MyProject.Domain.Enums;

namespace MyProject.Domain.Entities;

public sealed class User : BaseEntity
{
    public string Username { get; private set; } = null!;
    public string Email { get; private set; } = null!;
    public string PasswordHash { get; private set; } = null!;
    public UserRole Role { get; private set; }

    private User() { } 

    public static User Create(string username, string email, string passwordHash, UserRole role = UserRole.User)
    {
        return new User
        {
            Id = Guid.NewGuid(),
            Username = username,
            Email = email,
            PasswordHash = passwordHash,
            Role = role
        };
    }

    public void UpdateEmail(string email)
    {
        Email = email;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateRole(UserRole role)
    {
        Role = role;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdatePasswordHash(string passwordHash)
    {
        PasswordHash = passwordHash;
        UpdatedAt = DateTime.UtcNow;
    }
}