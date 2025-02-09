using MediatR;
using Microsoft.Extensions.Caching.Hybrid;


namespace Application.Common.Behaviours;


public class CachingBehavior<TRequest, TResponse>(HybridCache cache)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        string cacheKey = $"{typeof(TRequest).Name}-{request.GetHashCode()}";
        
        
        var cachedResponse = await cache.GetOrCreateAsync<TResponse>(cacheKey,
            async token =>
        {
            return await next();
        }, cancellationToken: cancellationToken);

        return cachedResponse;
    }
}
