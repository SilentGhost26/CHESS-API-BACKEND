using chessApi.Domain.model;
using chessApi.Infrastructure.persistence.mongoDB.document;

namespace chessApi.Infrastructure.persistence.mongoDB.mapper;

public class UserMapper : IMapper<UserDocument, User>
{
    public static UserDocument ToDocument(User model)
    {
        return new UserDocument
        {
            Id = string.IsNullOrEmpty(model.Id) ? null : model.Id,
            Name = model.Name,
            Email = model.Email,
            Password = model.Password,
            RegisterDate = model.RegisterDate,
            IsDeleted = model.IsDeleted,
        };
    }

    public static User ToModel(UserDocument document)
    {
        return new User
        {
            Id = document.Id,
            Name = document.Name,
            Email = document.Email,
            Password = document.Password,
            RegisterDate = document.RegisterDate,
            IsDeleted = document.IsDeleted,
        };
    }
}