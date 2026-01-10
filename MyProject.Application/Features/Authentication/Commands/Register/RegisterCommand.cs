using MediatR;
using MyProject.Application.Common;

namespace MyProject.Application.Features.Authentication.Commands.Register;

public sealed record RegisterCommand(string Username, string Email, string Password) : IRequest<Result<string>>;

