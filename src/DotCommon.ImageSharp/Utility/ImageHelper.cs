using System;
using System.Drawing;
using System.IO;

namespace DotCommon.Utility
{
    /// <summary>图片的一些操作
    /// </summary>
    public class ImageHelper
    {
        /// <summary>图片压缩
        /// </summary>
        /// <param name="image">图片</param>
        /// <param name="targetPath">保存路径</param>
        /// <param name="quality">图片质量</param>
        public static void ImageCompress(Image image, string targetPath, int quality)
        {
            //获取保存的质量参数
            var encoderParameters = ImageUtil.GetEncoderParametersByQuality(quality);
            //获取图片扩展名
            var extension = PathUtil.GetPathExtension(targetPath);
            //图片格式
            var imageFormat = ImageUtil.GetImageFormatByExtension(extension);
            //图片编码信息
            var imageCodecInfo = ImageUtil.GetImageCodecInfo(imageFormat);
            image.Save(targetPath, imageCodecInfo, encoderParameters);
        }

        /// <summary>图片压缩
        /// </summary>
        /// <param name="sourcePath">源图片地址</param>
        /// <param name="targetPath">目标图片保存地址</param>
        /// <param name="quality">图片质量(1-100)</param>
        public static void ImageCompress(string sourcePath, string targetPath, int quality)
        {
            var sourceImage = Image.FromFile(sourcePath);
            ImageCompress(sourceImage, targetPath, quality);
        }

        /// <summary>图片压缩
        /// </summary>
        /// <param name="image"></param>
        /// <param name="quality"></param>
        /// <returns></returns>
        public static Image ImageCompress(Image image, int quality)
        {
            //获取保存的质量参数
            var encoderParameters = ImageUtil.GetEncoderParametersByQuality(quality);
            //图片编码信息
            var imageCodecInfo = ImageUtil.GetImageCodecInfo(image);
            using(MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, imageCodecInfo, encoderParameters);
                return Image.FromStream(ms);
            }
        }

        /// <summary>图片压缩
        /// </summary>
        /// <param name="sourcePath">源图片地址</param>
        /// <param name="targetPath">目标图片保存地址</param>
        /// /// <param name="quality">图片质量(1-100)</param>
        public static byte[] ImageCompressToByte(Image image, int quality)
        {
            //获取保存的质量参数
            var encoderParameters = ImageUtil.GetEncoderParametersByQuality(quality);
            //图片编码信息
            var imageCodecInfo = ImageUtil.GetImageCodecInfo(image);

            using(MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, imageCodecInfo, encoderParameters);
                return StreamUtil.StreamToBytes(ms);
            }
        }

        /// <summary>调整图片的光暗值
        /// </summary>
        /// <param name="bitmap">原图</param>
        /// <param name="val">光暗值</param>
        /// <returns></returns>
        public static Bitmap Brightness(Bitmap bitmap, int val)
        {
            var bmp = new Bitmap(bitmap.Width, bitmap.Height); //初始化一个记录经过处理后的图片对象
            int x; //x、y是循环次数，后面三个是记录红绿蓝三个值的
            for (x = 0; x < bitmap.Width; x++)
            {
                int y; //x、y是循环次数，后面三个是记录红绿蓝三个值的
                for (y = 0; y < bitmap.Height; y++)
                {
                    var pixel = bitmap.GetPixel(x, y);
                    var resultR = pixel.R + val <= 255 ? pixel.R + val : pixel.R; //x、y是循环次数，后面三个是记录红绿蓝三个值的
                    var resultG = pixel.G + val <= 255 ? pixel.G + val : pixel.G; //x、y是循环次数，后面三个是记录红绿蓝三个值的
                    var resultB = pixel.B + val <= 255 ? pixel.B + val : pixel.B; //x、y是循环次数，后面三个是记录红绿蓝三个值的
                    bmp.SetPixel(x, y, Color.FromArgb(resultR, resultG, resultB)); //绘图
                }
            }
            return bmp;
        }

        /// <summary>图片反色处理
        /// </summary>
        public static Bitmap Inverse(Bitmap bitmap)
        {
            var bmp = new Bitmap(bitmap.Width, bitmap.Height); //初始化一个记录处理后的图片的对象
            int x;
            for (x = 0; x < bitmap.Width; x++)
            {
                int y;
                for (y = 0; y < bitmap.Height; y++)
                {
                    var pixel = bitmap.GetPixel(x, y);
                    var resultR = 255 - pixel.R;
                    var resultG = 255 - pixel.G;
                    var resultB = 255 - pixel.B;
                    bmp.SetPixel(x, y, Color.FromArgb(resultR, resultG, resultB)); //绘图
                }
            }
            return bmp;
        }

        /// <summary>图片浮雕处理
        /// </summary>
        public static Bitmap Relief(Bitmap bitmap)
        {
            var newBitmap = new Bitmap(bitmap.Width, bitmap.Height);
            for (var x = 0; x < bitmap.Width - 1; x++)
            {
                for (var y = 0; y < bitmap.Height - 1; y++)
                {
                    var color1 = bitmap.GetPixel(x, y);
                    var color2 = bitmap.GetPixel(x + 1, y + 1);
                    var r = Math.Abs(color1.R - color2.R + 128);
                    var g = Math.Abs(color1.G - color2.G + 128);
                    var b = Math.Abs(color1.B - color2.B + 128);
                    if (r > 255)r = 255;
                    if (r < 0)r = 0;
                    if (g > 255)g = 255;
                    if (g < 0)g = 0;
                    if (b > 255)b = 255;
                    if (b < 0)b = 0;
                    newBitmap.SetPixel(x, y, Color.FromArgb(r, g, b));
                }
            }
            return newBitmap;
        }

        /// <summary>图片滤色处理
        /// </summary>
        public static Bitmap ColourFilter(Bitmap bitmap)
        {
            var bmp = new Bitmap(bitmap.Width, bitmap.Height); //初始化一个记录滤色效果的图片对象
            int x;

            for (x = 0; x < bitmap.Width; x++)
            {
                int y;
                for (y = 0; y < bitmap.Height; y++)
                {
                    var pixel = bitmap.GetPixel(x, y);
                    bmp.SetPixel(x, y, Color.FromArgb(0, pixel.G, pixel.B)); //绘图
                }
            }
            return bmp;
        }

        /// <summary>转换为黑白图片
        /// </summary>
        public static Bitmap BlackWhite(Bitmap bitmap)
        {
            var bmp = new Bitmap(bitmap.Width, bitmap.Height);
            int x; //x,y是循环次数，result是记录处理后的像素值
            for (x = 0; x < bitmap.Width; x++)
            {
                int y; //x,y是循环次数，result是记录处理后的像素值
                for (y = 0; y < bitmap.Height; y++)
                {
                    var pixel = bitmap.GetPixel(x, y);
                    var result = (pixel.R + pixel.G + pixel.B) / 3; //x,y是循环次数，result是记录处理后的像素值
                    bmp.SetPixel(x, y, Color.FromArgb(result, result, result));
                }
            }
            return bmp;
        }

        /// <summary>旋转处理
        /// </summary>
        public static Image Rotate(Image original, RotateFlipType rotateFlipType)
        {
            original.RotateFlip(rotateFlipType);
            return original;
        }

    }
}
