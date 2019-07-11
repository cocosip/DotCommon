using DotCommon.ImageResize;
using System;
using System.Threading.Tasks;

namespace DotCommon.ImageResizer
{
    public interface IImageResizeService
    {
        /// <summary>获取图片的二进制数据
        /// </summary>
        /// <param name="imagePath">图片保存路径</param>
        /// <param name="resizeParameter">图片尺寸调整参数</param>
        /// <param name="lastWriteTimeUtc">最后修改时间</param>
        /// <returns></returns>
        Task<byte[]> GetImageData(string imagePath, ResizeParameter resizeParameter, DateTime lastWriteTimeUtc);
    }
}
