using MediatR;

namespace Application.Abstractions.Messaging;

public interface IQuery: IRequest<Unit>
{
}

public interface IQuery<out TResponse> : IRequest<TResponse>
{
}