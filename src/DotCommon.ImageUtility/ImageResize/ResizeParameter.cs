using DotCommon.Extensions;
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

        /// <summary>填充颜色
        /// </summary>
        public string BackColor { get; set; }

        /// <summary>裁剪开始位置
        /// </summary>
        public int CropX { get; set; }

        /// <summary>裁剪位置Y
        /// </summary>
        public int CropY { get; set; }

        /// <summary>是否存在参数
        /// </summary>
        public bool HasParams()
        {
            return (Width > 0 || Height > 0) && !Format.IsNullOrWhiteSpace() && !Mode.IsNullOrWhiteSpace();
        }

        /// <summary>Ctor
        /// </summary>
        public ResizeParameter()
        {

        }

        /// <summary>Override ToString
        /// </summary>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"Width: {Width},");
            sb.Append($"Height: {Height},");
            sb.Append($"AutoRotate: {AutoRotate},");
            sb.Append($"Quality: {Quality},");
            sb.Append($"Format: {Format},");
            sb.Append($"Mode: {Mode}");
            sb.Append($"CropX: {CropX}");
            sb.Append($"CropY: {CropY}");
            sb.Append($"BackColor: {BackColor}");
            return sb.ToString();
        }
    }
}
