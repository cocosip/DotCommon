using DotCommon.Utility;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace DotCommon.ImageResize
{
    /// <summary>图片尺寸调整
    /// </summary>
    public class ImageResizer
    {

        /// <summary>图片尺寸调整
        /// </summary>
        /// <param name="original">原图片</param>
        /// <param name="parameter">尺寸调整参数</param>
        /// <returns></returns>
        public static Image ResizeByParameter(Image original, ResizeParameter parameter)
        {
            //新生成的图片的宽与高
            var width = parameter.Width;
            var height = parameter.Height;

            //新建一个bmp图片
            var bitmap = new Bitmap(width, height);
            //新建一个画板
            var g = Graphics.FromImage(bitmap);
            try
            {

                //设置高质量插值法
                g.InterpolationMode = InterpolationMode.High;
                //设置高质量,低速度呈现平滑程度
                g.SmoothingMode = SmoothingMode.HighQuality;
                //清空画布并以透明背景色填充
                g.Clear(Color.Transparent);
                //在指定位置并且按指定大小绘制原图片的指定部分
                g.DrawImage(original, new Rectangle(0, 0, width, height), new Rectangle(0, 0, original.Width, original.Height), GraphicsUnit.Pixel);

                return bitmap;
            }
            finally
            {
                original.Dispose();
                g.Dispose();
            }
        }


        /// <summary>图片缩放
        /// </summary>
        /// <param name="original">原图</param>
        /// <param name="parameter">缩放参数</param>
        public static Image Zoom(Image original, ResizeParameter parameter)
        {
            //如果长与宽都小于0
            if ((parameter.Width <= 0 && parameter.Height <= 0) || parameter.Mode != ResizeMode.Zoom)
            {
                return original;
            }

            //新图片的宽度与高度
            var width = parameter.Width;
            var height = parameter.Height;
            var x = 0;
            var y = 0;
            //如果宽度为0,高度就是固定的
            if (parameter.Width == 0)
            {
                width = (int)Math.Round((float)original.Width * parameter.Height / original.Height);
            }
            //高度为0,固定宽度
            if (parameter.Height == 0)
            {
                height = (int)Math.Round((float)original.Height * parameter.Width / original.Width);
            }

            //长与宽都大于0
            if (parameter.Width > 0 && parameter.Height > 0)
            {
                //比例不一致,需要补空白
                if ((float)original.Height / original.Width > (float)parameter.Height / parameter.Width)
                {
                    //新图需要的宽度比较宽,那么高度按照新的高度来
                    width = (int)Math.Round((float)original.Width * parameter.Height / original.Height);
                    x = (int)Math.Round((float)Math.Abs(original.Width - parameter.Width) / 2);
                }
                else
                {
                    height = (int)Math.Round((float)original.Height * parameter.Width / original.Width);
                    y = (int)Math.Round((float)Math.Abs(original.Height - parameter.Height) / 2);
                }
            }

            //新建一个bmp图片
            var bitmap = new Bitmap(width, height);
            //新建一个画板
            var g = Graphics.FromImage(bitmap);
            try
            {

                //设置高质量插值法
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                //设置高质量,低速度呈现平滑程度
                g.SmoothingMode = SmoothingMode.HighQuality;

                //清空画布并以透明背景色填充
                g.Clear(Color.Transparent);
                //在指定位置并且按指定大小绘制原图片的指定部分
                g.DrawImage(original, new Rectangle(x, y, width, height), new Rectangle(0, 0, original.Width, original.Height), GraphicsUnit.Pixel);
                
                ////在保存图片时设置图片质量
                //Encoder qualityEncoder = Encoder.Quality;
                //EncoderParameters encoderParameters = new EncoderParameters(1);
                //EncoderParameter qualityEncoderParameter = new EncoderParameter(qualityEncoder, parameter.Quality);
                //encoderParameters.Param[0] = qualityEncoderParameter;

                //using (MemoryStream ms = new MemoryStream())
                //{
                //    bitmap.Save(ms, ImageUtil.GetEncoder(original), encoderParameters);
                //}
                return bitmap;
            }
            finally
            {
                original.Dispose();
                g.Dispose();
            }
        }

 


    }
}
