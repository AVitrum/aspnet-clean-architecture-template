using MediatR;
using MyProject.Application.Payloads;

namespace MyProject.Application.Authentication.Commands.Authenticate;

public record AuthenticateCommand(string Username, string Password) : IRequest<Result<string?>>;
