using MediatR;
using MyProject.Application.Authentication.Commands.Authenticate;
using MyProject.Application.Interfaces;
using MyProject.Application.Payloads;
using MyProject.Domain.Interfaces;

namespace MyProject.Application.Authentication.Handlers.Authenticate;

public class AuthenticateCommandHandler : IRequestHandler<AuthenticateCommand, Result<string?>>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public AuthenticateCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<Result<string?>> Handle(AuthenticateCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByUsernameAsync(request.Username);

        if (user is null || !_passwordHasher.Verify(request.Password, user.PasswordHash))
        {
            return Result<string?>.Fail("Invalid username or password.");
        }

        return new Result<string?>(true, "Authenticating...", 200, _jwtTokenGenerator.GenerateToken(user));
    }
}
