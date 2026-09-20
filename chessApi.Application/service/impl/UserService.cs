using chessApi.Application.dto;
using chessApi.Application.mapper;
using chessApi.Domain.repository;

namespace chessApi.Application.service.impl;

public class UserService(IUserRepository userRepository) : IUserService
{
    public async Task<IEnumerable<UserDto>> GetAllUsers(CancellationToken cancellationToken = default)
    {
        var users = await userRepository.GetAll(cancellationToken);
        return users.Select(UserMapper.ToResponseDto);
    }

    public async Task<UserDto> GetUserById(string id, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            throw new ArgumentException("id cannot be null or empty");
        }

        return UserMapper.ToResponseDto(await userRepository.GetById(id, cancellationToken));
    }

    public async Task<UserDto> UpdateUser(string id, UpdateUserDto dto, CancellationToken cancellationToken = default)
    {
        var user = await userRepository.GetById(id, cancellationToken);
        user.Name = dto.Name ?? user.Name;

        return UserMapper.ToResponseDto(await userRepository.Update(user, cancellationToken));
    }
}