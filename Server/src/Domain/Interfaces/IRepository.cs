using System.Linq.Expressions;

namespace Domain.Interfaces;

public interface IRepository<T> where T : IAggregateRoot
{
    Task<T> AddAsync(T entity, CancellationToken cancellationToken);
    Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IList<T>> GetAllAsync(CancellationToken cancellationToken);
    Task UpdateAsync(T entity, CancellationToken cancellationToken);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    Task<TResult?> GetByIndexAsync<TIndex, TResult>(
        Expression<Func<T, TIndex>> indexSelector,
        TIndex index,
        CancellationToken cancellationToken);
}
