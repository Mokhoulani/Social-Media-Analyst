using Application.Common.Interfaces;
using Domain.Interfaces;
using HealthChecks.Redis;
using Infrastructure.Authentication;
using Infrastructure.Notifications;
using Infrastructure.OptionsSetup;
using Infrastructure.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Extensions;
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

        services.AddAuthorization();
        services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
        services.AddSingleton<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();

        services.ConfigureOptions<JwtOptionsSetup>();
        services.ConfigureOptions<RefreshTokenOptionsSetup>();
        services.ConfigureOptions<JwtBearerOptionsSetup>();
        services.ConfigureOptions<FirebaseOptionsSetup>();


        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer();

        // Register Repositories & Services
        services.AddPersistenceLayer();
        services.AddSingleton<ITokenService, TokenService>();
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUser, CurrentUser>();

        services.AddSingleton<IFirebaseNotificationService, FirebaseNotificationService>();
        return services;
    }
}