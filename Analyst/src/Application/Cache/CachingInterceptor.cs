// using System.Reflection;
// using Castle.DynamicProxy;
// using ZiggyCreatures.Caching.Fusion;
//
// namespace Application.Cache;
//
// public abstract class CachingInterceptor : IInterceptor
// {
//     private readonly IFusionCache _cache;
//
//     public CachingInterceptor(IFusionCache cache)
//     {
//         _cache = cache;
//     }
//
//     public void Intercept(IInvocation invocation)
//     {
//         var method = invocation.MethodInvocationTarget ?? invocation.Method;
//         var cacheAttribute = method.GetCustomAttribute<CacheAttribute>();
//
//         if (cacheAttribute == null)
//         {
//             invocation.Proceed(); // No caching, just execute the method
//             return;
//         }
//
//         // Generate cache key
//         var cacheKey = cacheAttribute.GetCacheKey(invocation.Arguments);
//         var cacheDuration = TimeSpan.FromSeconds(cacheAttribute.DurationInSeconds);
//
//         // Try to get the cached result
//         var maybeValue = _cache.TryGet<object>(cacheKey);
//         if (maybeValue.HasValue)
//         {
//             // If cache hit, return the cached value
//             invocation.ReturnValue = maybeValue.Value;
//             return;
//         }
//
//         // Cache miss - proceed with method execution
//         invocation.Proceed();
//
//         // Cache the result if it's not null
//         if (invocation.ReturnValue != null)
//         {
//             _cache.Set(cacheKey, invocation.ReturnValue, cacheDuration);
//         }
//     }
// }
