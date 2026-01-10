using MyProject.Domain.Entities;

namespace MyProject.Application.Interfaces.Security;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}
