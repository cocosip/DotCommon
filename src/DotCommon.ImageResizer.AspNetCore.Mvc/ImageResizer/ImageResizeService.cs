using DotCommon.Caching;
using DotCommon.ImageResize;
using DotCommon.Utility;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System;
using System.Drawing;
using System.Threading.Tasks;

namespace DotCommon.ImageResizer
{
    public class ImageResizeService : IImageResizeService
    {
        private readonly IDistributedCache<ImageCacheItem> _imageCache;
        private readonly ILogger _logger;
        private readonly ImageResizerOption _option;
        public ImageResizeService(IDistributedCache<ImageCacheItem> imageCache, ILoggerFactory loggerFactory, ImageResizerOption option)
        {
            _imageCache = imageCache;
            _logger = loggerFactory.CreateLogger(DotCommonConsts.LoggerName);
            _option = option;
        }

        /// <summary>获取图片的二进制数据
        /// </summary>
        /// <param name="imagePath">图片保存路径</param>
        /// <param name="resizeParameter">图片尺寸调整参数</param>
        /// <param name="lastWriteTimeUtc">最后修改时间</param>
        /// <returns></returns>
        public async Task<byte[]> GetImageData(string imagePath, ResizeParameter resizeParameter, DateTime lastWriteTimeUtc)
        {
            try
            {
                var key = $"{imagePath}{resizeParameter.ToString()}{lastWriteTimeUtc.ToString("yyyy-MM-dd HH:mm:ss")}";
                var cacheKey = $"Image:{ShaUtil.GetStringSha1Hash(key)}";

                byte[] imageBytes;
                if (_option.EnableImageCache)
                {
                    var imageCacheItem = await _imageCache.GetAsync(cacheKey);
                    if (imageCacheItem != null)
                    {
                        return imageCacheItem.Data;
                    }
                }
                //图片操作
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
                var imageFormat = ImageUtil.GetImageFormatByFormatName(resizeParameter.Format);
                imageBytes = ImageHelper.ImageCompressToBytes(resizeImage, resizeParameter.Quality, imageFormat);
                image.Dispose();

                //只有开启图片缓存情况下,才会对图片再次进行缓存
                if (_option.EnableImageCache)
                {
                    //将图片缓存
                    await _imageCache.SetAsync(cacheKey, new ImageCacheItem(imageBytes), new DistributedCacheEntryOptions()
                    {
                        SlidingExpiration = TimeSpan.FromSeconds(_option.ImageCacheSlidingExpirationSeconds)
                    });
                }
                return imageBytes;
            }
            catch (Exception ex)
            {
                _logger.LogError("Resize图片出现错误,{0}", ex.Message);
            }
            return default(byte[]);
        }


    }
}
