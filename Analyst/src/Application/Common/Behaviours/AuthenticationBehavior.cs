using System.Reflection;
using MediatR;
using Domain.Shared;
using Microsoft.AspNetCore.Authorization;
using Domain.Interfaces;
using Domain.Rules.AuthenticationRules;
using Domain.Shared.ResultTypes.AuthenticationResult;

namespace Application.Common.Behaviours;

public sealed class AuthenticationBehavior<TRequest, TResponse>(ICurrentUser currentUser)
    : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse> where TResponse : Result
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var requestType = typeof(TRequest);
        var allowAnonymous = requestType.GetCustomAttribute<AllowAnonymousAttribute>() != null;

        if (allowAnonymous) return await next();

        var rule = new MustBeAuthenticatedRule(currentUser);

        if (rule.IsBroken())
        {
            var failureResult = CreateFailureResult<TResponse>(rule.Error.Code, rule.Error.Message);
            return failureResult;
        }

        return await next();
    }

    private TResult CreateFailureResult<TResult>(string code, string message) where TResult : Result
    {
        if (typeof(TResult) == typeof(Result))
        {
            return (AuthenticationResult.Unauthenticated(code, message) as TResult)!;
        }

        var authenticatedResult = typeof(AuthenticationResult<>)
            .GetGenericTypeDefinition()
            .MakeGenericType(typeof(TResult).GenericTypeArguments[0])
            .GetMethod(nameof(AuthenticationResult.Unauthenticated))!
            .Invoke(null, [code, message])!;

        return (TResult)authenticatedResult;
    }
}