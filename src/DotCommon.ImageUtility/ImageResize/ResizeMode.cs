﻿namespace DotCommon.ImageResize
{
    /// <summary>压缩模式
    /// </summary>
    public static class ResizeMode
    {

        /// <summary>等比例缩放
        /// </summary>
        public const string Zoom = "Zoom";

        /// <summary>平铺
        /// </summary>
        public const string Carvel = "Carvel";

        /// <summary>裁剪
        /// </summary>
        public const string Crop = "Crop";

        /// <summary>模式
        /// </summary>
        public static readonly string[] Modes = new string[] { Zoom, Carvel, Crop };
    }
}
