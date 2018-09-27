using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DotCommon.Caching
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddGenericsMemoryCache(this IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddDistributedMemoryCache();
            services.AddSingleton(typeof(IDistributedCache<>), typeof(DistributedCache<>));
            return services;
        }
        public static IServiceCollection AddGenericsMemoryCache(this IServiceCollection services, Action<MemoryCacheOptions> memoryCacheOptions, Action<MemoryDistributedCacheOptions> memoryDistributedCacheOptions)
        {
            services.AddMemoryCache(memoryCacheOptions);
            services.AddDistributedMemoryCache(memoryDistributedCacheOptions);
            services.AddSingleton(typeof(IDistributedCache<>), typeof(DistributedCache<>));
            return services;
        }

    }
}
