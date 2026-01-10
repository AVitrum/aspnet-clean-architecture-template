namespace WebApi.Contracts.Requests;

public sealed record AuthenticateRequest(string Username, string Password);

