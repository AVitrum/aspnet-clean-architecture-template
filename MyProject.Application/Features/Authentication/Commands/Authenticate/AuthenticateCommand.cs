using MediatR;
using MyProject.Application.Common;

namespace MyProject.Application.Features.Authentication.Commands.Authenticate;


public sealed record AuthenticateCommand(string Username, string Password) : IRequest<Result<string>>;

