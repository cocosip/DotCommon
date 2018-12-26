using DotCommon.ImageResize;
using DotCommon.Logging;
using DotCommon.Utility;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DotCommon.ImageResizer.AspNetCore
{
    /// <summary>图片缩放中间件
    /// </summary>
    public class ImageResizerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private readonly IHostingEnvironment _env;
        private readonly IImageResizeService _imageResizeService;
        public ImageResizerMiddleware(RequestDelegate next, IHostingEnvironment env, ILogger<DefaultLoggerName> logger, IImageResizeService imageResizeService)
        {
            _next = next;
            _env = env;
            _logger = logger;
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
            var resizeParameter = GetResizeParameter(path, context.Request.Query);
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
            byte[] imageBytes = _imageResizeService.GetImageData(imagePath, resizeParameter, lastWriteTimeUtc);
            //无法调整图片,那么就直接返回下一个
            if (imageBytes == null || imageBytes.Length <= 0)
            {
                await _next.Invoke(context);
                return;
            }

            //contentType
            context.Response.ContentType = MimeTypeNameUtil.GetMimeName(resizeParameter.Format);
            context.Response.ContentLength = imageBytes.Length;
            await context.Response.Body.WriteAsync(imageBytes.ToArray(), 0, (int)imageBytes.Length);
        }

        /// <summary>获取请求参数
        /// </summary>
        private ResizeParameter GetResizeParameter(PathString path, IQueryCollection query)
        {
            ResizeParameter resizeParameter = new ResizeParameter();
            //Format
            resizeParameter.Format = query.ContainsKey("format") ? query["format"].ToString() : ImageUtil.DefaultFormatName();

            //自动旋转
            bool autoRotate = false;
            if (query.ContainsKey("autorotate"))
            {
                bool.TryParse(query["autorotate"], out autoRotate);
            }
            resizeParameter.AutoRotate = autoRotate;

            int quality = 100;
            if (query.ContainsKey("quality"))
            {
                int.TryParse(query["quality"], out quality);
            }
            resizeParameter.Quality = quality;

            int w = 0;
            if (query.ContainsKey("w"))
            {
                int.TryParse(query["w"], out w);
            }
            resizeParameter.Width = w;

            int h = 0;
            if (query.ContainsKey("h"))
            {
                int.TryParse(query["h"], out h);
            }
            resizeParameter.Height = h;
            resizeParameter.Mode = ResizeMode.Zoom;

            int x = 0;
            int y = 0;
            //CropX
            if (query.ContainsKey("x"))
            {
                int.TryParse(query["x"], out x);
            }
            resizeParameter.CropX = x;
            //CropY
            if (query.ContainsKey("y"))
            {
                int.TryParse(query["y"], out y);
            }
            resizeParameter.CropY = y;

            //Mode
            if ((h != 0 || w != 0) && query.ContainsKey("mode") && ResizeMode.Modes.Any(m => string.Equals(m, query["mode"], StringComparison.OrdinalIgnoreCase)))
            {
                if (string.Equals(query["mode"], ResizeMode.Crop))
                {
                    if (resizeParameter.CropX > 0 && resizeParameter.CropY > 0)
                    {
                        resizeParameter.Mode = ResizeMode.Crop;
                    }
                }

                resizeParameter.Mode = query["mode"];
            }



            return resizeParameter;
        }


        /// <summary>是否为图片地址
        /// </summary>
        private bool IsImagePath(PathString path)
        {
            if (path == null || !path.HasValue)
            {
                return false;
            }
            var extension = PathUtil.GetPathExtension(path);
            return ImageUtil.IsImageExtension(extension);
        }
    }
}
