using chessApi.Application.dto;

namespace chessApi.Application.service;

public interface IAuthService
{
    Task<UserDto> RegisterUser(CreateUserDto dto, CancellationToken cancellationToken = default);
    Task<UserDto> LoginUser(LoginDto dto, CancellationToken cancellationToken = default);
}