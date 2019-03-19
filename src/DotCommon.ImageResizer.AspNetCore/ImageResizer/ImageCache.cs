using System;

namespace DotCommon.ImageResizer
{
    [Serializable]
    public class ImageCacheItem
    {
        public byte[] Data { get; set; }

        public ImageCacheItem()
        {

        }

        public ImageCacheItem(byte[] data)
        {
            Data = data;
        }
    }
}
