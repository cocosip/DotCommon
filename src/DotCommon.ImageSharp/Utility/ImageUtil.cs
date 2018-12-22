using DotCommon.Extensions;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace DotCommon.Utility
{
    /// <summary>图片的相关操作
    /// </summary>
    public class ImageUtil
    {
        private static string[] ImageExtensions = new string[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".icon", ".ico" };

        /// <summary>判断扩展名是否为图片
        /// </summary>
        public bool IsImageExtension(string extension)
        {
            if (extension.IsNullOrWhiteSpace())
            {
                return false;
            }
            return ImageExtensions.Contains(extension.ToLower());
        }

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

        /// <summary>根据图片格式,获取格式名
        /// </summary>
        public static string GetImageFormatName(ImageFormat format)
        {
            if (format.Equals(ImageFormat.Jpeg))
            {
                return "Jpeg";
            }
            if (format.Equals(ImageFormat.Png))
            {
                return "Png";
            }
            if (format.Equals(ImageFormat.Gif))
            {
                return "Gif";
            }
            if (format.Equals(ImageFormat.Icon))
            {
                return "Icon";
            }

            if (format.Equals(ImageFormat.Bmp))
            {
                return "Bmp";
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


        /// <summary>将图片转换成流文件
        /// </summary>
        public static Stream ImageToStream(Image image)
        {
            MemoryStream ms = new MemoryStream();
            image.Save(ms, image.RawFormat);
            ms.Seek(0, SeekOrigin.Begin);
            return ms;
        }


        /// <summary>将图片转换成byte[]
        /// </summary>
        public static byte[] ImageToBytes(Image image)
        {
            using(MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, image.RawFormat);
                ms.Seek(0, SeekOrigin.Begin);
                return StreamUtil.StreamToBytes(ms);
            }
        }

        /// <summary>将图片转换成Base64编码
        /// </summary>
        public static string ImageToBase64(Image image, bool includeHeader = false)
        {
            var imageBytes = ImageToBytes(image);
            var base64 = Convert.ToBase64String(imageBytes);
            if (includeHeader)
            {
                base64 = $"data:image/{GetImageFormatName(image.RawFormat)};base64,{base64}";
            }
            return base64;
        }

        /// <summary>将Base64转换成图片
        /// </summary>
        public static Image Base64ToImage(string base64, bool hasHeader = false)
        {
            try
            {
                //base64中是否包含了头部
                if (hasHeader)
                {
                    base64 = base64.Substring(base64.IndexOf(',') + 1);
                }
                byte[] bytes = Convert.FromBase64String(base64);
                MemoryStream memStream = new MemoryStream(bytes);
                Image img = Image.FromStream(memStream);
                return img;
            }
            catch
            {
                return null;
            }
        }

    }
}