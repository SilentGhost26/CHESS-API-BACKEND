using chessApi.Application.dto;

namespace chessApi.Application.service;

public interface IUserService
{
    Task<IEnumerable<UserDto>> GetAllUsers(CancellationToken cancellationToken = default);
    Task<UserDto> GetUserById(string id, CancellationToken cancellationToken = default);
    Task<UserDto> UpdateUser(string id,UpdateUserDto dto, CancellationToken cancellationToken = default);
}