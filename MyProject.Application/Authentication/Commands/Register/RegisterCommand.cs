using MediatR;
using MyProject.Application.Payloads;

namespace MyProject.Application.Authentication.Commands.Register;

public record RegisterCommand(string Username, string Email, string Password) : IRequest<Result<string?>>;
