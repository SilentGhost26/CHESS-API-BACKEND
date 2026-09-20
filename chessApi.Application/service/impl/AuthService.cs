using chessApi.Application.dto;
using chessApi.Application.mapper;
using chessApi.Domain.repository;

namespace chessApi.Application.service.impl;

public class AuthService(IUserRepository userRepository) : IAuthService
{
    public async Task<UserDto> RegisterUser(CreateUserDto dto, CancellationToken cancellationToken = default)
    {
        var user = UserMapper.ToModel(dto);
        var existentUser = await userRepository.GetByEmail(dto.Email, cancellationToken);
        if (existentUser != null)
        {
            throw new InvalidOperationException("Email already in use");
        }

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
        user.Password = passwordHash;
        
        return UserMapper.ToResponseDto(await userRepository.Create(user, cancellationToken));
    }

    public async Task<UserDto> LoginUser(LoginDto dto, CancellationToken cancellationToken = default)
    {
        var user = await userRepository.GetByEmail(dto.Email!, cancellationToken);
        if (user == null)
        {
            throw new KeyNotFoundException("User not found");
        }
        
        var isValidPassword = BCrypt.Net.BCrypt.Verify(dto.Password, user.Password);
        if (!isValidPassword)
        {
            throw new InvalidOperationException("Incorrect email or password");
        }

        return UserMapper.ToResponseDto(user);
    }
}