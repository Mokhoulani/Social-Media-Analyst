using Domain.Primitives;

namespace Domain.Interfaces;
public interface IUnitOfWork
{
    IRepository<T> Repository<T>() where T : Entity, IAggregateRoot;
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
    public void Dispose();
}