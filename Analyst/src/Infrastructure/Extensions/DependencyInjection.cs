using Application.Common.Interfaces;
using Domain.Interfaces;
using HealthChecks.Redis;
using Infrastructure.Authentication;
using Persistence.Repositories;
using Persistence.Persistence;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using ZiggyCreatures.Caching.Fusion;


namespace Infrastructure.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureLayer(
        this IServiceCollection services,
        IConfiguration configuration)
    {
       services.AddDatabaseConfiguration(configuration);
       services.AddRedisConfiguration(configuration);
       
        services.AddSingleton<RedisHealthCheck>();
        services.AddSingleton<IFusionCache, FusionCache>();

        // Register Repositories & Services
        services.AddScoped<Dictionary<Type, object>>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddSingleton<ITokenService, TokenService>();

        return services;
    }
}