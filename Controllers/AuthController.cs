using Microsoft.AspNetCore.Mvc;
using SigmaMessenger.Services;
using SigmaMessenger.Models;
using SigmaMessenger.DTOs;

namespace SigmaMessenger.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _auth;

    public AuthController(AuthService auth)
    {
        _auth = auth;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(AuthDTO dto)
    {
        if (await _auth.RegisterAsync(dto) == null)
        {
            return BadRequest("Имя занято");
        }

        return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(AuthDTO dto)
    {
        var token = await _auth.LogInAsync(dto);
        if (token == null)
        {
            return Unauthorized("Неверный логин или пароль");
        }

        return Ok(new { token });
    }
}