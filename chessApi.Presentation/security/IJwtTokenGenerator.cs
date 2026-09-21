using chessApi.Application.dto;

namespace chessApi.security;

public interface IJwtTokenGenerator
{
    string GenerateToken(UserDto dto);
}