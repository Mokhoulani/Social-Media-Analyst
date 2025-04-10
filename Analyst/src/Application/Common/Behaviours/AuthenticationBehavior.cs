using System.Reflection;
using Application.Common.Interfaces;
using MediatR;
using Domain.Shared;
using Microsoft.AspNetCore.Authorization;

namespace Application.Common.Behaviours;

public sealed class AuthenticationBehavior<TRequest, TResponse>(ICurrentUser currentUser)
    : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse> where TResponse : Result
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var requestType = typeof(TRequest);
        var allowAnonymous = requestType.GetCustomAttribute<AllowAnonymousAttribute>() != null;

        if (allowAnonymous)
            return await next();

        if (currentUser.IsAuthenticated && !string.IsNullOrWhiteSpace(currentUser.UserId))
            return await next();
        
        var failureResult = CreateFailureResult<TResponse>("Authentication", "User is not authenticated.");
        return failureResult;
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