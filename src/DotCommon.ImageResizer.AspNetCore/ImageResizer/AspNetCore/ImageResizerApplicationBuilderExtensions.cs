using DotCommon.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DotCommon.ImageResizer.AspNetCore
{
    /// <summary>图片调整扩展
    /// </summary>
    public static class ImageResizerApplicationBuilderExtensions
    {
     
        /// <summary>使用Middleware
        /// </summary>
        public static IApplicationBuilder UseImageResizer(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ImageResizerMiddleware>();
        }
    }
}
