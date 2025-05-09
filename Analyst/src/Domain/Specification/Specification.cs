using System.Linq.Expressions;
using Domain.Interfaces;
using Domain.Primitives;
using Microsoft.EntityFrameworkCore.Query;

namespace Domain.Specification;

public abstract class Specification<TEntity, TKey>(Expression<Func<TEntity, bool>>? criteria = null)
    where TEntity : Entity<TKey>, IAggregateRoot
    where TKey : notnull
{
    public bool IsSplitQuery { get; protected set; }

    public Expression<Func<TEntity, bool>>? Criteria { get; } = criteria;

    public List<Expression<Func<TEntity, object>>> IncludeExpressions { get; } = new();

    public List<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>> IncludeNavigations { get; } = new();

    public Expression<Func<TEntity, object>>? OrderByExpression { get; private set; }

    public Expression<Func<TEntity, object>>? OrderByDescendingExpression { get; private set; }

    public LambdaExpression? Selector { get; private set; }

    protected void AddInclude(Expression<Func<TEntity, object>> includeExpression)
    {
        IncludeExpressions.Add(includeExpression);
    }

    protected void AddIncludeNavigation(
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includeNavigation)
    {
        IncludeNavigations.Add(includeNavigation);
    }

    protected void AddOrderBy(Expression<Func<TEntity, object>> orderByExpression)
    {
        OrderByExpression = orderByExpression;
    }

    protected void AddOrderByDescending(Expression<Func<TEntity, object>> orderByDescendingExpression)
    {
        OrderByDescendingExpression = orderByDescendingExpression;
    }

    protected void AddSelector<TOut>(Expression<Func<TEntity, TOut>> selector)
    {
        Selector = selector;
    }
}