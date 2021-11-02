using System.IO;

namespace DotCommon.IO
{
    /// <summary>
    /// 文件工具类
    /// </summary>
    public static class FileHelper
    {
        /// <summary>
        /// 如果文件存在就删除文件
        /// </summary>
        /// <param name="fileName">文件名</param>
        public static void DeleteIfExists(string fileName)
        {
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
        }

        /// <summary>
        /// 获取文件的大小
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static long GetFileSize(string fileName)
        {
            var fileInfo = new FileInfo(fileName);
            return fileInfo.Length;
        }
    }
}
