using DotCommon.Utility;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace DotCommon.ImageResize
{
    /// <summary>图片尺寸调整
    /// </summary>
    public class ImageResizer
    {

        /// <summary>图片缩放
        /// </summary>
        /// <param name="original">原图片</param>
        /// <param name="parameter">尺寸调整参数</param>
        /// <returns>图片</returns>
        public static Image Zoom(Image original, ResizeParameter parameter)
        {
            //如果长与宽都小于0
            if (parameter.Width <= 0 & parameter.Height <= 0)
            {
                throw new ArgumentException("缩略图宽度和高度至少有有一项需要大于0");
            }

            if (parameter.Mode != ResizeMode.Zoom)
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
                width = parameter.Width = (int)Math.Round((float)original.Width * parameter.Height / original.Height);
            }
            //高度为0,固定宽度
            if (parameter.Height == 0)
            {
                height = parameter.Height = (int)Math.Round((float)original.Height * parameter.Width / original.Width);
            }

            //比例不一致,需要补空白
            if ((float)original.Height / original.Width > (float)parameter.Height / parameter.Width)
            {
                //新图需要的宽度比较宽,那么高度按照新的高度来
                width = (int)Math.Round((float)original.Width * parameter.Height / original.Height);
                x = (int)Math.Round((float)Math.Abs(parameter.Width - width) / 2);
                //画布的宽度与高度
            }
            else
            {
                height = (int)Math.Round((float)original.Height * parameter.Width / original.Width);
                y = (int)Math.Round((float)Math.Abs(parameter.Height - height) / 2);
            }

            try
            {
                //新建一个bmp图片
                var bitmap = new Bitmap(parameter.Width, parameter.Height);
                using (var g = Graphics.FromImage(bitmap))
                {
                    //设置高质量插值法
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    //设置高质量,低速度呈现平滑程度
                    g.SmoothingMode = SmoothingMode.HighQuality;
                    //清空画布并以透明背景色填充
                    g.Clear(ImageUtil.GetColor(parameter.BackColor));
                    //在指定位置并且按指定大小绘制原图片的指定部分
                    g.DrawImage(original, new Rectangle(x, y, width, height), new Rectangle(0, 0, original.Width, original.Height), GraphicsUnit.Pixel);
                    return bitmap;
                }
            }
            finally
            {
                original.Dispose();
            }
        }

        /// <summary>图片裁剪
        /// </summary>
        /// <param name="original">原图片</param>
        /// <param name="parameter">尺寸调整参数</param>
        /// <returns>图片</returns>
        public static Image Crop(Image original, ResizeParameter parameter)
        {
            if (parameter.Width <= 0 || parameter.Height <= 0)
            {
                throw new ArgumentException("图片裁剪的宽度与高度不能为0");
            }
            if (parameter.Width >= original.Width || parameter.Height >= original.Height)
            {
                //长宽超过了,重置为原图的长与宽
                parameter.Width = original.Width;
                parameter.Height = original.Height;
            }
            if (parameter.Mode != ResizeMode.Crop)
            {
                return original;
            }
            //太宽了
            if (parameter.CropX + parameter.Width > original.Width)
            {
                parameter.CropX = original.Width - parameter.Width;
            }
            if (parameter.CropY + parameter.Height > original.Height)
            {
                parameter.CropY = original.Height - parameter.Height;
            }
            try
            {
                var bitmap = new Bitmap(parameter.Width, parameter.Height);
                using(var g = Graphics.FromImage(bitmap))
                {
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    //设置高质量,低速度呈现平滑程度
                    g.SmoothingMode = SmoothingMode.HighQuality;

                    //清空画布并以透明背景色填充
                    g.Clear(ImageUtil.GetColor(parameter.BackColor));
                    //在指定位置并且按指定大小绘制原图片的指定部分
                    g.DrawImage(original, new Rectangle(0, 0, parameter.Width, parameter.Height), new Rectangle(parameter.CropX, parameter.CropY, parameter.Width, parameter.Height), GraphicsUnit.Pixel);
                    return bitmap;
                }
            }
            finally
            {
                original.Dispose();
            }
        }



    }
}
