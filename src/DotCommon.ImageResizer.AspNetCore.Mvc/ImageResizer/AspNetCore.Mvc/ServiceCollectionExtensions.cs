using DotCommon.Caching;
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

            services
                .AddTransient<IImageResizeService, ImageResizeService>()
                .AddSingleton<ImageResizerOption>(option);
            return services;
        }

    }
}
