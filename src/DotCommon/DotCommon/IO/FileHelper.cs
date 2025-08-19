using System;
using System.IO;

namespace DotCommon.IO
{
    /// <summary>
    /// Provides utility methods for file operations.
    /// </summary>
    public static class FileHelper
    {
        /// <summary>
        /// Deletes a file if it exists.
        /// </summary>
        /// <param name="fileName">The name of the file to delete.</param>
        /// <exception cref="ArgumentNullException">Thrown when fileName is null.</exception>
        /// <exception cref="ArgumentException">Thrown when fileName is empty or contains invalid characters.</exception>
        public static void DeleteIfExists(string fileName)
        {
            if (fileName == null)
                throw new ArgumentNullException(nameof(fileName));

            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentException("File name cannot be null or empty.", nameof(fileName));

            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
        }

        /// <summary>
        /// Gets the size of a file.
        /// </summary>
        /// <param name="fileName">The name of the file to get the size of.</param>
        /// <returns>The size of the file in bytes.</returns>
        /// <exception cref="ArgumentNullException">Thrown when fileName is null.</exception>
        /// <exception cref="ArgumentException">Thrown when fileName is empty.</exception>
        /// <exception cref="FileNotFoundException">Thrown when the file does not exist.</exception>
        public static long GetFileSize(string fileName)
        {
            if (fileName == null)
                throw new ArgumentNullException(nameof(fileName));

            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentException("File name cannot be null or empty.", nameof(fileName));

            var fileInfo = new FileInfo(fileName);
            return fileInfo.Length;
        }
    }
}