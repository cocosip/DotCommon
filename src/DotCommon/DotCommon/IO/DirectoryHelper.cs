using System;
using System.IO;

namespace DotCommon.IO
{
    /// <summary>
    /// Provides utility methods for directory operations.
    /// </summary>
    public static class DirectoryHelper
    {
        /// <summary>
        /// Creates a directory if it does not already exist.
        /// </summary>
        /// <param name="directory">The path of the directory to create.</param>
        /// <exception cref="ArgumentNullException">Thrown when directory is null.</exception>
        /// <exception cref="ArgumentException">Thrown when directory is empty or contains invalid characters.</exception>
        public static void CreateIfNotExists(string directory)
        {
            if (directory == null)
                throw new ArgumentNullException(nameof(directory));

            if (string.IsNullOrEmpty(directory))
                throw new ArgumentException("Directory path cannot be null or empty.", nameof(directory));

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        /// <summary>
        /// Deletes a directory if it exists.
        /// </summary>
        /// <param name="directory">The path of the directory to delete.</param>
        /// <param name="recursive">True to remove directories, subdirectories, and files; false to remove only empty directories.</param>
        /// <exception cref="ArgumentNullException">Thrown when directory is null.</exception>
        /// <exception cref="ArgumentException">Thrown when directory is empty.</exception>
        public static void DeleteIfExist(string directory, bool recursive = false)
        {
            if (directory == null)
                throw new ArgumentNullException(nameof(directory));

            if (string.IsNullOrEmpty(directory))
                throw new ArgumentException("Directory path cannot be null or empty.", nameof(directory));

            if (Directory.Exists(directory))
            {
                Directory.Delete(directory, recursive);
            }
        }

        /// <summary>
        /// Copies a directory and all its contents to a target location.
        /// </summary>
        /// <param name="sourceDir">The path of the source directory to copy.</param>
        /// <param name="targetDir">The path of the target directory where the contents will be copied.</param>
        /// <exception cref="ArgumentNullException">Thrown when sourceDir or targetDir is null.</exception>
        /// <exception cref="ArgumentException">Thrown when sourceDir or targetDir is empty.</exception>
        /// <exception cref="DirectoryNotFoundException">Thrown when the source directory does not exist.</exception>
        public static void DirectoryCopy(string sourceDir, string targetDir)
        {
            if (sourceDir == null)
                throw new ArgumentNullException(nameof(sourceDir));

            if (targetDir == null)
                throw new ArgumentNullException(nameof(targetDir));

            if (string.IsNullOrEmpty(sourceDir))
                throw new ArgumentException("Source directory path cannot be null or empty.", nameof(sourceDir));

            if (string.IsNullOrEmpty(targetDir))
                throw new ArgumentException("Target directory path cannot be null or empty.", nameof(targetDir));

            if (!Directory.Exists(sourceDir))
                throw new DirectoryNotFoundException($"Source directory does not exist: {sourceDir}");

            CreateIfNotExists(targetDir);

            DirectoryInfo sourceDirectoryInfo = new DirectoryInfo(sourceDir);
            FileSystemInfo[] fileSystemInfos = sourceDirectoryInfo.GetFileSystemInfos();

            foreach (FileSystemInfo fileSystemInfo in fileSystemInfos)
            {
                if (fileSystemInfo is DirectoryInfo subDirectoryInfo)
                {
                    // Recursively copy subdirectories
                    DirectoryCopy(subDirectoryInfo.FullName, Path.Combine(targetDir, subDirectoryInfo.Name));
                }
                else
                {
                    // Copy files with overwrite
                    File.Copy(fileSystemInfo.FullName, Path.Combine(targetDir, fileSystemInfo.Name), true);
                }
            }
        }
    }
}