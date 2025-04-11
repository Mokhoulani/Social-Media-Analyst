using System.Linq.Expressions;
using Domain.Errors;
using Domain.Interfaces;
using Domain.Primitives;
using Domain.Shared;
using Domain.Specification;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Persistence.Repositories;

public class Repository<T, TKey>(ApplicationDbContext applicationDbContext) : IRepository<T, TKey>
    where T : Entity<TKey>, IAggregateRoot
{
    private readonly DbSet<T> _dbSet = applicationDbContext.Set<T>();

    public async Task<Result<T>> AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        try
        {
            await _dbSet.AddAsync(entity, cancellationToken);
            return entity;
        }
        catch (Exception ex)
        {
            return Result.Failure<T>(new Error("Database.AddError", $"Failed to add entity: {ex.Message}"));
        }
    }

    public async Task<Result<T>> GetByIdAsync(TKey id, CancellationToken cancellationToken = default)
    {
        var entity = await _dbSet.FindAsync([id], cancellationToken);

        return entity is null ? DomainErrors.NotFound<T>() : entity;
    }

    public async Task<Result<T>> GetAsync(Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        var entity = await _dbSet.FirstOrDefaultAsync(predicate, cancellationToken);

        return entity is null ? DomainErrors.NotFound<T>() : entity;
    }

    public async Task<Result<List<T>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _dbSet.ToListAsync(cancellationToken);

        return entities.Count == 0 ? DomainErrors.NotFound<T>() : entities;
    }

    public async Task<Result<T>> SoftUpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        var existingEntity = await _dbSet.FindAsync([entity.Id], cancellationToken);

        if (existingEntity is null) return DomainErrors.NotFound<T>();

        _dbSet.Entry(existingEntity).CurrentValues.SetValues(entity);
        return existingEntity;
    }

    public async Task<Result<T>> FullUpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        _dbSet.Entry(entity).State = EntityState.Modified;
        await Task.CompletedTask;
        return entity;
    }

    public async Task<Result<T>> DeleteAsync(TKey id, CancellationToken cancellationToken)
    {
        var entity = await _dbSet.FindAsync([id], cancellationToken);
        if (entity is null) return DomainErrors.NotFound<T>();

        _dbSet.Remove(entity);
        return entity;
    }

    public async Task<Result<T>> FindOneAsync(Specification<T, TKey> specification,
        CancellationToken cancellationToken = default)
    {
        var queryWithSpec = SpecificationEvaluator.GetQuery(_dbSet.AsQueryable(), specification);

        var entity = await queryWithSpec.FirstOrDefaultAsync(cancellationToken);

        return entity is null ? DomainErrors.NotFound<T>() : entity;
    }

    public async Task<Result<List<T>>> FindManyAsync(Specification<T, TKey> specification,
        CancellationToken cancellationToken = default)
    {
        var queryWithSpec = SpecificationEvaluator.GetQuery(_dbSet.AsQueryable(), specification);

        var list = await queryWithSpec.ToListAsync(cancellationToken);

        return list.Count == 0 ? DomainErrors.NotFound<T>() : list;
    }

    public async Task<Result<bool>> ExistsAsync(Specification<T, TKey> specification,
        CancellationToken cancellationToken = default)
    {
        var queryWithSpec = SpecificationEvaluator.GetQuery(_dbSet.AsQueryable(), specification);

        return await queryWithSpec.AnyAsync(cancellationToken);
    }

    public async Task<Result<int>> CountAsync(Specification<T, TKey> specification,
        CancellationToken cancellationToken = default)
    {
        var queryWithSpec = SpecificationEvaluator.GetQuery(_dbSet.AsQueryable(), specification);

        return await queryWithSpec.CountAsync(cancellationToken);
    }
}