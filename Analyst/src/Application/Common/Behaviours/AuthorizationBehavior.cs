using System.Reflection;
using Application.Common.Interfaces;
using MediatR;
using Domain.Shared;
using Microsoft.AspNetCore.Authorization;

namespace Application.Common.Behaviours;

public sealed class AuthorizationBehavior<TRequest, TResponse>(
    ICurrentUser currentUser,
    IAuthorizationService authorizationService)
    : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse> where TResponse : Result
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var requestType = typeof(TRequest);

        var allowAnonymous = requestType.GetCustomAttribute<AllowAnonymousAttribute>() != null;
        if (allowAnonymous) return await next();

        var authorizeAttributes = requestType.GetCustomAttributes<AuthorizeAttribute>(inherit: true);
        var attributes = authorizeAttributes.ToList();
        if (!attributes.Any()) return await next();

        foreach (var attribute in attributes)
        {
            if (!string.IsNullOrWhiteSpace(attribute.Policy))
            {
                var result = await authorizationService.AuthorizeAsync(
                    currentUser.ClaimsPrincipal, request, attribute.Policy);

                if (!result.Succeeded)
                {
                    return CreateFailureResult<TResponse>("Authorization",
                        $"User does not have the required permission: {attribute.Policy}");
                }
            }

            if (string.IsNullOrWhiteSpace(attribute.Roles)) continue;

            var roles = attribute.Roles.Split(',');
            var hasRole = roles.Any(role => currentUser.IsInRole(role.Trim()));

            if (!hasRole)
            {
                return CreateFailureResult<TResponse>("Authorization",
                    $"User does not have one of the required roles: {attribute.Roles}");
            }
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
            .FirstOrDefault(m => m.Name == nameof(Result.Failure) && m.IsGenericMethod);

        if (failureMethod == null)
        {
            throw new InvalidOperationException("Could not find the Failure method for type: " + typeof(TResponse));
        }

        var genericFailureMethod = failureMethod.MakeGenericMethod(genericType);

        var result = genericFailureMethod.Invoke(null, new object[] { new Error(errorCode, errorMessage) });

        return ((TResponse)result!)!;
    }
}