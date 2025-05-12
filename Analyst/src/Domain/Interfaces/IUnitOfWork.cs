using Domain.Primitives;

namespace Domain.Interfaces;
public interface IUnitOfWork
{
    IRepository<T, TKey> Repository<T, TKey>() where TKey : notnull where T : Entity<TKey>, IAggregateRoot;
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
    public void Dispose();
}