using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using chessApi.Application.dto;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace chessApi.security.impl;

public class JwtTokenTokenGenerator(IConfiguration config) : IJwtTokenGenerator
{
    public string GenerateToken(UserDto dto)
    {
        var jwt = config.GetSection("JwtSettings");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["SecretKey"]!));
        var jwtIssuer = config["JwtSettings:Issuer"] ?? "GameAPI";
        var jwtAudience = config["JwtSettings:Audience"] ?? "GameAPI";
        var jwtExpireMinutes = int.Parse(config["JwtSettings:ExpireMinutes"] ?? "60");

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, dto.Id!),
            new Claim(JwtRegisteredClaimNames.Email, dto.Email),
            new Claim(JwtRegisteredClaimNames.Name, dto.Name),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString())
        };

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: jwtIssuer,
            audience: jwtAudience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(jwtExpireMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}