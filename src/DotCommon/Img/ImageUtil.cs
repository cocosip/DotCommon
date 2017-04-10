#if NET45
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;

namespace DotCommon.Img
{
    /// <summary>图片的相关操作
    /// </summary>
    public class ImageUtils
    {

        #region 获取特定的图像编解码信息

        /// <summary> 获取特定的图像编解码信息
        /// </summary>
        public static ImageCodecInfo GetCodec(ImageFormat format)
        {
            var codecs = ImageCodecInfo.GetImageDecoders();
            return codecs.FirstOrDefault(codec => codec.FormatID == format.Guid);
        }

        #endregion

        #region 根据图片获取扩展名

        /// <summary>
        /// 根据图片获取扩展名
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        public static string GetImageExtension(Image img)
        {
            Type type = typeof (ImageFormat);
            var imageFormatList = type.GetProperties(BindingFlags.Static | BindingFlags.Public);
            for (int i = 0; i != imageFormatList.Length; i++)
            {
                ImageFormat formatClass = (ImageFormat) imageFormatList[i].GetValue(null, null);
                if (formatClass.Guid.Equals(img.RawFormat.Guid))
                {
                    return imageFormatList[i].Name;
                }
            }
            return "";
        }

        #endregion

        #region 获取图片的格式

        /// <summary>
        /// 获取图片的格式
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        public static ImageFormat GetImageFormate(Image img)
        {
            var type = typeof (ImageFormat);
            var imageFormatList = type.GetProperties(BindingFlags.Static | BindingFlags.Public);
            for (int i = 0; i != imageFormatList.Length; i++)
            {
                ImageFormat formatClass = (ImageFormat) imageFormatList[i].GetValue(null, null);
                if (formatClass.Guid.Equals(img.RawFormat.Guid))
                {
                    return formatClass;
                }
            }
            return ImageFormat.Jpeg;
        }

        #endregion

        #region 将图片从Image转换成byte

        /// <summary>
        /// 将图片从Image转换成byte
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static byte[] ImageToByte(Image image)
        {
            // ImageFormat formate = image.RawFormat;
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, ImageFormat.Jpeg);
                byte[] buffer = new byte[ms.Length];
                ms.Seek(0, SeekOrigin.Begin);
                ms.Read(buffer, 0, buffer.Length);
                return buffer;
            }

        }

        #endregion

        #region 将图片从Image转换成Stream

        public static Stream ImageToStream(Image image)
        {
            MemoryStream ms = new MemoryStream();
            ImageFormat format = GetImageFormate(image);
            image.Save(ms, format);
            ms.Seek(0, SeekOrigin.Begin);
            return ms;
        }

        #endregion

        #region 将图片转换成Base64

        /// <summary>
        /// 将图片转换成Base64
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static string ImageToBase64(Image image)
        {
            var imgBytes = ImageToByte(image);
            var base64 = Convert.ToBase64String(imgBytes);
            return base64;
        }

        /// <summary>
        /// 将图片转换成Base64编码,带有头部
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static string ImageToBase64WithHeader(Image image)
        {
            var base64Img = ImageToBase64(image);
            string base64 = $@"data:image/{GetImageExtension(image)};base64,{base64Img}";
            return base64;
        }

        #endregion

        #region 将Base64的字符转换成图片

        /// <summary>
        /// 将Base64编码转换成图片
        /// </summary>
        /// <param name="base64"></param>
        /// <returns></returns>
        public static Image Base64ToImage(string base64)
        {
            try
            {
                var bytes = Convert.FromBase64String(base64);
                var memStream = new MemoryStream(bytes);
                var img = Image.FromStream(memStream);
                return img;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 将带有头部编码的Base64编码转换成图片
        /// </summary>
        /// <param name="base64"></param>
        /// <returns></returns>
        public static Image Base64ToImageWithHeader(string base64)
        {
            try
            {
                base64 = base64.Substring(base64.IndexOf(',') + 1);
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

        /// <summary>
        /// 去除base64的头部
        /// </summary>
        /// <param name="base64"></param>
        /// <returns></returns>
        public static string Base64HeaderRemove(string base64)
        {
            try
            {
                base64 = base64.Substring(base64.IndexOf(',') + 1);
                return base64;
            }
            catch
            {
                return "";
            }
        }

        #endregion

        #region 将byte转换成图片

        /// <summary>
        /// 将byte转换成图片
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static Image ByteToImage(byte[] buffer)
        {
            using (MemoryStream ms = new MemoryStream(buffer))
            {
                Image image = Image.FromStream(ms);
                return image;
            }
        }

        #endregion

    }
}
#endif