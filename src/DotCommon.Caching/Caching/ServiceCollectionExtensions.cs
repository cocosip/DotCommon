using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DotCommon.Caching
{
    /// <summary>ServiceCollection扩展方法
    /// </summary>
    public static class ServiceCollectionExtensions
    {

        /// <summary>添加分布式缓存
        /// </summary>
        public static IServiceCollection AddGenericsMemoryCache(this IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddDistributedMemoryCache();
            services.AddSingleton(typeof(IDistributedCache<>), typeof(DistributedCache<>));
            return services;
        }

        /// <summary>添加分布式缓存
        /// </summary>
        public static IServiceCollection AddGenericsMemoryCache(this IServiceCollection services, Action<MemoryCacheOptions> memoryCacheOptions, Action<MemoryDistributedCacheOptions> memoryDistributedCacheOptions)
        {
            services.AddMemoryCache(memoryCacheOptions);
            services.AddDistributedMemoryCache(memoryDistributedCacheOptions);
            services.AddSingleton(typeof(IDistributedCache<>), typeof(DistributedCache<>));
            return services;
        }

    }
}
