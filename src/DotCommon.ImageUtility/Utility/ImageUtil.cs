using DotCommon.Extensions;
using System;
using System.Collections.Generic;
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
        protected ImageUtil()
        {

        }

        /// <summary>图片信息集合
        /// </summary>
        private static List<ImageInfo> Infos { get; set; }

        /// <summary>颜色字典
        /// </summary>
        private static Dictionary<string, Color> ColorDict { get; set; }

        /// <summary>默认扩展名
        /// </summary>
        public const string DefaultExtension = ".jpeg";

        /// <summary>默认FormatName
        /// </summary>
        public const string DefaultFormatName = "Jpeg";

        static ImageUtil()
        {
            Infos = new List<ImageInfo>();
            Infos.Add(new ImageInfo(ImageFormat.Jpeg, "Jpeg", ".jpeg", ".jpg"));
            Infos.Add(new ImageInfo(ImageFormat.Png, "Png", ".png"));
            Infos.Add(new ImageInfo(ImageFormat.Gif, "Gif", ".Gif"));
            Infos.Add(new ImageInfo(ImageFormat.Bmp, "Bmp", ".bmp"));
            Infos.Add(new ImageInfo(ImageFormat.Icon, "Icon", ".icon", ".ico"));
            Infos.Add(new ImageInfo(ImageFormat.Exif, "Exif", ".exif"));

            //颜色字典

            ColorDict = new Dictionary<string, Color>();
            ColorDict.Add("Red", Color.Red);
            ColorDict.Add("Blue", Color.Blue);
            ColorDict.Add("Yellow", Color.Yellow);
            ColorDict.Add("White", Color.White);
            ColorDict.Add("Black", Color.Black);
            ColorDict.Add("Pink", Color.Pink);
            ColorDict.Add("Green", Color.Green);
            ColorDict.Add("Gray", Color.Gray);
            ColorDict.Add("Orange", Color.Orange);
            ColorDict.Add("Transparent", Color.Transparent);
        }


        /// <summary>默认格式
        /// </summary>
        public static ImageFormat DefaultImageFormat()
        {
            return ImageFormat.Jpeg;
        }

        /// <summary>判断扩展名是否为图片
        /// </summary>
        public static bool IsImageExtension(string extension)
        {
            if (extension.IsNullOrWhiteSpace())
            {
                return false;
            }
            var extensions = Infos.SelectMany(x => x.Extensions);
            return extensions.Any(x => string.Equals(x, extension, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>判断FormatName是否为图片
        /// </summary>
        public static bool IsImageFormatName(string formatName)
        {
            if (formatName.IsNullOrWhiteSpace())
            {
                return false;
            }
            return Infos.Any(x => string.Equals(x.FormatName, formatName, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>获取图片格式的编码
        /// </summary>
        public static ImageCodecInfo GetImageCodecInfo(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            ImageCodecInfo codecInfo = null;
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    codecInfo = codec;
                    break;
                }
            }
            // 取默认的ImageFormat
            if (codecInfo == null)
            {
                return codecs.FirstOrDefault(x => x.FormatID == DefaultImageFormat().Guid);
            }
            return codecInfo;
        }

        /// <summary>根据扩展名获取图片的格式
        /// </summary>
        public static ImageFormat GetImageFormatByExtension(string extension)
        {
            var imageInfo = Infos.FirstOrDefault(x => x.Extensions.Any(c => string.Equals(c, extension, StringComparison.OrdinalIgnoreCase)));
            return imageInfo?.ImageFormat ?? DefaultImageFormat();
        }

        /// <summary>根据Format名称获取Format
        /// </summary>
        public static ImageFormat GetImageFormatByFormatName(string formatName)
        {
            var imageInfo = Infos.FirstOrDefault(x => string.Equals(x.FormatName, formatName, StringComparison.OrdinalIgnoreCase));
            return imageInfo?.ImageFormat ?? DefaultImageFormat();
        }

        /// <summary>获取图片ImageFormat
        /// </summary>
        public static ImageFormat GetImageFormatByImage(Image image)
        {
            var imageFormats = Infos.Select(x => x.ImageFormat).ToList();
            foreach (var imageFormat in imageFormats)
            {
                if (imageFormat.Guid == image.RawFormat.Guid)
                {
                    return imageFormat;
                }
            }
            return DefaultImageFormat();
        }

        /// <summary>根据图片格式获取扩展名
        /// </summary>
        public static string GetExtensionByImageFormat(ImageFormat format)
        {
            return Infos.FirstOrDefault(x => x.ImageFormat.Guid == format.Guid)?.FirstExtension() ?? DefaultExtension;
        }

        /// <summary>根据Format名称获取扩展名
        /// </summary>
        public static string GetExtensionByFormatName(string formatName)
        {
            return Infos.FirstOrDefault(x => string.Equals(x.FormatName, formatName, StringComparison.OrdinalIgnoreCase))?.FirstExtension() ?? DefaultExtension;
        }

        /// <summary>根据Format获取Format名称
        /// </summary>
        public static string GetFormatNameByImageFormat(ImageFormat format)
        {
            return Infos.FirstOrDefault(x => x.ImageFormat.Guid == format.Guid)?.FormatName ?? DefaultFormatName;
        }

        /// <summary>根据扩展名获取Format名称
        /// </summary>
        public static string GetFormatNameByExtension(string extension)
        {
            var imageInfo = Infos.FirstOrDefault(x => x.Extensions.Any(c => string.Equals(c, extension, StringComparison.OrdinalIgnoreCase)));
            return imageInfo?.FormatName ?? DefaultFormatName;
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
            if (color.IsNullOrWhiteSpace())
            {
                //透明
                return Color.Transparent;
            }
            var kv = ColorDict.FirstOrDefault(x => string.Equals(x.Key, color, StringComparison.OrdinalIgnoreCase));
            if (kv.Key != null)
            {
                return kv.Value;
            }
            return Color.Transparent;
        }

        /// <summary>将图片转换成流文件
        /// </summary>
        public static Stream ImageToStream(Image image)
        {
            MemoryStream ms = new MemoryStream();
            var imageFormat = GetImageFormatByImage(image);
            image.Save(ms, imageFormat);
            ms.Seek(0, SeekOrigin.Begin);
            return ms;
        }

        /// <summary>将图片转换成byte[]
        /// </summary>
        public static byte[] ImageToBytes(Image image)
        {
            using (var stream = ImageUtil.ImageToStream(image))
            {
                return StreamUtil.StreamToBytes(stream);
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
                base64 = $"data:image/{GetFormatNameByImageFormat(image.RawFormat)};base64,{base64}";
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
