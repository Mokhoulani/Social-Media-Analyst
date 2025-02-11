using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Domain.Interfaces;
using Domain.Primitives;

namespace Infrastructure.Persistence.Repositories;

public class Repository<T> : IRepository<T> where T : Entity, IAggregateRoot
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly DbSet<T> _dbSet;
    private readonly IUnitOfWork _unitOfWork;

    public Repository(ApplicationDbContext applicationDbContext, IUnitOfWork unitOfWork)
    {
      _applicationDbContext = applicationDbContext;
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _dbSet = _applicationDbContext.Set<T>();
    }

    public async Task<T> AddAsync(T entity, CancellationToken cancellationToken)
    {
        await _dbSet.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _dbSet.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<TResult?> GetByIndexAsync<TIndex, TResult>(
        Expression<Func<T, TIndex>> indexSelector, 
        TIndex index, 
        CancellationToken cancellationToken)
    {
        var lambda = Expression.Lambda<Func<T, bool>>(
            Expression.Equal(indexSelector.Body, Expression.Constant(index)), 
            indexSelector.Parameters
        );

        return await _dbSet
            .Where(lambda)
            .OfType<TResult>()
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IList<T>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _dbSet.ToListAsync(cancellationToken);
    }

    public async Task UpdateAsync(T entity, CancellationToken cancellationToken)
    {
        var existingEntity = await _dbSet.FindAsync(new object[] { entity.Id }, cancellationToken);
        
        if (existingEntity == null)
            throw new KeyNotFoundException($"Entity {typeof(T).Name} with ID {entity.Id} not found.");

        _applicationDbContext.Entry(existingEntity).CurrentValues.SetValues(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var entity = await _dbSet.FindAsync(new object[] { id }, cancellationToken);
        if (entity == null)
            throw new KeyNotFoundException($"Entity {typeof(T).Name} with ID {id} not found.");

        _dbSet.Remove(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
