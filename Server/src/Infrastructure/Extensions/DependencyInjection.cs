using System;
using System.Collections.Generic;
using Application.Common.Interfaces;
using Domain.Interfaces;
using Infrastructure.Authentication;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;

using Microsoft.Extensions.DependencyInjection;


namespace Infrastructure.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureLayer(
        this IServiceCollection services)
    {
        services.AddScoped<Dictionary<Type, object>>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddSingleton<ITokenService, TokenService>();
        return services;
    }
}
