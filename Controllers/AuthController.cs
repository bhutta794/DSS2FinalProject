using Microsoft.AspNetCore.Mvc;
using FinalProjectDss.DTOs;
using FinalProjectDss.Services;

namespace FinalProjectDss.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        // Validate ModelState
        if (!ModelState.IsValid)
        {
            return BadRequest(new ProblemDetails
            {
                Type = "https://httpstatuses.com/400",
                Title = "Validation failed",
                Status = 400,
                Extensions = { ["errors"] = ModelState }
            });
        }

        var result = await _authService.RegisterAsync(request);
        if (result == null)
        {
            return Conflict(new ProblemDetails
            {
                Type = "https://httpstatuses.com/409",
                Title = "Conflict",
                Status = 409,
                Detail = "Email already registered"
            });
        }

        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        // Validate ModelState
        if (!ModelState.IsValid)
        {
            return BadRequest(new ProblemDetails
            {
                Type = "https://httpstatuses.com/400",
                Title = "Validation failed",
                Status = 400,
                Extensions = { ["errors"] = ModelState }
            });
        }

        var result = await _authService.LoginAsync(request);
        if (result == null)
        {
            return Unauthorized(new ProblemDetails
            {
                Type = "https://httpstatuses.com/401",
                Title = "Unauthorized",
                Status = 401,
                Detail = "Invalid email or password"
            });
        }

        return Ok(result);
    }
}
