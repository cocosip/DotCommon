using System.IO;

namespace DotCommon.IO
{
    /// <summary>文件夹帮助类
    /// </summary>
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

        /// <summary>拷贝文件夹和文件夹下的文件
        /// </summary>
        public static void DirectoryCopy(string sourceDir, string targetDir)
        {

            CreateIfNotExists(targetDir);
            DirectoryInfo dir = new DirectoryInfo(sourceDir);
            FileSystemInfo[] fileinfo = dir.GetFileSystemInfos();  //获取目录下（不包含子目录）的文件和子目录
            foreach (FileSystemInfo i in fileinfo)
            {
                //判断是否文件夹
                if (i is DirectoryInfo)
                {
                    //递归调用复制子文件夹
                    DirectoryCopy(i.FullName, Path.Combine(targetDir, i.Name));
                }
                else
                {
                    //拷贝文件,覆盖的形式
                    File.Copy(i.FullName, Path.Combine(targetDir, i.Name), true);
                }
            }

        }

    }
}
