using System;
using System.IO;
using System.Linq;
using System.Text;

namespace DotCommon.Utility
{
    /// <summary>
    /// Provides utility methods for working with file and directory paths.
    /// </summary>
    public static class PathUtil
    {
        /// <summary>
        /// Resolves a given path to an absolute path. If the path is already absolute, it is returned as is.
        /// Otherwise, it is combined with the application's base directory.
        /// </summary>
        /// <param name="path">The path to resolve. Can be absolute or relative.</param>
        /// <returns>The absolute path.</returns>
        public static string GetAbsolutePath(string path = "")
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return AppContext.BaseDirectory;
            }
            if (!Path.IsPathRooted(path))
            {
                return Path.Combine(AppContext.BaseDirectory, path);
            }
            return path;
        }

        /// <summary>
        /// Combines an array of paths into a single path.
        /// This method correctly handles path separators for the current operating system.
        /// </summary>
        /// <param name="paths">An array of paths to combine.</param>
        /// <returns>The combined path.</returns>
        public static string CombinePaths(params string[] paths)
        {
            if (paths == null || paths.Length == 0)
            {
                return string.Empty;
            }
            return Path.Combine(paths);
        }

        /// <summary>
        /// Navigates up the directory tree from a given path by a specified number of layers.
        /// </summary>
        /// <param name="path">The starting absolute path.</param>
        /// <param name="layerCount">The number of layers to go up. Must be non-negative.</param>
        /// <returns>The ancestor directory path. If navigating beyond the root, the root path is returned.</returns>
        /// <exception cref="ArgumentException">Thrown if the provided path is not absolute or layerCount is negative.</exception>
        public static string GetAncestorDirectory(string path, int layerCount = 1)
        {
            if (!Path.IsPathRooted(path))
            {
                throw new ArgumentException("Path must be an absolute path.", nameof(path));
            }
            if (layerCount < 0)
            {
                throw new ArgumentException("Layer count cannot be negative.", nameof(layerCount));
            }

            // Normalize path separators to the current OS's separator
            string normalizedPath = path.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);

            string? currentPath = normalizedPath;
            for (int i = 0; i < layerCount; i++)
            {
                if (currentPath == null || Path.GetPathRoot(currentPath) == currentPath)
                {
                    // Already at or above the root
                    return Path.GetPathRoot(normalizedPath);
                }
                currentPath = Path.GetDirectoryName(currentPath);
            }
            return currentPath ?? Path.GetPathRoot(normalizedPath); // Return root if currentPath becomes null
        }

        /// <summary>
        /// Resolves a virtual path (e.g., "~/", "../") to a physical file system path.
        /// This method is similar to ASP.NET's Server.MapPath.
        /// </summary>
        /// <param name="virtualPath">The virtual path to resolve.</param>
        /// <returns>The resolved physical path.</returns>
        public static string ResolveVirtualPath(string virtualPath)
        {
            if (string.IsNullOrWhiteSpace(virtualPath))
            {
                return AppContext.BaseDirectory;
            }

            // Handle application root (~) and parent directory (..) references
            string fullPath = virtualPath.Replace("~/", AppContext.BaseDirectory);
            fullPath = Path.GetFullPath(fullPath);

            return fullPath;
        }

        /// <summary>
        /// Gets the file extension of the specified path string.
        /// </summary>
        /// <param name="path">The path string from which to get the extension.</param>
        /// <returns>The extension of the specified path (including the "period "."), or <c>null</c> if the path is null, 
        /// or an empty string if the path does not have extension information.</returns>
        public static string? GetFileExtension(string? path)
        {
            return Path.GetExtension(path);
        }
    }
}