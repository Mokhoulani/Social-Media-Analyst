using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.Interfaces;
using Domain.Primitives;
using Infrastructure.Persistence.Repositories;

namespace Infrastructure.Persistence;
internal sealed class UnitOfWork(
    ApplicationDbContext dbContext,
    Dictionary<Type, object> repositories) : IUnitOfWork
{
    public IRepository<TEntity> Repository<TEntity>() where TEntity : Entity, IAggregateRoot
    {
        if (repositories.ContainsKey(typeof(TEntity)))
        {
            return (IRepository<TEntity>)repositories[typeof(TEntity)];
        }

        var repository = new Repository<TEntity>(dbContext);
        repositories.Add(typeof(TEntity), repository);
        return repository;
    }
    
    public Task SaveChangesAsync(CancellationToken cancellationToken = default) =>
        dbContext.SaveChangesAsync(cancellationToken);
    
    public void Dispose()
    {
        dbContext.Dispose();
    }
}