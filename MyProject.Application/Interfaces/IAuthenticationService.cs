using MyProject.Application.Payloads;

namespace MyProject.Application.Interfaces;

public interface IAuthenticationService
{
    // TODO: Change types
    Task<Result<string?>> RegisterAsync(string username, string email, string password);
    Task<Result<string?>> AuthenticateAsync(string usernameOrEmail, string password);
}