//using System;
//using System.Drawing;
//using System.Drawing.Drawing2D;

//namespace DotCommon.ImageResize
//{
//    /// <summary>图片尺寸调整
//    /// </summary>
//    public class ImageResizer
//    {

//        /// <summary>图片尺寸调整
//        /// </summary>
//        /// <param name="original">原图片</param>
//        /// <param name="parameter">尺寸调整参数</param>
//        /// <returns></returns>
//        public Image ResizeByParameter(Image original, ResizeParameter parameter)
//        {
//            //新生成的图片的宽与高
//            var width = parameter.Width;
//            var height = parameter.Height;

//            //新建一个bmp图片
//            var bitmap = new Bitmap(width, height);
//            //新建一个画板
//            var g = Graphics.FromImage(bitmap);
//            try
//            {

//                //设置高质量插值法
//                g.InterpolationMode = InterpolationMode.High;
//                //设置高质量,低速度呈现平滑程度
//                g.SmoothingMode = SmoothingMode.HighQuality;
//                //清空画布并以透明背景色填充
//                g.Clear(Color.Transparent);
//                //在指定位置并且按指定大小绘制原图片的指定部分
//                g.DrawImage(original, new Rectangle(0, 0, width, height), new Rectangle(0, 0, original.Width, original.Height), GraphicsUnit.Pixel);

//                return bitmap;
//            }
//            finally
//            {
//                original.Dispose();
//                g.Dispose();
//            }
//        }


//        /// <summary>图片缩放
//        /// </summary>
//        /// <param name="original">原图</param>
//        /// <param name="parameter">缩放参数</param>
//        public Image Zoom(Image original, ResizeParameter parameter)
//        {
//            //如果是缩小,那么按照长宽比例中,比较小的那个为标准进行缩放。比如目标的长度比较长,那么应该以宽度为标准,长度补空白
//            //如果放大,



//            //新建一个bmp图片
//            var bitmap = new Bitmap(width, height);
//            //新建一个画板
//            var g = Graphics.FromImage(bitmap);
//            try
//            {

//                //设置高质量插值法
//                g.InterpolationMode = InterpolationMode.High;
//                //设置高质量,低速度呈现平滑程度
//                g.SmoothingMode = SmoothingMode.HighQuality;
//                //清空画布并以透明背景色填充
//                g.Clear(Color.Transparent);
//                //在指定位置并且按指定大小绘制原图片的指定部分
//                g.DrawImage(original, new Rectangle(0, 0, width, height), new Rectangle(0, 0, original.Width, original.Height), GraphicsUnit.Pixel);

//                return bitmap;
//            }
//            finally
//            {
//                original.Dispose();
//                g.Dispose();
//            }
//        }



//        /// <summary>Pad模式(放大,当新的图片的宽,高比原图大)
//        /// </summary>
//        /// <param name="original">原图</param>
//        /// <param name="parameter">尺寸调整参数</param>
//        /// <returns></returns>
//        public Image Pad(Image original, ResizeParameter parameter)
//        {
//            //图片的宽度与高度
//            var width = parameter.Width < original.Width ? original.Width : parameter.Width;
//            var height = parameter.Height < original.Height ? original.Height : parameter.Height;

//            // find co-ords to draw original at
//            var x = original.Width < parameter.Width ? (parameter.Width - original.Width) / 2 : 0;
//            var y = original.Height < parameter.Height ? (parameter.Height - original.Height) / 2 : 0;

//            //新建一个bmp图片
//            var bitmap = new Bitmap(width, height);
//            //新建一个画板
//            var g = Graphics.FromImage(bitmap);
//            try
//            {

//                //设置高质量插值法
//                g.InterpolationMode = InterpolationMode.High;
//                //设置高质量,低速度呈现平滑程度
//                g.SmoothingMode = SmoothingMode.HighQuality;
//                //清空画布并以透明背景色填充
//                g.Clear(Color.Transparent);
//                //在指定位置并且按指定大小绘制原图片的指定部分
//                g.DrawImage(original, new Rectangle(0, 0, width, height), new Rectangle(0, 0, original.Width, original.Height), GraphicsUnit.Pixel);

//                return bitmap;
//            }
//            finally
//            {
//                original.Dispose();
//                g.Dispose();
//            }

//        }

//        /// <summary>裁剪
//        /// </summary>
//        /// <param name="original">原图</param>
//        /// <param name="parameter">裁剪参数</param>
//        /// <returns></returns>
//        public Image Crop(Image original, ResizeParameter parameter)
//        {
//            var cropSides = 0; //宽度填充
//            var cropTopBottom = 0;//高度填充
//            //新图片的宽高
//            var width = parameter.Width;
//            var height = parameter.Height;

//            // calculate amount of pixels to remove from sides and top/bottom
//            if ((float)parameter.Width / original.Width < parameter.Height / original.Height)
//            {
//                //高度比较高,那么就固定高度,调整宽度,在宽度进行填充
//                cropSides = original.Width - (int)Math.Round((float)original.Height / parameter.Height * parameter.Width);
//                //新宽度=原宽度+空白
//                width = width + cropSides;
//            }
//            else
//            {
//                //宽度比较宽,那么就固定宽度,调整高度,在高度进行填充
//                cropTopBottom = original.Height - (int)Math.Round((float)original.Width / parameter.Width * parameter.Height);
//                //新高度=原高度+空白
//                height = height + cropTopBottom;
//            }

//            //新建一个bmp图片,宽度
//            var bitmap = new Bitmap(width, height);
//            //新建一个画板
//            var g = Graphics.FromImage(bitmap);
//            try
//            {

//                //设置高质量插值法
//                g.InterpolationMode = InterpolationMode.High;
//                //设置高质量,低速度呈现平滑程度
//                g.SmoothingMode = SmoothingMode.HighQuality;
//                //清空画布并以透明背景色填充
//                g.Clear(Color.Transparent);
//                //在指定位置并且按指定大小绘制原图片的指定部分
//                //缩放的时候,需要把空白的部分计算进去
//                g.DrawImage(original, new Rectangle(cropSides / 2, cropTopBottom / 2, parameter.Width, parameter.Height), new Rectangle(0, 0, original.Width, original.Height), GraphicsUnit.Pixel);

//                return bitmap;
//            }
//            finally
//            {
//                original.Dispose();
//                g.Dispose();
//            }
//        }

//    }
//}
