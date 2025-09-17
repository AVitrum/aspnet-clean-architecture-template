using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyProject.Application.Authentication.Commands.Authenticate;
using MyProject.Application.Authentication.Commands.Register;
using MyProject.Application.Payloads.Requests;

namespace MyProject.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public sealed class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var command = new RegisterCommand(request.Username, request.Email, request.Password);
        var result = await _mediator.Send(command);
        return StatusCode(result.Code, result);
    }

    [HttpPost("authenticate")]
    public async Task<IActionResult> Authenticate([FromBody] AuthenticateRequest request)
    {
        var command = new AuthenticateCommand(request.Username, request.Password);
        var result = await _mediator.Send(command);
        return StatusCode(result.Code, result);
    }
}