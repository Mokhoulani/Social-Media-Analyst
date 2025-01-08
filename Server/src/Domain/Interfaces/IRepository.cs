using Domain.Base;
namespace Domain.Interfaces;

public interface IRepository<T> where T : BaseEntity, IAggregateRoot
{
    Task AddAsync(T entity, CancellationToken cancellationToken);
    Task<T> GetByIdAsync(int id, CancellationToken cancellationToken);
    
}