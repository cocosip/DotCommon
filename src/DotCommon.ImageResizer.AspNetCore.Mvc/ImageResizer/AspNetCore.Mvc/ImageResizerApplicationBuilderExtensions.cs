using Microsoft.AspNetCore.Builder;

namespace DotCommon.ImageResizer.AspNetCore.Mvc
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
