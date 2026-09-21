using chessApi.Domain.model;

namespace chessApi.Domain.repository;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetAll(CancellationToken cancellationToken = default);
    Task<User> Create(User user, CancellationToken cancellationToken = default);
    Task<User?> GetById(string id, CancellationToken cancellationToken = default);
    Task<bool> DeleteById(string id, CancellationToken cancellationToken = default);
    Task<User?> GetByEmail(string email, CancellationToken cancellationToken = default);
    Task<User> Update(User user, CancellationToken cancellationToken = default);
}