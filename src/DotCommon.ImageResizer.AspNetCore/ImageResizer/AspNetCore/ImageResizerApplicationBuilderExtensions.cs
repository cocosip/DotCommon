using DotCommon.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;

namespace DotCommon.ImageResizer.AspNetCore
{
    /// <summary>图片调整扩展
    /// </summary>
    public static class ImageResizerApplicationBuilderExtensions
    {
        /// <summary>添加图片调整扩展
        /// </summary>
        public static IServiceCollection AddImageResizer(this IServiceCollection services)
        {
            //添加缓存
            services.AddServiceWhenNull(s => s.ServiceType == typeof(IMemoryCache), s =>
            {
                s.AddMemoryCache();
            });

            return services;
        }

        /// <summary>使用Middleware
        /// </summary>
        public static IApplicationBuilder UseImageResizer(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ImageResizerMiddleware>();
        }
    }
}
