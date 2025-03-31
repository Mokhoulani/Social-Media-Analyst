using Application.Common.Interfaces;
using Domain.Interfaces;
using HealthChecks.Redis;
using Infrastructure.Authentication;
using Persistence.Persistence;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Persistence.Persistence.Repositories;
using ZiggyCreatures.Caching.Fusion;
using Persistence.Extensions;


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
        services.AddPersistenceLayer();
        services.AddSingleton<ITokenService, TokenService>();

        return services;
    }
}