using chessApi.Application.dto;

namespace chessApi.response;

public class LoginResponseDto
{
    public string Token { get; set; }
    public UserDto User { get; set; }
}