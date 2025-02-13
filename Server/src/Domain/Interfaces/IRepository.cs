using System.Linq.Expressions;
using Domain.Primitives;
using Domain.Specification;

namespace Domain.Interfaces;

public interface IRepository<T> where T : Entity, IAggregateRoot
{
    Task<T> AddAsync(T entity, CancellationToken cancellationToken);
    Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<T?> GetAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken);
    Task<IList<T>> GetAllAsync(CancellationToken cancellationToken);
    Task SoftUpdateAsync(T entity, CancellationToken cancellationToken);
    Task FullUpdateAsync(T entity, CancellationToken cancellationToken);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    Task<T?> FindOneAsync(Specification<T> specification, CancellationToken cancellationToken );
    Task<IReadOnlyList<T>> FindManyAsync(Specification<T> specification, CancellationToken cancellationToken );
    Task<bool> ExistsAsync(Specification<T> specification, CancellationToken cancellationToken );
    Task<int> CountAsync(Specification<T> specification, CancellationToken cancellationToken );
}
