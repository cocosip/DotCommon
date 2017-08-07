using System.IO;

namespace DotCommon.IO
{
    public static class DirectoryHelper
    {
        /// <summary>如果文件夹不存在,就创建新的文件夹
        /// </summary>
        public static void CreateIfNotExists(string directory)
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }
    }
}
