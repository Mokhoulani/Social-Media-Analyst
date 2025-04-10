using Domain.Interfaces;
using Domain.Primitives;
using Microsoft.EntityFrameworkCore;

namespace Domain.Specification;

public static class SpecificationEvaluator
{
    public static IQueryable<TEntity> GetQuery<TEntity,TKey>(
        IQueryable<TEntity> inputQueryable,
        Specification<TEntity,TKey> specification)
        where TEntity : Entity<TKey>, IAggregateRoot
    {
        IQueryable<TEntity> queryable = inputQueryable;

        if (specification.Criteria is not null)
        {
            queryable = queryable.Where(specification.Criteria);
        }

        specification.IncludeExpressions.Aggregate(
            queryable,
            (current, includeExpression) =>
                current.Include(includeExpression));

        if (specification.OrderByExpression is not null)
        {
            queryable = queryable.OrderBy(specification.OrderByExpression);
        }
        else if (specification.OrderByDescendingExpression is not null)
        {
            queryable = queryable.OrderByDescending(
                specification.OrderByDescendingExpression);
        }

        if (specification.IsSplitQuery)
        {
            queryable = queryable.AsSplitQuery();
        }

        return queryable;
    }
}