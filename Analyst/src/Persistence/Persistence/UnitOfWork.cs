using Domain.Interfaces;
using Domain.Primitives;
using Persistence.Persistence.Repositories;

namespace Persistence.Persistence;

internal sealed class UnitOfWork(ApplicationDbContext dbContext, Dictionary<Type, object> repositories) : IUnitOfWork
{
    public IRepository<TEntity, TKey> Repository<TEntity, TKey>() where TEntity : Entity<TKey>, IAggregateRoot
    {
        if (repositories.TryGetValue(typeof(TEntity), out var repo))
        {
            return (IRepository<TEntity, TKey>)repo;
        }

        var repository = new Repository<TEntity, TKey>(dbContext);
        repositories[typeof(TEntity)] = repository;

        return repository;
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default) =>
        dbContext.SaveChangesAsync(cancellationToken);

    public void Dispose()
    {
        dbContext.Dispose();
    }
}