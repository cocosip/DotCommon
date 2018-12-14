using DotCommon.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;

namespace DotCommon.AspNetCore.Mvc.ImageResizer
{
    public static class ImageResizerMiddlewareExtensions
    {
        public static IServiceCollection AddImageResizer(this IServiceCollection services)
        {
            services.AddServiceWhenNull(c => c.ServiceType == typeof(IMemoryCache), s =>
            {
                services.AddMemoryCache();
            });
            return services;
        }

        public static IApplicationBuilder UseImageResizer(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ImageResizerMiddleware>();
        }
    }
}
