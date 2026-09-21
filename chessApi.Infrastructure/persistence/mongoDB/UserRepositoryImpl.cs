using chessApi.Domain.model;
using chessApi.Domain.repository;
using chessApi.Infrastructure.persistence.mongoDB.document;
using chessApi.Infrastructure.persistence.mongoDB.mapper;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace chessApi.Infrastructure.persistence.mongoDB;

public class UserRepositoryImpl : IUserRepository
{
    private readonly IMongoCollection<UserDocument> _userCollection;
    
    public UserRepositoryImpl(IMongoClient client, IOptions<MongoDbSettings> settings)
    {
        var database = client.GetDatabase(settings.Value.DatabaseName);
        _userCollection = database.GetCollection<UserDocument>("users");
    }
    
    public async Task<IEnumerable<User>> GetAll(CancellationToken cancellationToken = default)
    {
        var users = await _userCollection.Find(u => !u.IsDeleted)
            .ToListAsync(cancellationToken);
        return users.Select(UserMapper.ToModel);
    }

    public async Task<User> Create(User user, CancellationToken cancellationToken = default)
    {
        var document = UserMapper.ToDocument(user);
        await _userCollection.InsertOneAsync(document, cancellationToken: cancellationToken);
        return UserMapper.ToModel(document);
    }

    public async Task<User> GetById(string id, CancellationToken cancellationToken = default)
    {
        return UserMapper.ToModel( await _userCollection.Find(u => u.Id == id && !u.IsDeleted)
            .FirstOrDefaultAsync(cancellationToken));
    }

    public async Task<bool> DeleteById(string id, CancellationToken cancellationToken = default)
    {
        var user = await _userCollection.Find(u => u.Id == id && !u.IsDeleted)
            .FirstOrDefaultAsync(cancellationToken);
        if (user == null)
        {
            return false;
        }

        var update = Builders<UserDocument>.Update
            .Set(u => u.IsDeleted, true);
        
        await _userCollection.UpdateOneAsync(u => u.Id == id, update, cancellationToken: cancellationToken);
        return true;
    }

    public async Task<User> GetByEmail(string email, CancellationToken cancellationToken = default)
    {
        return UserMapper.ToModel(await _userCollection.Find(u => u.Email == email)
            .FirstOrDefaultAsync(cancellationToken));
    }

    public async Task<User> Update(User user, CancellationToken cancellationToken = default)
    {
        var document = await _userCollection.Find(u => u.Id == user.Id && !u.IsDeleted)
            .FirstOrDefaultAsync(cancellationToken);
        if (document == null)
        {
            return null;
        }

        document.Email = user.Email ?? document.Email;
        document.Password = user.Password ?? document.Password;
        document.Name = user.Name ?? document.Name;

        await _userCollection.ReplaceOneAsync(u => u.Id == user.Id, document, cancellationToken: cancellationToken);
        return UserMapper.ToModel(document);
    }
}