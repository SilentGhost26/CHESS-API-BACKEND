using System.Net.Mail;
using chessApi.Application.dto;
using chessApi.Application.service;
using chessApi.response;
using chessApi.security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace chessApi.controller;

[ApiController]
[Route("/api/v1/[controller]")]
public class AuthController(IAuthService authService, IJwtTokenGenerator tokenGenerator) : ControllerBase
{
    
    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> RegisterUser([FromBody] CreateUserDto request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Name) ||
            string.IsNullOrWhiteSpace(request.Email) ||
            string.IsNullOrWhiteSpace(request.Password))
        {
            return BadRequest(new { message = "All fields are mandatory" });
        }

        if (!IsValidEmail(request.Email))
        {
            return BadRequest(new { message = "Invalid Email" });
        }
        
        try
        {
            var user = await authService.RegisterUser(request, cancellationToken);
            return Created("/api/v1/register", user);
        }
        catch (InvalidOperationException e) { return Conflict(new { message = e.Message }); }
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginDto request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Email) ||
            string.IsNullOrWhiteSpace(request.Password))
        {
            return BadRequest(new { message = "All fields are mandatory" });
        }
        if (!IsValidEmail(request.Email))
        {
            return BadRequest(new { message = "Invalid Email" });
        }
        
        try
        {
            var user = await authService.LoginUser(request, cancellationToken);
            var token = tokenGenerator.GenerateToken(user);
            
            return Ok(new LoginResponseDto
            {
                Token = token,
                User = user,
            });
        }
        catch (KeyNotFoundException e) { return NotFound(new { message = e.Message }); }
        catch (InvalidOperationException e) { return Unauthorized(new { message = e.Message }); }
    }

    private bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email)) return false;
        try
        {
            var addr = new MailAddress(email);
            return addr.Address == email;
        }
        catch (Exception e)
        {
            return false;
        }
    }
}