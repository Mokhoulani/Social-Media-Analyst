using MediatR;

namespace Application.Abstractions.Messaging;

public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, Unit>
    where TQuery : IQuery<TResponse>, IRequest<Unit>
{
}