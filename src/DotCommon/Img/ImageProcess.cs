#if !NETSTANDARD2_0
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;

namespace DotCommon.Img
{
    public class ImageProcess
    {
        #region 转换生成缩略图

        /// <summary>
        /// 转换生成缩略图
        /// </summary>
        /// <param name="imgByte"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public static Image MakeThumbnail(byte[] imgByte, int width, int height, ThumbnailMode mode)
        {
            var originalImage = ImageUtils.ByteToImage(imgByte);
            return MakeThumbnail(originalImage, width, height, mode);
        }

        public static Image MakeThumbnail(Image originalImage, int width, int height, ThumbnailMode mode)
        {

            var towidth = width;
            var toheight = height;

            var x = 0;
            var y = 0;
            var ow = originalImage.Width;
            var oh = originalImage.Height;

            switch (mode)
            {
                case ThumbnailMode.SpecifyBoth: //指定高宽缩放（可能变形）                
                    break;
                case ThumbnailMode.SpecifyWidth: //指定宽，高按比例                    
                    toheight = originalImage.Height * width / originalImage.Width;
                    break;
                case ThumbnailMode.SpecifyHigh: //指定高，宽按比例
                    towidth = originalImage.Width * height / originalImage.Height;
                    break;
                case ThumbnailMode.Cut: //指定高宽裁减（不变形）                
                    if (originalImage.Width / (double)originalImage.Height > towidth / (double)toheight)
                    {
                        oh = originalImage.Height;
                        ow = originalImage.Height * towidth / toheight;
                        y = 0;
                        x = (originalImage.Width - ow) / 2;
                    }
                    else
                    {
                        ow = originalImage.Width;
                        oh = originalImage.Width * height / towidth;
                        x = 0;
                        y = (originalImage.Height - oh) / 2;
                    }
                    break;
            }
            //新建一个bmp图片
            var bitmap = new Bitmap(towidth, toheight);
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
                g.DrawImage(originalImage, new Rectangle(0, 0, towidth, toheight), new Rectangle(x, y, ow, oh),
                    GraphicsUnit.Pixel);
                return bitmap;
                //以jpg格式保存缩略图
                //  bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            finally
            {
                originalImage.Dispose();
                bitmap.Dispose();
                g.Dispose();
            }
        }

        #endregion

        #region 图片水印

        /// <summary> 图片水印处理方法
        /// </summary>
        public static Image ImageWatermark(Image image, Image waterImage, WaterPosition waterPosition)
        {
            using (var g = Graphics.FromImage(image))
            {
                var local = GetLocation(image.Width, image.Height, waterImage.Width, waterImage.Height, waterPosition);
                g.DrawImage(waterImage, new Rectangle(local.Item1, local.Item2, waterImage.Width, waterImage.Height));
                return image;
            }
        }

        /// <summary>图片水印位置处理方法
        /// </summary>
        public static Tuple<int, int> GetLocation(int imgWidth, int imgHeight, int waterWidth, int waterHeight,
            WaterPosition waterPosition)
        {
            int x = 0;
            int y = 0;
            switch (waterPosition)
            {
                case WaterPosition.LeftCenter:
                    y = imgHeight/2 - waterHeight/2;
                    break;
                case WaterPosition.LeftBottom:
                    y = imgHeight - waterHeight;
                    break;
                case WaterPosition.RightTop:
                    x = imgWidth - waterWidth;
                    break;
                case WaterPosition.RightCenter:
                    x = imgWidth - waterWidth;
                    y = imgHeight/2 - waterHeight/2;
                    break;
                case WaterPosition.RigthBottom:
                    x = imgWidth - waterWidth;
                    y = imgHeight - waterHeight;
                    break;
                case WaterPosition.TopCenter:
                    x = imgWidth/2 - waterWidth/2;
                    break;
                case WaterPosition.BottomCenter:
                    x = imgWidth/2 - waterWidth/2;
                    y = imgHeight - waterHeight;
                    break;
                case WaterPosition.Center:
                    x = imgWidth/2 - waterWidth/2;
                    y = imgHeight/2 - waterHeight/2;
                    break;
            }
            return new Tuple<int, int>(x, y);
        }

        #endregion

        #region 文字水印

        /// <summary> 文字水印处理方法
        /// </summary>
        public static Image LetterWatermark(Image img, int size, string letter, Color color, WaterPosition waterPosition)
        {

            using (var g = Graphics.FromImage(img))
            {
                var waterWidth = letter.Length*size + 5;
                var waterHeight = size + 5;
                var local = GetLocation(img.Width, img.Height, waterWidth, waterHeight, waterPosition);
                var font = new Font("宋体", size);
                var brash = new SolidBrush(color);
                g.DrawString(letter, font, brash, float.Parse(local.Item1.ToString()),
                    float.Parse(local.Item2.ToString()));
            }
            return img;
        }
 

        #endregion

        #region 调整光暗

        /// <summary>
        /// 调整光暗
        /// </summary>
        /// <param name="bmp">原始图片</param>
        /// <param name="width">原始图片的长度</param>
        /// <param name="height">原始图片的高度</param>
        /// <param name="val">增加或减少的光暗值</param>
        public Bitmap LdPic(Bitmap bmp, int width, int height, int val)
        {
            var newBmp = new Bitmap(width, height); //初始化一个记录经过处理后的图片对象
            int x; //x、y是循环次数，后面三个是记录红绿蓝三个值的
            for (x = 0; x < width; x++)
            {
                int y; //x、y是循环次数，后面三个是记录红绿蓝三个值的
                for (y = 0; y < height; y++)
                {
                    var pixel = bmp.GetPixel(x, y);
                    var resultR = pixel.R + val; //x、y是循环次数，后面三个是记录红绿蓝三个值的
                    var resultG = pixel.G + val; //x、y是循环次数，后面三个是记录红绿蓝三个值的
                    var resultB = pixel.B + val; //x、y是循环次数，后面三个是记录红绿蓝三个值的
                    newBmp.SetPixel(x, y, Color.FromArgb(resultR, resultG, resultB)); //绘图
                }
            }
            return newBmp;
        }

        #endregion

        #region 反色处理

        /// <summary>
        /// 反色处理
        /// </summary>
        /// <param name="bmp">原始图片</param>
        /// <param name="width">原始图片的长度</param>
        /// <param name="height">原始图片的高度</param>
        public Bitmap RePic(Bitmap bmp, int width, int height)
        {
            var bm = new Bitmap(width, height); //初始化一个记录处理后的图片的对象
            int x;
            for (x = 0; x < width; x++)
            {
                int y;
                for (y = 0; y < height; y++)
                {
                    var pixel = bmp.GetPixel(x, y);
                    var resultR = 255 - pixel.R;
                    var resultG = 255 - pixel.G;
                    var resultB = 255 - pixel.B;
                    bm.SetPixel(x, y, Color.FromArgb(resultR, resultG, resultB)); //绘图
                }
            }
            return bm;
        }

        #endregion

        #region 浮雕处理

        /// <summary>
        /// 浮雕处理
        /// </summary>
        /// <param name="bmp">原始图片</param>
        /// <param name="width">原始图片的长度</param>
        /// <param name="height">原始图片的高度</param>
        public Bitmap Fd(Bitmap bmp, int width, int height)
        {
            var newBitmap = new Bitmap(width, height);
            for (var x = 0; x < width - 1; x++)
            {
                for (var y = 0; y < height - 1; y++)
                {
                    var color1 = bmp.GetPixel(x, y);
                    var color2 = bmp.GetPixel(x + 1, y + 1);
                    var r = Math.Abs(color1.R - color2.R + 128);
                    var g = Math.Abs(color1.G - color2.G + 128);
                    var b = Math.Abs(color1.B - color2.B + 128);
                    if (r > 255) r = 255;
                    if (r < 0) r = 0;
                    if (g > 255) g = 255;
                    if (g < 0) g = 0;
                    if (b > 255) b = 255;
                    if (b < 0) b = 0;
                    newBitmap.SetPixel(x, y, Color.FromArgb(r, g, b));
                }
            }
            return newBitmap;
        }

        #endregion

        #region 拉伸图片

        /// <summary>
        /// 拉伸图片
        /// </summary>
        /// <param name="bmp">原始图片</param>
        /// <param name="newW">新的宽度</param>
        /// <param name="newH">新的高度</param>
        public static Bitmap ResizeImage(Bitmap bmp, int newW, int newH)
        {
            try
            {
                Bitmap bap = new Bitmap(newW, newH);
                Graphics g = Graphics.FromImage(bap);
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(bap, new Rectangle(0, 0, newW, newH), new Rectangle(0, 0, bap.Width, bap.Height),
                    GraphicsUnit.Pixel);
                g.Dispose();
                return bap;
            }
            catch
            {
                return null;
            }
        }

        #endregion

        #region 滤色处理

        /// <summary>
        /// 滤色处理
        /// </summary>
        /// <param name="bmp">原始图片</param>
        /// <param name="width">原始图片的长度</param>
        /// <param name="height">原始图片的高度</param>
        public Bitmap FilPic(Bitmap bmp, int width, int height)
        {
            var newBmp = new Bitmap(width, height); //初始化一个记录滤色效果的图片对象
            int x;

            for (x = 0; x < width; x++)
            {
                int y;
                for (y = 0; y < height; y++)
                {
                    var pixel = bmp.GetPixel(x, y);
                    newBmp.SetPixel(x, y, Color.FromArgb(0, pixel.G, pixel.B)); //绘图
                }
            }
            return newBmp;
        }

        #endregion

        #region 左右翻转

        /// <summary>
        /// 左右翻转
        /// </summary>
        /// <param name="bmp">原始图片</param>
        /// <param name="width">原始图片的长度</param>
        /// <param name="height">原始图片的高度</param>
        public Bitmap RevPicLr(Bitmap bmp, int width, int height)
        {
            var newBmp = new Bitmap(width, height);
            int y; //x,y是循环次数,z是用来记录像素点的x坐标的变化的
            for (y = height - 1; y >= 0; y--)
            {
                int x; //x,y是循环次数,z是用来记录像素点的x坐标的变化的
                int z; //x,y是循环次数,z是用来记录像素点的x坐标的变化的
                for (x = width - 1, z = 0; x >= 0; x--)
                {
                    var pixel = bmp.GetPixel(x, y);
                    newBmp.SetPixel(z++, y, Color.FromArgb(pixel.R, pixel.G, pixel.B)); //绘图
                }
            }
            return newBmp;
        }

        #endregion

        #region 上下翻转

        /// <summary>
        /// 上下翻转
        /// </summary>
        /// <param name="bmp">原始图片</param>
        /// <param name="width">原始图片的长度</param>
        /// <param name="height">原始图片的高度</param>
        public Bitmap RevPicUd(Bitmap bmp, int width, int height)
        {
            var newBmp = new Bitmap(width, height);
            int x;
            for (x = 0; x < width; x++)
            {
                int y;
                int z;
                for (y = height - 1, z = 0; y >= 0; y--)
                {
                    var pixel = bmp.GetPixel(x, y);
                    newBmp.SetPixel(x, z++, Color.FromArgb(pixel.R, pixel.G, pixel.B)); //绘图
                }
            }
            return newBmp;
        }

        #endregion

        #region 压缩图片

        /// <summary>
        /// 压缩到指定尺寸
        /// </summary>
        /// <param name="bmp">原文件</param>
        /// <param name="newfile">新文件</param>
        public Bitmap Compress(Bitmap bmp)
        {
            var newSize = new Size(100, 125);
            var outBmp = new Bitmap(newSize.Width, newSize.Height);
            var g = Graphics.FromImage(outBmp);
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(bmp, new Rectangle(0, 0, newSize.Width, newSize.Height), 0, 0, bmp.Width, bmp.Height,
                GraphicsUnit.Pixel);
            g.Dispose();
            var encoderParams = new EncoderParameters();
            var quality = new long[1];
            quality[0] = 100;
            var encoderParam = new EncoderParameter(Encoder.Quality, quality);
            encoderParams.Param[0] = encoderParam;
            var arrayIci = ImageCodecInfo.GetImageEncoders();
            var jpegIci = arrayIci.FirstOrDefault(t => t.FormatDescription.Equals("JPEG"));
            bmp.Dispose();
            outBmp.Dispose();
            return outBmp;
        }

        #endregion

        #region 图片灰度化

        public Color Gray(Color c)
        {
            var rgb = Convert.ToInt32(((0.3 * c.R) + (0.59 * c.G)) + (0.11 * c.B));
            return Color.FromArgb(rgb, rgb, rgb);
        }

        #endregion

        #region 转换为黑白图片

        /// <summary>
        /// 转换为黑白图片
        /// </summary>
        /// <param name="bmp"></param>
        /// <param name="width">图片的长度</param>
        /// <param name="height">图片的高度</param>
        public Bitmap BwPic(Bitmap bmp, int width, int height)
        {
            var newBmp = new Bitmap(width, height);
            int x; //x,y是循环次数，result是记录处理后的像素值
            for (x = 0; x < width; x++)
            {
                int y; //x,y是循环次数，result是记录处理后的像素值
                for (y = 0; y < height; y++)
                {
                    var pixel = bmp.GetPixel(x, y);
                    var result = (pixel.R + pixel.G + pixel.B) / 3; //x,y是循环次数，result是记录处理后的像素值
                    newBmp.SetPixel(x, y, Color.FromArgb(result, result, result));
                }
            }
            return newBmp;
        }

        #endregion


        #region 产生波形滤镜效果

        /// <summary>
        /// 正弦曲线Wave扭曲图片
        /// </summary>
        /// <param name="bmp">图片路径</param>
        /// <param name="bXDir">如果扭曲则选择为True</param>
        /// <param name="dMultValue"></param>
        /// <param name="dPhase">波形的起始相位，取值区间[0-2*PI)</param>
        /// <returns></returns>
        public static Bitmap TwistImage(Bitmap bmp, bool bXDir, double dMultValue, double dPhase)
        {
            Bitmap destBmp = new Bitmap(bmp.Width, bmp.Height);
            // 将位图背景填充为白色
            Graphics graph = Graphics.FromImage(destBmp);
            graph.FillRectangle(new SolidBrush(Color.White), 0, 0, destBmp.Width, destBmp.Height);
            graph.Dispose();
            double dBaseAxisLen = bXDir ? destBmp.Height : destBmp.Width;
            for (int i = 0; i < destBmp.Width; i++)
            {
                for (var j = 0; j < destBmp.Height; j++)
                {
                    var dx = bXDir ? (2 * Math.PI * j) / dBaseAxisLen : (2 * Math.PI * i) / dBaseAxisLen;
                    dx += dPhase;
                    var dy = Math.Sin(dx);
                    // 取得当前点的颜色
                    var nOldX = bXDir ? i + (int)(dy * dMultValue) : i;
                    var nOldY = bXDir ? j : j + (int)(dy * dMultValue);
                    var color = bmp.GetPixel(i, j);
                    if (nOldX >= 0 && nOldX < destBmp.Width
                        && nOldY >= 0 && nOldY < destBmp.Height)
                    {
                        destBmp.SetPixel(nOldX, nOldY, color);
                    }
                }
            }

            return destBmp;
        }

        #endregion
    }
}
#endif