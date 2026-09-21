using chessApi.Application.dto;
using chessApi.Domain.model;

namespace chessApi.Application.mapper;

public class UserMapper
{
    public static UserDto ToResponseDto(User user)
    {
        return new UserDto()
        {
            Id = user.Id,
            Email = user.Email,
            Name = user.Name,
        };
    }

    public static User ToModel(CreateUserDto dto)
    {
        return new User()
        {
            Email = dto.Email,
            Name = dto.Name,
            Password = dto.Password,
        };
    }
}