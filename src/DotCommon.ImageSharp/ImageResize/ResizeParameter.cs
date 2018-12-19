using System.Text;

namespace DotCommon.ImageResize
{
    /// <summary>图片缩放参数
    /// </summary>
    public class ResizeParameter
    {
        /// <summary>质量,1-100
        /// </summary>
        public int Quality { get; set; }

        /// <summary>是否自动旋转
        /// </summary>
        public bool AutoRotate { get; set; }

        /// <summary>格式化,png,jpg,jpeg,bmp,gif
        /// </summary>
        public string Format { get; set; }

        /// <summary>宽度
        /// </summary>
        public int Width { get; set; }

        /// <summary>高度
        /// </summary>
        public int Height { get; set; }

        /// <summary>模式
        /// </summary>
        public string Mode { get; set; }

        //public bool hasParams;
        //public int w;
        //public int h;
        //public bool autorotate;
        //public int quality; // 0 - 100
        //public string format; // png, jpg, jpeg
        //public string mode; // pad, max, crop, stretch

        //public static string[] modes = new string[] { "pad", "max", "crop", "stretch" };

        public ResizeParameter()
        {

        }


        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"Width: {Width}, ");
            sb.Append($"Height: {Height}, ");
            sb.Append($"AutoRotate: {AutoRotate}, ");
            sb.Append($"Quality: {Quality}, ");
            sb.Append($"Format: {Format}, ");
            sb.Append($"Mode: {Mode}");
            return sb.ToString();
        }
    }
}
