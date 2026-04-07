using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using BCrypt.Net;
using FinalProjectDss.DTOs;
using FinalProjectDss.Models;
using FinalProjectDss.Repositories;
using FinalProjectDss.DTOs;
using BCrypt.Net;  // Add this line

namespace FinalProjectDss.Services;

public class AuthService
{
    private readonly IGenericRepository<User> _userRepository;
    private readonly IConfiguration _configuration;

    public AuthService(IGenericRepository<User> userRepository, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _configuration = configuration;
    }

    public async Task<AuthUserResponse?> RegisterAsync(RegisterRequest request)
    {
        if (await _userRepository.ExistsAsync(u => u.Email == request.Email.ToLower()))
            return null;

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = request.Email.ToLower(),
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            DisplayName = request.DisplayName
        };

        await _userRepository.AddAsync(user);

        return new AuthUserResponse { Id = user.Id, Email = user.Email, DisplayName = user.DisplayName };
    }

    public async Task<LoginResponse?> LoginAsync(LoginRequest request)
    {
        var user = (await _userRepository.FindAsync(u => u.Email == request.Email.ToLower())).FirstOrDefault();
        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            return null;

        var token = GenerateJwtToken(user);

        return new LoginResponse
        {
            AccessToken = token,
            TokenType = "Bearer",
            ExpiresInSeconds = 3600,
            User = new AuthUserResponse { Id = user.Id, Email = user.Email, DisplayName = user.DisplayName }
        };
    }

    private string GenerateJwtToken(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? "secret-key-1234567890"));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(claims: new[] { new Claim("userId", user.Id.ToString()) }, expires: DateTime.UtcNow.AddHours(1), signingCredentials: creds);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
