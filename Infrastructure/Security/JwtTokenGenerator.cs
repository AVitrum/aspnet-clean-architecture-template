using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Application.Interfaces.Security;
using Domain.Entities;

namespace Infrastructure.Security;

public sealed class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly IConfiguration _configuration;

    public JwtTokenGenerator(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(GetJwtKey());
        var expiryMinutes = GetExpiryMinutes();
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity([
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            ]),
            Expires = DateTime.UtcNow.AddMinutes(expiryMinutes),
            Issuer = GetJwtIssuer(),
            Audience = GetJwtAudience(),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
    
    private string GetJwtKey()
    {
        return Environment.GetEnvironmentVariable("JWT_KEY") 
            ?? _configuration["Jwt:Key"] 
            ?? throw new InvalidOperationException("JWT_KEY environment variable or Jwt:Key configuration is required");
    }
    
    private string GetJwtIssuer()
    {
        return Environment.GetEnvironmentVariable("JWT_ISSUER") 
            ?? _configuration["Jwt:Issuer"] 
            ?? "CleanArchitecture";
    }
    
    private string GetJwtAudience()
    {
        return Environment.GetEnvironmentVariable("JWT_AUDIENCE") 
            ?? _configuration["Jwt:Audience"] 
            ?? "CleanArchitectureUsers";
    }
    
    private int GetExpiryMinutes()
    {
        var envValue = Environment.GetEnvironmentVariable("JWT_EXPIRY_MINUTES");
        if (!string.IsNullOrEmpty(envValue) && int.TryParse(envValue, out var minutes))
            return minutes;
        
        return 60; // Default to 60 minutes
    }
}