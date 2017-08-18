#if !NETSTANDARD2_0
using System;
using System.Drawing;
using DotCommon.Utility;

namespace DotCommon.Img
{
    /// <summary>验证码图片
    /// </summary>
    public class VerificationImage
    {
        private static readonly string[] Fonts = { "Consolas", "courier new", "微软雅黑" };
        private static readonly FontStyle[] Styles = { FontStyle.Regular, FontStyle.Bold, FontStyle.Bold, FontStyle.Bold };

        #region 根据字符内容生成图片

        /// <summary> 根据字符转换成图片
        /// </summary>
        public static Image CreateCode(string code)
        {
            //字体的大小
            int fontSize = 20;
            //每个字所占用的宽度
            int fWidth = fontSize;
            //图片的宽度,为 字体宽度*字数*2
            int imageWidth = code.Length*fWidth*2;
            //图片的高度,为 字体高度的2倍
            int imageHeight = fontSize*2;
            var image = new Bitmap(imageWidth, imageHeight);
            Graphics g = Graphics.FromImage(image);
            //获取品质(压缩率)编码
            //System.Drawing.Imaging.Encoder encoder = System.Drawing.Imaging.Encoder.Quality;
            //EncoderParameter mycoder = new EncoderParameter(encoder, 30L);
            //EncoderParameters myCoders = new EncoderParameters(1);
            //myCoders.Param[0] = mycoder;
            //ImageCodecInfo jpgInfo = ImageHelper.GetEncoder(ImageFormat.Jpeg);
            //g.CompositingQuality = CompositingQuality.HighSpeed;
            //g.SmoothingMode = SmoothingMode.HighSpeed;
            //g.InterpolationMode = InterpolationMode.Low;
            //背景图
            g.Clear(Color.White);
            //先生成随机数
            Random rd = new Random(RandomUtil.GetRandomSeed(4));
            //随机获取字体的名字
            string fontName = Fonts[rd.Next(Fonts.Length - 1)];
            //随机获取字体的样式
            FontStyle fontStyle = Styles[rd.Next(Styles.Length - 1)];
            //随机获取验证码的字体
            Font font = new Font(fontName, fontSize, fontStyle);
            //验证码颜色画刷
            Brush brush = new SolidBrush(Color.Blue);
            int minLeft = 0;
            int maxLeft = imageWidth - code.Length*fontSize;
            int left = rd.Next(minLeft, maxLeft);
            for (int i = 0; i < code.Length; i++)
            {
                //需要写入的字符
                int rLeft = left + (i*fontSize);
                //上下移动
                //int minTop = fontSize/8;
                //int maxTop = fontSize / 4;
                //int top = rd.Next(minTop, maxTop);
                //旋转
                int minRotation = -5;
                int maxRotation = 5;
                int rotation = rd.Next(minRotation, maxRotation);
                g.RotateTransform(rotation);
                g.DrawString(code[i].ToString(), font, brush, rLeft, 0);
                g.RotateTransform(-rotation);

            }
            //画一个边框 边框颜色为Color.Gainsboro
            // g.DrawRectangle(new Pen(Color.Gainsboro, 0), 0, 0, image.Width - 1, image.Height - 1);
            g.Dispose();
            //产生波形
            image = ImageProcess.TwistImage(image, true, 2, 4);
            return image;
        }

        #endregion

    }
}
#endif