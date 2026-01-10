using MediatR;
using Application.Common;
using Application.Interfaces.Repositories;
using Application.Interfaces.Security;

namespace Application.Features.Authentication.Commands.Authenticate;

public sealed class AuthenticateCommandHandler : IRequestHandler<AuthenticateCommand, Result<string>>
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

    public async Task<Result<string>> Handle(AuthenticateCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByUsernameAsync(request.Username, cancellationToken);

        if (user is null || !_passwordHasher.Verify(request.Password, user.PasswordHash))
        {
            return Result<string>.Failure("Invalid username or password.");
        }

        var token = _jwtTokenGenerator.GenerateToken(user);
        return Result<string>.Success(token, "Authentication successful.");
    }
}

