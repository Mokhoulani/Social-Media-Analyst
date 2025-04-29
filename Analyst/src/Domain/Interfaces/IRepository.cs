using System.Linq.Expressions;
using Domain.Primitives;
using Domain.Shared;
using Domain.Specification;

namespace Domain.Interfaces;

public interface IRepository<T, TKey> where T : Entity<TKey>, IAggregateRoot
{
    Task<Result<T>> AddAsync(T entity, CancellationToken cancellationToken);
    Task<Result<T>> GetByIdAsync(TKey id, CancellationToken cancellationToken);
    Task<Result<T>> GetAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken);
    Task<Result<List<T>>> GetAllAsync(CancellationToken cancellationToken);
    Task<Result<T>> SoftUpdateAsync(T entity, CancellationToken cancellationToken);
    Task<Result<T>> FullUpdateAsync(T entity, CancellationToken cancellationToken);
    Task<Result<T>> DeleteAsync(TKey id, CancellationToken cancellationToken);
    Task<Result<T>> FindOneAsync(Specification<T, TKey> specification, CancellationToken cancellationToken);
    Task<Result<List<T>>> FindManyAsync(Specification<T, TKey> specification, CancellationToken cancellationToken);
    Task<Result<bool>> ExistsAsync(Specification<T, TKey> specification, CancellationToken cancellationToken);
    Task<Result<int>> CountAsync(Specification<T, TKey> specification, CancellationToken cancellationToken);
    IQueryable<T> AsQueryable(bool trackChanges = false);
}