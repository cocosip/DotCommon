using DotCommon.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace DotCommon.ImageResizer.AspNetCore
{
    /// <summary>图片缩放中间件
    /// </summary>
    public class ImageResizerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private readonly IHostingEnvironment _env;
        private readonly IMemoryCache _memoryCache;
        public ImageResizerMiddleware(RequestDelegate next, IHostingEnvironment env, ILogger<DefaultLoggerName> logger, IMemoryCache memoryCache)
        {
            _next = next;
            _env = env;
            _logger = logger;
            _memoryCache = memoryCache;
        }
    }
}