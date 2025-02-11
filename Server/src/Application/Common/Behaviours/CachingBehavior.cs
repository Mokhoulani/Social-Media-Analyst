using System.Text.Json;
using MediatR;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;


namespace Application.Common.Behaviours;

public class CachingBehavior<TRequest, TResponse>(HybridCache cache, ILogger<CachingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        // Generate a deterministic cache key
        string cacheKey = $"{typeof(TRequest).Name}-{JsonSerializer.Serialize(request)}";

        try
        {
            logger.LogInformation("Checking cache for key: {CacheKey}", cacheKey);

            var cachedResponse = await cache.GetOrCreateAsync<TResponse>(
                cacheKey,
                async token =>
                {
                    logger.LogInformation("Cache miss for key: {CacheKey}", cacheKey);
                    return await next();
                },
                cancellationToken: cancellationToken);

            logger.LogInformation("Cache hit for key: {CacheKey}", cacheKey);
            return cachedResponse;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while handling caching for request: {Request}", typeof(TRequest).Name);
            throw;
        }
    }
}