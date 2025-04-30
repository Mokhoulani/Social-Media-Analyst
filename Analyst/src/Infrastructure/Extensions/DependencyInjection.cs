using Application.Common.Interfaces;
using HealthChecks.Redis;
using Infrastructure.Authentication;
using Infrastructure.Service;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using ZiggyCreatures.Caching.Fusion;
using Persistence.Extensions;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;


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

        services.AddAuthorization();
        services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
        services.AddSingleton<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();

        // Register Repositories & Services
        services.AddPersistenceLayer();
        services.AddSingleton<ITokenService, TokenService>();
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUser, CurrentUser>();
        return services;
    }
}