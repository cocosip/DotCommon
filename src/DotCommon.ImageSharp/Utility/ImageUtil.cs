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
        public static ImageCodecInfo GetImageCodecInfo(ImageFormat format)
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
        public static ImageCodecInfo GetImageCodecInfo(Image image)
        {
            return GetImageCodecInfo(image.RawFormat);
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

        /// <summary>根据质量获取EncoderParameters参数
        /// </summary>
        public static EncoderParameters GetEncoderParametersByQuality(int quality)
        {
            Encoder qualityEncoder = Encoder.Quality;
            EncoderParameters encoderParameters = new EncoderParameters(1);
            EncoderParameter qualityEncoderParameter = new EncoderParameter(qualityEncoder, quality);
            encoderParameters.Param[0] = qualityEncoderParameter;
            return encoderParameters;
        }

        /// <summary>以指定的质量将图片保存到指定位置
        /// </summary>
        /// <param name="image">图片</param>
        /// <param name="quality">图片质量</param>
        /// <param name="savePath">保存路径</param>
        public static void SaveByQuality(Image image, int quality, string savePath)
        {
            var encoderParameters = GetEncoderParametersByQuality(quality);
            var extension = PathUtil.GetPathExtension(savePath);
            //图片格式
            var imageFormat = GetImageFormatByExtension(extension);
            //图片编码信息
            var imageCodecInfo = GetImageCodecInfo(imageFormat);
            image.Save(savePath, imageCodecInfo, encoderParameters);
        }

        /// <summary>根据颜色字符串获取颜色
        /// </summary>
        public static Color GetColor(string color)
        {
            switch (color.ToLower())
            {
                case "white":
                    return Color.White;
                case "black":
                    return Color.Black;
                case "red":
                    return Color.Red;
                case "blue":
                    return Color.Blue;
                case "yellow":
                    return Color.Yellow;
                default:
                    //透明
                    return Color.Transparent;
            }
        }

    }
}
