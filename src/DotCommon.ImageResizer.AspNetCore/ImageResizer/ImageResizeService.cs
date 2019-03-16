using DotCommon.Alg;
using DotCommon.ImageResize;
using DotCommon.Logging;
using DotCommon.Utility;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Drawing;

namespace DotCommon.ImageResizer
{
    public class ImageResizeService : IImageResizeService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger _logger;
        private readonly ImageResizerOption _option;
        public ImageResizeService(IMemoryCache memoryCache, ILogger<DefaultLoggerName> logger, ImageResizerOption option)
        {
            _memoryCache = memoryCache;
            _logger = logger;
            _option = option;
        }

        /// <summary>获取图片的二进制数据
        /// </summary>
        /// <param name="imagePath">图片保存路径</param>
        /// <param name="resizeParameter">图片尺寸调整参数</param>
        /// <param name="lastWriteTimeUtc">最后修改时间</param>
        /// <returns></returns>
        public byte[] GetImageData(string imagePath, ResizeParameter resizeParameter, DateTime lastWriteTimeUtc)
        {
            try
            {
                var key = $"{imagePath}{resizeParameter.ToString()}{lastWriteTimeUtc.ToString("yyyy-MM-dd HH:mm:ss")}";
                var cacheKey = $"Image:{Sha1Alg.GetStringSha1Hash(key)}";

                byte[] imageBytes;
                if (_option.EnableImageCache)
                {
                    bool isCached = _memoryCache.TryGetValue<byte[]>(cacheKey, out imageBytes);
                    //从缓存中读取
                    if (isCached)
                    {
                        _logger.LogInformation("ImageResizer from cache,key:{0}", cacheKey);
                        return imageBytes;
                    }
                }
                //图片操作
                //读取图片
                var image = Image.FromFile(imagePath);
                //对图片进行相应的操作,放大等
                Image resizeImage = null;
                if (resizeParameter.Mode == ResizeMode.Zoom)
                {
                    resizeImage = ImageResize.ImageResizer.Zoom(image, resizeParameter);
                }
                else if (resizeParameter.Mode == ResizeMode.Crop)
                {
                    //图片裁剪
                    resizeImage = ImageResize.ImageResizer.Crop(image, resizeParameter);
                }
                if (resizeImage != null)
                {
                    var imageFormat = ImageUtil.GetImageFormatByFormatName(resizeParameter.Format);
                    imageBytes = ImageHelper.ImageCompressToBytes(resizeImage, resizeParameter.Quality, imageFormat);
                    image.Dispose();
                    //设置缓存
                    _memoryCache.Set<byte[]>(cacheKey, imageBytes, new MemoryCacheEntryOptions()
                    {
                        SlidingExpiration = TimeSpan.FromSeconds(_option.ImageCacheSlidingExpirationSeconds)
                    });
                    return imageBytes;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Resize图片出现错误,{0}", ex.Message);
            }
            return default(byte[]);
        }


    }
}
