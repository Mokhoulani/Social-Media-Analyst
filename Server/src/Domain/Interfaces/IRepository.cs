using Domain.Base;
namespace Domain.Interfaces;

public interface IRepository<T> where T : IAggregateRoot
{
    Task AddAsync(T entity, CancellationToken cancellationToken);
    Task<T> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<IList<T>> GetAllAsync(CancellationToken cancellationToken);
}