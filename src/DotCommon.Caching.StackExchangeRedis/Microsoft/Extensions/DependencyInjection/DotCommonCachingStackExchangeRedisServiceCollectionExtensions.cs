using System;
using DotCommon.Caching;
using DotCommon.Caching.StackExchangeRedis;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DotCommonCachingStackExchangeRedisServiceCollectionExtensions
    {
        public static IServiceCollection AddDotCommonStackExchangeRedisCache(
            this IServiceCollection services,
            Action<RedisCacheOptions>? setupAction = null)
        {
            services.AddStackExchangeRedisCache(setupAction ?? (_ => { }));

            services.Replace(ServiceDescriptor.Singleton<IDistributedCache, DotCommonRedisCache>());

            return services;
        }

        public static IServiceCollection AddDotCommonCachingWithRedis(
            this IServiceCollection services,
            Action<RedisCacheOptions>? redisSetupAction = null)
        {
            services.AddDotCommonCaching();

            services.AddDotCommonStackExchangeRedisCache(redisSetupAction);

            return services;
        }
    }
}