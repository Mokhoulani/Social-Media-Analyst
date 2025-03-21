using Infrastructure.OptionsSetup;
using Infrastructure.Settings;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using ZiggyCreatures.Caching.Fusion;
using ZiggyCreatures.Caching.Fusion.Backplane.StackExchangeRedis;
using ZiggyCreatures.Caching.Fusion.Serialization.NewtonsoftJson;

namespace Infrastructure.Extensions;

public static class RedisConfiguration
{
    public static void AddRedisConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureOptions<RedisOptionsSetup>();
        var redisOptions = services.BuildServiceProvider().GetRequiredService<IOptions<RedisOptions>>().Value;
    
        services.AddSingleton<IConnectionMultiplexer>(sp
            => ConnectionMultiplexer.Connect(new ConfigurationOptions
        {
            EndPoints = { redisOptions.ConnectionString },
            ConnectRetry = redisOptions.ConnectRetry,
            ConnectTimeout = redisOptions.ConnectTimeout,
            AbortOnConnectFail = redisOptions.AbortOnConnectFail,
            ReconnectRetryPolicy = new ExponentialRetry(5000)
        }));


        services.AddFusionCache()
            .WithSerializer(new FusionCacheNewtonsoftJsonSerializer())
            .WithDistributedCache(sp
                => new RedisCache(new RedisCacheOptions
                { Configuration = redisOptions.ConnectionString })
            )
            .WithBackplane(sp 
                => new RedisBackplane(new RedisBackplaneOptions 
                { Configuration = redisOptions.ConnectionString })
            )
            .WithDefaultEntryOptions(options =>
            {
                options
                    .SetDuration(TimeSpan.FromMinutes(5))
                    .SetFailSafe(true)
                    .SetFactoryTimeouts(TimeSpan.FromSeconds(10));
            });
        
        services.AddHealthChecks().AddRedis(redisOptions.ConnectionString);
    }
}