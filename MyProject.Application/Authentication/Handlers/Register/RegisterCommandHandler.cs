using MediatR;
using MyProject.Application.Authentication.Commands.Register;
using MyProject.Application.Interfaces;
using MyProject.Application.Payloads;
using MyProject.Domain.Entities;
using MyProject.Domain.Enums;
using MyProject.Domain.Interfaces;

namespace MyProject.Application.Authentication.Handlers.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<string?>>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public RegisterCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<Result<string?>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        if (await _userRepository.ExistsByUsernameAsync(request.Username))
        {
            return Result<string?>.Fail("Username already exists.");
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = request.Username,
            Email = request.Email,
            PasswordHash = _passwordHasher.Hash(request.Password),
            Role = UserRole.User
        };

        await _userRepository.AddUserAsync(user);

        var createdUser = await _userRepository.GetUserByUsernameAsync(user.Username);
        return createdUser == null
            ? Result<string?>.Fail("User registration failed.")
            : new Result<string?>(true, "User registered successfully.", 200, _jwtTokenGenerator.GenerateToken(createdUser));
    }
}
