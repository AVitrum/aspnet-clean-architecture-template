using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyProject.Application.Features.Authentication.Commands.Authenticate;
using MyProject.Application.Features.Authentication.Commands.Register;
using MyProject.WebApi.Contracts.Requests;

namespace MyProject.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class UsersController : ControllerBase
{
    private readonly ISender _mediator;

    public UsersController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken cancellationToken)
    {
        var command = new RegisterCommand(request.Username, request.Email, request.Password);
        var result = await _mediator.Send(command, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("authenticate")]
    public async Task<IActionResult> Authenticate([FromBody] AuthenticateRequest request, CancellationToken cancellationToken)
    {
        var command = new AuthenticateCommand(request.Username, request.Password);
        var result = await _mediator.Send(command, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }
}