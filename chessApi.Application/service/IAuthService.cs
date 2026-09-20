using chessApi.Application.dto;

namespace chessApi.Application.service;

public interface IAuthService
{
    Task<UserDto> RegisterUser(CreateUserDto dto, CancellationToken cancellationToken = default);
    Task<string> LoginUser(LoginDto dto, CancellationToken cancellationToken = default);
}