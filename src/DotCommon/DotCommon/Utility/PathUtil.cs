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
            
            // Replicate the behavior of the historical CombineRelative method
            var pathBuilder = new StringBuilder();
            for (int i = 0; i < paths.Length; i++)
            {
                var currentPath = paths[i];
                
                // For paths after the first, remove ~/ and ../ prefixes
                if (i > 0)
                {
                    currentPath = currentPath.Replace("~/", "").Replace("../", "");
                }
                
                // Ensure the path ends with a forward slash
                if (!currentPath.EndsWith("/"))
                {
                    currentPath = $"{currentPath}/";
                }
                
                // Remove leading slash if present
                if (currentPath.StartsWith("/"))
                {
                    currentPath = currentPath.Substring(1);
                }
                
                pathBuilder.Append(currentPath);
            }
            
            // Remove the trailing slash if the result is not empty
            if (pathBuilder.Length > 0)
            {
                pathBuilder.Remove(pathBuilder.Length - 1, 1);
            }
            
            return pathBuilder.ToString();
        }

        /// <summary>
        /// Navigates up the directory tree from a given path by a specified number of layers.
        /// Supports both Windows-style (C:\path\to\file) and Unix-style (/path/to/file) paths on any platform.
        /// </summary>
        /// <param name="path">The starting path (can be relative or absolute).</param>
        /// <param name="layerCount">The number of layers to go up.</param>
        /// <returns>The ancestor directory path.</returns>
        public static string GetAncestorDirectory(string path, int layerCount = 1)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return path;
            }

            // Detect path style: Windows (contains : like C:) or Unix (starts with /)
            bool isWindowsPath = path.Length >= 2 && path[1] == ':' && char.IsLetter(path[0]);
            bool isUnixPath = path.StartsWith("/");

            // Return non-absolute paths as-is (for backward compatibility)
            if (!isWindowsPath && !isUnixPath)
            {
                return path;
            }

            // Determine the separator used in this path
            char pathSeparator;
            string rootPath;

            if (isWindowsPath)
            {
                // Windows path: extract drive letter (e.g., "C:")
                pathSeparator = '\\';
                rootPath = path.Substring(0, 2); // "C:", "D:", etc.
            }
            else
            {
                // Unix path: root is "/"
                pathSeparator = '/';
                rootPath = "/";
            }

            // Split by both separators to handle mixed paths (e.g., "C:/path/to/file")
            var pathSegments = path.Split(new[] { '\\', '/' }, StringSplitOptions.RemoveEmptyEntries)
                .Where(segment => !string.IsNullOrWhiteSpace(segment) && !segment.Contains(":"))
                .ToList();

            // If we're trying to go up more levels than we have segments, return the root
            if (pathSegments.Count <= layerCount)
            {
                return rootPath;
            }

            // Remove the specified number of segments from the end
            pathSegments.RemoveRange(pathSegments.Count - layerCount, layerCount);

            // Rebuild the path maintaining the original style
            if (isWindowsPath)
            {
                // Windows: "C:\path\to"
                return rootPath + pathSeparator + string.Join(pathSeparator.ToString(), pathSegments);
            }
            else
            {
                // Unix: "/path/to"
                return rootPath + string.Join(pathSeparator.ToString(), pathSegments);
            }
        }

        /// <summary>
        /// Resolves a virtual path (e.g., "~/", "../") to a physical file system path.
        /// This method is similar to ASP.NET's Server.MapPath.
        /// </summary>
        /// <param name="virtualPath">The virtual path to resolve.</param>
        /// <returns>The resolved physical path.</returns>
        public static string ResolveVirtualPath(string virtualPath)
        {
            // Handle null or empty paths
            if (string.IsNullOrEmpty(virtualPath))
            {
                return Directory.GetCurrentDirectory();
            }

            // Replicate the behavior of the historical MapPath method
            string locatePath = Directory.GetCurrentDirectory();
            // Default number of folders to go back is 2
            var backLayer = 2;
            // Calculate how many levels to go back based on the relative path
            // Each "../" represents going up one level
            backLayer += virtualPath.Length - virtualPath.Replace("../", "..").Length;
            
            // Navigate up the required number of levels
            string basePath = locatePath;
            for (int i = 0; i < backLayer; i++)
            {
                basePath = Directory.GetParent(basePath)?.FullName ?? basePath;
            }
            
            // Extract the useful parts of the path (excluding "..")
            var usefulPaths = virtualPath.Split('/').Where(segment => segment != ".." && segment != "~").ToArray();
            return Path.GetFullPath(Path.Combine(basePath, Path.Combine(usefulPaths)));
        }

        /// <summary>
        /// Gets the file extension from a path or file name.
        /// </summary>
        /// <param name="path">The path or file name.</param>
        /// <returns>The file extension, including the leading dot, or an empty string if no extension exists.</returns>
        public static string GetFileExtension(string path)
        {
            if (string.IsNullOrWhiteSpace(path) || path.IndexOf('.') < 0)
            {
                return string.Empty;
            }

            return path.Substring(path.LastIndexOf('.'));
        }
    }
}