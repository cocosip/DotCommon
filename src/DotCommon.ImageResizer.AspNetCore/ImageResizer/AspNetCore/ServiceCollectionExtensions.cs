using DotCommon.Caching;
using DotCommon.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DotCommon.ImageResizer.AspNetCore
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

            services.AddServiceWhenNull(s => s.ServiceType == typeof(IDistributedCache<>), s =>
            {
                services.AddGenericsMemoryCache();
            });

            services
                .AddTransient<IImageResizeService, ImageResizeService>()
                .AddSingleton<ImageResizerOption>(option);
            return services;
        }

    }
}
