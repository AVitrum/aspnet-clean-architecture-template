namespace MyProject.Application.Payloads.Requests;

public sealed class AuthenticateRequest
{
    public required string Username { get; init; }
    public required string Password { get; init; }
}