using MediatR;
using Application.Common;

namespace Application.Features.Authentication.Commands.Register;

public sealed record RegisterCommand(string Username, string Email, string Password) : IRequest<Result<string>>;

