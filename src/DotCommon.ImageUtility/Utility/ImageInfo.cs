using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;

namespace DotCommon.Utility
{
    /// <summary>图片信息
    /// </summary>
    public class ImageInfo
    {
        /// <summary>ImageFormat
        /// </summary>
        public ImageFormat ImageFormat { get; set; }

        /// <summary>Format名称,如Jpeg
        /// </summary>
        public string FormatName { get; set; }

        /// <summary>扩展名
        /// </summary>
        public List<string> Extensions { get; set; } = new List<string> { };

        /// <summary>第一个扩展名
        /// </summary>
        public string FirstExtension()
        {
            return Extensions.FirstOrDefault();
        }

        /// <summary>Ctor
        /// </summary>
        public ImageInfo()
        {

        }

        /// <summary>Ctor
        /// </summary>
        public ImageInfo(ImageFormat imageFormat, string formatName, params string[] extensions)
        {
            ImageFormat = imageFormat;
            FormatName = formatName;
            Extensions = extensions.ToList();
        }
    }
}
