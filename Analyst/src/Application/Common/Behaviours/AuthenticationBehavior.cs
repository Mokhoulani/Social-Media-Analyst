using System.Reflection;
using Domain.Errors;
using MediatR;
using Domain.Shared;
using Microsoft.AspNetCore.Authorization;
using Domain.Interfaces;
using Domain.Rules.AuthenticationRules;
using Domain.Rules.AuthorizationRules;
using Domain.Shared.ResultTypes.AuthenticationResult;
using Domain.Shared.ResultTypes.AuthorizationResult;
using AuthorizationResult = Domain.Shared.ResultTypes.AuthorizationResult.AuthorizationResult;

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

        var ruleAuthenticated = new MustBeAuthenticatedRule(currentUser);

        if (ruleAuthenticated.IsBroken())
        {
            var failureResult =
                CreateAuthenticatedResult<TResponse>(ruleAuthenticated.Error.Code, ruleAuthenticated.Error.Message);
            return failureResult;
        }

        var authorizeAttributes = requestType.GetCustomAttributes<AuthorizeAttribute>(inherit: true);

        foreach (var attribute in authorizeAttributes)
        {
            if (!string.IsNullOrWhiteSpace(attribute.Roles))
            {
                var requiredRoles = attribute.Roles.Split(',');
                if (!requiredRoles.Any(currentUser.IsInRole))
                {
                    var failureResult = CreateAuthenticatedResult<TResponse>(
                        AuthorizationErrors.Forbidden("Missing required role").Code,
                        "User does not have the required role.");
                    return failureResult;
                }
            }

            if (!string.IsNullOrWhiteSpace(attribute.Policy))
            {
                // If your policies map to permissions, do something like:
                var rule = new MustBeAuthorizationRule(currentUser, attribute.Policy);
                if (rule.IsBroken())
                {
                    var failureResult = CreateAuthorizedResult<TResponse>(
                        rule.Error.Code,
                        rule.Error.Message);
                    return failureResult;
                }
            }
        }
        return await next();
    }

    private TResult CreateAuthenticatedResult<TResult>(string code, string message) where TResult : Result
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
    
    private TResult CreateAuthorizedResult<TResult>(string code, string message) where TResult : Result
    {
        if (typeof(TResult) == typeof(Result))
        {
            return (AuthorizationResult.Authorized(code, message) as TResult)!;
        }

        var authenticatedResult = typeof(AuthorizationResult<>)
            .GetGenericTypeDefinition()
            .MakeGenericType(typeof(TResult).GenericTypeArguments[0])
            .GetMethod(nameof(AuthorizationResult.Authorized))!
            .Invoke(null, [code, message])!;
            
        return (TResult)authenticatedResult;
    }
    
    
}