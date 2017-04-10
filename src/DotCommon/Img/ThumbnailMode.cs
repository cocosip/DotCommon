#if NET45
namespace DotCommon.Img
{
    public enum ThumbnailMode
    {
        /// <summary> 指定高宽裁减（不变形）  
        /// </summary>
        Cut = 1,

        /// <summary>指定宽度,高度自动
        /// </summary>
        SpecifyWidth = 2,

        /// <summary> 指定高度,宽度自动
        /// </summary>
        SpecifyHigh = 3,

        /// <summary> 宽度跟高度都指定,但是会变形
        /// </summary>
        SpecifyBoth = 4
    }
}
#endif