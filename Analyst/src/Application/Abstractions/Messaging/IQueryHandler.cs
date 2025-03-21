using Domain.Shared;
using MediatR;

namespace Application.Abstractions.Messaging;

public interface IQueryHandler<in TQuery> : IRequestHandler<TQuery, Unit>
    where TQuery : IQuery
{
}


public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<Result<TResponse>>
{
}