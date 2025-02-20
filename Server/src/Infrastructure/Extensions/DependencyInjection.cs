using Application.Common.Interfaces;
using Domain.Interfaces;
using HealthChecks.Redis;
using Infrastructure.Authentication;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Redis;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using StackExchange.Redis;
using ZiggyCreatures.Caching.Fusion;
using ZiggyCreatures.Caching.Fusion.Backplane.StackExchangeRedis;
using ZiggyCreatures.Caching.Fusion.Serialization.NewtonsoftJson;

namespace Infrastructure.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureLayer(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<RedisOptions>(configuration.GetSection("Redis"));
    
        services.AddSingleton<IConnectionMultiplexer>(sp =>
        {
            var redisOptions = sp.GetRequiredService<IOptions<RedisOptions>>().Value;
            

            return ConnectionMultiplexer.Connect(new ConfigurationOptions
            {
                EndPoints = { redisOptions.ConnectionString },
                ConnectRetry = redisOptions.ConnectRetry,
                ConnectTimeout = redisOptions.ConnectTimeout,
                AbortOnConnectFail = redisOptions.AbortOnConnectFail,
                ReconnectRetryPolicy = new ExponentialRetry(5000)
            });
        });


        services.AddFusionCache()
            .WithSerializer(new FusionCacheNewtonsoftJsonSerializer())
            .WithDistributedCache(sp =>
            {
                var redisOptions = sp.GetRequiredService<IOptions<RedisOptions>>().Value;
                return new RedisCache(new RedisCacheOptions
                    { Configuration = redisOptions.ConnectionString });
            })
            .WithBackplane(sp =>
            {
                var redisOptions = sp.GetRequiredService<IOptions<RedisOptions>>().Value;
                return new RedisBackplane(new RedisBackplaneOptions 
                    { Configuration = redisOptions.ConnectionString });
            })
            .WithDefaultEntryOptions(options =>
            {
                options
                    .SetDuration(TimeSpan.FromMinutes(5))
                    .SetFailSafe(true)
                    .SetFactoryTimeouts(TimeSpan.FromSeconds(10));
            });


        services.AddHealthChecks()
            .AddCheck<RedisHealthCheck>(
                "redis", 
                failureStatus: HealthStatus.Unhealthy,
                tags: new[] { "cache", "redis" });


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