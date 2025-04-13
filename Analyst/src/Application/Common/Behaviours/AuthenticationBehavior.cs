using System.Reflection;
using MediatR;
using Domain.Shared;
using Microsoft.AspNetCore.Authorization;
using Domain.Interfaces;
using Domain.Rules.AuthenticationRules;

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

    private TResponse CreateFailureResult<T>(string errorCode, string errorMessage) where T : Result
    {
        var genericType = typeof(TResponse).GetGenericArguments().FirstOrDefault();

        if (genericType == null)
        {
            return (TResponse)(object)Result.Failure(new Error(errorCode, errorMessage));
        }

        var failureMethod = typeof(Result).GetMethods()
            .FirstOrDefault(m => m is { Name: nameof(Result.Failure), IsGenericMethod: true });

        if (failureMethod == null)
        {
            throw new InvalidOperationException("Could not find the Failure method for type: " + typeof(TResponse));
        }

        var genericFailureMethod = failureMethod.MakeGenericMethod(genericType);

        var result = genericFailureMethod.Invoke(null, new object[] { new Error(errorCode, errorMessage) });

        return ((TResponse)result!)!;
    }
}