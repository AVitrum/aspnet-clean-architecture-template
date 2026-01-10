using MediatR;
using Application.Common;

namespace Application.Features.Authentication.Commands.Authenticate;


public sealed record AuthenticateCommand(string Username, string Password) : IRequest<Result<string>>;

