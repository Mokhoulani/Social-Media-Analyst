using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Domain.Interfaces;
using Domain.Primitives;
using Domain.Specification;

namespace Infrastructure.Persistence.Repositories;

public class Repository<T> : IRepository<T> where T : Entity, IAggregateRoot
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly DbSet<T> _dbSet;

    public Repository(ApplicationDbContext applicationDbContext)
    {
      _applicationDbContext = applicationDbContext;
        _dbSet = _applicationDbContext.Set<T>();
    }

    public async Task<T> AddAsync(T entity, CancellationToken cancellationToken)
    {
        await _dbSet.AddAsync(entity, cancellationToken);
        return entity;
    }

    public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet.FindAsync(new object[] { id }, cancellationToken);
    }
    
    public async Task<T?> GetAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken)
    {
        return await _dbSet.FirstOrDefaultAsync(predicate, cancellationToken);
    }
    
    public async Task<IList<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet.ToListAsync(cancellationToken);
    }

    public async Task SoftUpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        var existingEntity = await _dbSet.FindAsync(new object[] { entity.Id }, cancellationToken);
        
        if (existingEntity == null)
            throw new KeyNotFoundException($"Entity {typeof(T).Name} with ID {entity.Id} not found.");

        _applicationDbContext.Entry(existingEntity).CurrentValues.SetValues(entity);
    }
    
    public async Task FullUpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);

        _applicationDbContext.Entry(entity).State = EntityState.Modified;
        await Task.CompletedTask;
    }
    
    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var entity = await _dbSet.FindAsync(new object[] { id }, cancellationToken);
        if (entity == null)
            throw new KeyNotFoundException($"Entity {typeof(T).Name} with ID {id} not found.");

        _dbSet.Remove(entity);
    }

    public async Task<T?> FindOneAsync(Specification<T> specification,
        CancellationToken cancellationToken = default)
    {
        var queryWithSpec = SpecificationEvaluator.GetQuery(
            _dbSet.AsQueryable(),
            specification);
        return await queryWithSpec.FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<T>> FindManyAsync(
        Specification<T> specification,
        CancellationToken cancellationToken = default)
    {
        var queryWithSpec = SpecificationEvaluator.GetQuery(
            _dbSet.AsQueryable(),
            specification);
        return await queryWithSpec.ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(
        Specification<T> specification,
        CancellationToken cancellationToken = default)
    {
        var queryWithSpec = SpecificationEvaluator.GetQuery(
            _dbSet.AsQueryable(), 
            specification);
        return await queryWithSpec.AnyAsync(cancellationToken);
    }

    public async Task<int> CountAsync(
        Specification<T> specification,
        CancellationToken cancellationToken = default)
    {
        var queryWithSpec = SpecificationEvaluator.GetQuery(
            _dbSet.AsQueryable(),
            specification);
        return await queryWithSpec.CountAsync(cancellationToken);
    }

}
