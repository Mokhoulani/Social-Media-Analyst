using Application.Abstractions.Messaging;
using Application.Cache;
using MediatR;
using ZiggyCreatures.Caching.Fusion;


namespace Application.Common.Behaviours;

public class CacheBehavior<TRequest, TResponse>(IFusionCache cache) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IQuery<TResponse> 
{
    public async Task<TResponse> Handle(
        TRequest request, 
        RequestHandlerDelegate<TResponse> next, 
        CancellationToken cancellationToken)
    {
        
        if (typeof(TRequest)
                .GetCustomAttributes(typeof(CacheAttribute), false)
                .FirstOrDefault() is not CacheAttribute cacheAttribute)
        {
            return await next(); 
        }

        var cacheKey = GenerateCacheKey(request, cacheAttribute);
        
        return await cache.GetOrSetAsync(
            cacheKey,
            async (token) => await next(),
            options => options
                .SetDuration(TimeSpan.FromSeconds(cacheAttribute.DurationInSeconds))
                .SetFailSafe(true)
                .SetFactoryTimeouts(TimeSpan.FromSeconds(5)),
            token: cancellationToken);
    }
    private static string GenerateCacheKey(TRequest request, CacheAttribute cacheAttribute)
    {
        return $"{typeof(TRequest).Name}:{string.Format(cacheAttribute.CacheKeyTemplate, request)}";
    }
}
