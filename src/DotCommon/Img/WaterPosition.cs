#if !NETSTANDARD2_0
namespace DotCommon.Img
{
    /// <summary>图片水印位置枚举
    /// </summary>
    public enum WaterPosition
    {
        /// <summary>左上
        /// </summary>
        LeftTop = 1,

        /// <summary>左中
        /// </summary>
        LeftCenter = 2,

        /// <summary>左下
        /// </summary>
        LeftBottom = 3,

        /// <summary>右上
        /// </summary>
        RightTop = 4,

        /// <summary>右中
        /// </summary>
        RightCenter = 5,

        /// <summary>右下
        /// </summary>
        RigthBottom = 6,

        /// <summary>顶部居中
        /// </summary>
        TopCenter = 7,

        /// <summary>底部居中
        /// </summary>
        BottomCenter=8, //底部居中   

        /// <summary>中心
        /// </summary>
        Center = 9
    }
}
#endif
