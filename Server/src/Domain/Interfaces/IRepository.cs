using Domain.Entities;

namespace Domain.Interfaces;

public interface IRepository<T> where T : IAggregateRoot
{
    Task<User?> AddAsync(T entity, CancellationToken cancellationToken);
    Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<IList<T>> GetAllAsync(CancellationToken cancellationToken);
    Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken);
}