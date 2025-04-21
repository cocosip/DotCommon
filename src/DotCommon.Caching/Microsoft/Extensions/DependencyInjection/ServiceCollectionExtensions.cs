using System;
using DotCommon.Caching;
using DotCommon.Caching.Hybrid;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add DotCommon Caching
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddDotCommonCaching(this IServiceCollection services)
        {
            services
                .AddTransient<IDistributedCacheSerializer, Utf8JsonDistributedCacheSerializer>()
                .AddTransient<IDistributedCacheKeyNormalizer, DistributedCacheKeyNormalizer>()
                .AddMemoryCache()
                .AddDistributedMemoryCache()
                .AddSingleton(typeof(IDistributedCache<>), typeof(DistributedCache<>))
                .AddSingleton(typeof(IDistributedCache<,>), typeof(DistributedCache<,>))
                .AddSingleton(typeof(IHybridCache<>), typeof(DotCommonHybridCache<>));


            services
                .AddHybridCache()
                .AddSerializerFactory<DotCommonHybridCacheJsonSerializerFactory>();

            services.Configure<DotCommonDistributedCacheOptions>(cacheOptions =>
            {
                cacheOptions.GlobalCacheEntryOptions.SlidingExpiration = TimeSpan.FromMinutes(20);
            });

            return services;
        }
    }
}
