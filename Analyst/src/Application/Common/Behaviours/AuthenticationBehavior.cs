using System.Reflection;
using Domain.Errors;
using MediatR;
using Domain.Shared;
using Microsoft.AspNetCore.Authorization;
using Domain.Interfaces;
using Domain.Rules.AuthenticationRules;
using Domain.Rules.AuthorizationRules;
using Domain.Shared.Extensions;

namespace Application.Common.Behaviours;

public sealed class AuthenticationBehavior<TRequest, TResponse>(
    ICurrentUser currentUser)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : Result
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var requestType = typeof(TRequest);
        var allowAnonymous = requestType.GetCustomAttribute<AllowAnonymousAttribute>() != null;

        if (allowAnonymous)
            return await next();

        var ruleAuthenticated = new MustBeAuthenticatedRule(currentUser);

        if (ruleAuthenticated.IsBroken())
        {
            return (ruleAuthenticated.Error.Code, ruleAuthenticated.Error.Message)
                .AsAuthenticationFailure<TResponse>();
        }

        var authorizeAttributes = requestType.GetCustomAttributes<AuthorizeAttribute>(inherit: true);

        foreach (var attribute in authorizeAttributes)
        {
            if (!string.IsNullOrWhiteSpace(attribute.Roles))
            {
                var requiredRoles = attribute.Roles.Split(',');

                if (!requiredRoles.Any(currentUser.IsInRole))
                {
                    var error = AuthorizationErrors.Forbidden("Missing required role");
                    return error.AsAuthorizationFailure<TResponse>();
                }
            }

            if (!string.IsNullOrWhiteSpace(attribute.Policy))
            {
                var rule = new MustBeAuthorizationRule(currentUser,
                    attribute.Policy);
                if (rule.IsBroken())
                {
                    return rule.Error.AsAuthorizationFailure<TResponse>();
                }
            }
        }

        return await next();
    }
}