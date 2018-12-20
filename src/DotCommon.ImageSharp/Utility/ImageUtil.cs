using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace DotCommon.Utility
{
    /// <summary>图片的相关操作
    /// </summary>
    public class ImageUtil
    {
        /// <summary>获取图片格式的编码
        /// </summary>
        /// <param name="format">图片格式</param>
        /// <returns></returns>
        public static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }

        /// <summary>获取当前图片的编码
        /// </summary>
        public static ImageCodecInfo GetEncoder(Image image)
        {
            return GetEncoder(image.RawFormat);
        }

        /// <summary>根据扩展名获取图片的格式
        /// </summary>
        public static ImageFormat GetImageFormatByExtension(string extension)
        {
            if (string.Equals(extension, ".jpg", StringComparison.OrdinalIgnoreCase) || string.Equals(extension, ".jpeg", StringComparison.OrdinalIgnoreCase))
            {
                return ImageFormat.Jpeg;
            }
            if (string.Equals(extension, ".png", StringComparison.OrdinalIgnoreCase))
            {
                return ImageFormat.Png;
            }
            if (string.Equals(extension, ".gif", StringComparison.OrdinalIgnoreCase))
            {
                return ImageFormat.Gif;
            }
            if (string.Equals(extension, ".ico", StringComparison.OrdinalIgnoreCase) || string.Equals(extension, ".icon", StringComparison.OrdinalIgnoreCase))
            {
                return ImageFormat.Icon;
            }
            if (string.Equals(extension, ".bmp", StringComparison.OrdinalIgnoreCase))
            {
                return ImageFormat.Bmp;
            }
            return ImageFormat.Jpeg;
        }

        /// <summary>根据图片格式获取扩展名
        /// </summary>
        public static string GetExtensionByImageFormat(ImageFormat format)
        {
            if (format.Equals(ImageFormat.Jpeg))
            {
                return ".jpeg";
            }
            if (format.Equals(ImageFormat.Png))
            {
                return ".png";
            }
            if (format.Equals(ImageFormat.Gif))
            {
                return ".gif";
            }
            if (format.Equals(ImageFormat.Icon))
            {
                return ".icon";
            }

            if (format.Equals(ImageFormat.Bmp))
            {
                return ".bmp";
            }

            return "";
        }



    }
}
