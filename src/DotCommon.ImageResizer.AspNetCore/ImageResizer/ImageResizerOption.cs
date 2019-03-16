namespace DotCommon.ImageResizer
{
    /// <summary>图片调整配置
    /// </summary>
    public class ImageResizerOption
    {
        /// <summary>是否开启图片缓存配置
        /// </summary>
        public bool EnableImageCache { get; set; }

        /// <summary>图片默认过期时间为 300(s)
        /// </summary>
        public int ImageCacheSlidingExpirationSeconds { get; set; } = 300;

    }
}
