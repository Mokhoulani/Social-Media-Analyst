using Microsoft.Extensions.DependencyInjection;
using Application.CQRS.User.Commands;
using Application.CQRS.User.Handlers;
using FluentValidation;
using MediatR;

namespace Application.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
    {
        // Register MediatR
        services.AddMediatR(typeof(CreateUserCommandHandler).Assembly);
        services.AddMediatR(typeof(GetUserByIdQueryHandler).Assembly);

        // Register FluentValidation
        services.AddValidatorsFromAssembly(typeof(CreateUserValidator).Assembly);

        return services;
    }
}
