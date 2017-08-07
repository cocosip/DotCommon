using System.IO;

namespace DotCommon.IO
{
    public static class FileHelper
    {
        /// <summary>如果文件存在就删除文件
        /// </summary>
        public static void DeleteIfExists(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}
