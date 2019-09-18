using System;

namespace DotCommon.ImageResizer
{
    /// <summary>图片缓存
    /// </summary>
    [Serializable]
    public class ImageCacheItem
    {
        /// <summary>缓存数据
        /// </summary>
        public byte[] Data { get; set; }

        /// <summary>Ctor
        /// </summary>
        public ImageCacheItem()
        {

        }

        /// <summary>Ctor
        /// </summary>
        public ImageCacheItem(byte[] data)
        {
            Data = data;
        }
    }
}
