using MediatR;

namespace Application.Abstractions.Messaging;

public interface ICommand : IRequest<Unit>
{
}

public interface ICommand<out TResponse> : IRequest<TResponse>
{
}
