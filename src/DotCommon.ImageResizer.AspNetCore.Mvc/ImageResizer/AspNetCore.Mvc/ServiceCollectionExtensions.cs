using DotCommon.Caching;
using DotCommon.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DotCommon.ImageResizer.AspNetCore.Mvc
{
    /// <summary>ServiceCollection扩展
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>添加图片调整扩展
        /// </summary>
        public static IServiceCollection AddImageResizer(this IServiceCollection services, Action<ImageResizerOption> configure = null)
        {
            //配置
            var option = new ImageResizerOption();
            configure?.Invoke(option);

            //开启缓存的情况下,才需要添加缓存
            if (option.EnableImageCache)
            {
                services.AddServiceWhenNull(s => s.ServiceType == typeof(IDistributedCache<>), s =>
                {
                    s.AddGenericsMemoryCache();
                });
            }

            services
                .AddTransient<IImageResizeService, ImageResizeService>()
                .AddSingleton<ImageResizerOption>(option);
            return services;
        }

    }
}
