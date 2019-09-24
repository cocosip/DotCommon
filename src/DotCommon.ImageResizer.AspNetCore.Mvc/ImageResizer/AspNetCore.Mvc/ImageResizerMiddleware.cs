using DotCommon.ImageResize;
using DotCommon.Utility;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DotCommon.ImageResizer.AspNetCore.Mvc
{
    /// <summary>图片缩放中间件
    /// </summary>
    public class ImageResizerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private readonly IWebHostEnvironment _env;
        private readonly IImageResizeService _imageResizeService;
        
        /// <summary>Ctor
        /// </summary>
        public ImageResizerMiddleware(RequestDelegate next, IWebHostEnvironment env, ILoggerFactory loggerFactory, IImageResizeService imageResizeService)
        {
            _next = next;
            _env = env;
            _logger = loggerFactory.CreateLogger(DotCommonConsts.LoggerName);
            _imageResizeService = imageResizeService;
        }

        /// <summary>Invoke
        /// </summary>
        public async Task Invoke(HttpContext context)
        {
            var path = context.Request.Path;
            // hand to next middleware if we are not dealing with an image
            if (context.Request.Query.Count == 0 || !IsImagePath(path))
            {
                await _next.Invoke(context);
                return;
            }
            //图片需要的参数
            var resizeParameter = GetResizeParameter(context.Request.Query);
            //无参数,不缩放
            if (!resizeParameter.HasParams())
            {
                await _next.Invoke(context);
                return;
            }

            //图片路径
            var imagePath = Path.Combine(
                _env.WebRootPath,
                path.Value.Replace('/', Path.DirectorySeparatorChar).TrimStart(Path.DirectorySeparatorChar));
            //图片不存在
            if (!File.Exists(imagePath))
            {
                await _next.Invoke(context);
                return;
            }
            //图片最后修改时间
            var lastWriteTimeUtc = File.GetLastWriteTimeUtc(imagePath);
            //图片二进制
            byte[] imageBytes = await _imageResizeService.GetImageData(imagePath, resizeParameter, lastWriteTimeUtc);
            //无法调整图片,那么就直接返回下一个
            if (imageBytes == null || imageBytes.Length <= 0)
            {
                await _next.Invoke(context);
                return;
            }

            //contentType
            context.Response.ContentType = MimeTypeNameUtil.GetMimeName(resizeParameter.Format);
            context.Response.ContentLength = imageBytes.Length;
            await context.Response.Body.WriteAsync(imageBytes.ToArray(), 0, imageBytes.Length);
        }

        /// <summary>获取请求参数
        /// </summary>
        private ResizeParameter GetResizeParameter(IQueryCollection query)
        {
            ResizeParameter resizeParameter = new ResizeParameter
            {
                //Format
                Format = GetQueryValue(query, "format", ImageUtil.DefaultFormatName),
                //自动旋转
                AutoRotate = bool.Parse(GetQueryValue(query, "autorotate", "false")),
                //质量
                Quality = int.Parse(GetQueryValue(query, "q", "100")),
                //宽度
                Width = int.Parse(GetQueryValue(query, "w", "0")),
                //高度
                Height = int.Parse(GetQueryValue(query, "h", "0")),
                //模式
                Mode = GetQueryValue(query, "mode", ResizeMode.Zoom),
                //x
                CropX = int.Parse(GetQueryValue(query, "x", "0")),
                //y
                CropY = int.Parse(GetQueryValue(query, "y", "0"))
            };

            return resizeParameter;
        }

        private string GetQueryValue(IQueryCollection query, string name, string defaultValue = "")
        {
            if (query.TryGetValue(name, out StringValues value))
            {
                return value.ToString();
            }
            return defaultValue;
        }


        /// <summary>是否为图片地址
        /// </summary>
        private bool IsImagePath(PathString path)
        {
            _logger.LogInformation("非有效的图片路径,将不会进行图片的缩放");
            if (!path.HasValue)
            {
                return false;
            }
            var extension = PathUtil.GetPathExtension(path);
            return ImageUtil.IsImageExtension(extension);
        }
    }
}
