using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DotCommon.Utility
{
    /// <summary>
    /// A utility class for working with MIME type names.
    /// </summary>
    public static class MimeTypeNameUtil
    {
        private const string DefaultMimeType = "application/octet-stream";

        private static readonly IReadOnlyDictionary<string, string> MimeTypes = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { ".*", DefaultMimeType },
            { ".323", "text/h323" },
            { ".3g2", "video/3gpp2" },
            { ".3gp", "video/3gpp" },
            { ".3gp2", "video/3gpp2" },
            { ".3gpp", "video/3gpp" },
            { ".7z", "application/x-7z-compressed" },
            { ".aif", "audio/aiff" },
            { ".aifc", "audio/aiff" },
            { ".aiff", "audio/aiff" },
            { ".asf", "video/x-ms-asf" },
            { ".asp", "text/asp" },
            { ".asx", "video/x-ms-asf" },
            { ".au", "audio/basic" },
            { ".avi", "video/avi" },
            { ".bmp", "image/bmp" },
            { ".css", "text/css" },
            { ".csv", "text/csv" },
            { ".doc", "application/msword" },
            { ".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document" },
            { ".dot", "application/msword" },
            { ".dotx", "application/vnd.openxmlformats-officedocument.wordprocessingml.template" },
            { ".eml", "message/rfc822" },
            { ".eot", "application/vnd.ms-fontobject" },
            { ".eps", "application/postscript" },
            { ".exe", "application/octet-stream" },
            { ".flv", "video/x-flv" },
            { ".gif", "image/gif" },
            { ".htm", "text/html" },
            { ".html", "text/html" },
            { ".ico", "image/x-icon" },
            { ".jar", "application/java-archive" },
            { ".java", "text/x-java-source" },
            { ".jpeg", "image/jpeg" },
            { ".jpg", "image/jpeg" },
            { ".js", "application/javascript" },
            { ".json", "application/json" },
            { ".log", "text/plain" },
            { ".m3u", "audio/mpegurl" },
            { ".m4a", "audio/mp4" },
            { ".m4v", "video/mp4" },
            { ".mdb", "application/x-msaccess" },
            { ".mid", "audio/midi" },
            { ".midi", "audio/midi" },
            { ".mov", "video/quicktime" },
            { ".mp3", "audio/mpeg" },
            { ".mp4", "video/mp4" },
            { ".mpeg", "video/mpeg" },
            { ".mpg", "video/mpeg" },
            { ".msi", "application/x-msdownload" },
            { ".ogg", "audio/ogg" },
            { ".pdf", "application/pdf" },
            { ".png", "image/png" },
            { ".ppt", "application/vnd.ms-powerpoint" },
            { ".pptx", "application/vnd.openxmlformats-officedocument.presentationml.presentation" },
            { ".ps", "application/postscript" },
            { ".psd", "image/vnd.adobe.photoshop" },
            { ".rar", "application/x-rar-compressed" },
            { ".rtf", "application/rtf" },
            { ".svg", "image/svg+xml" },
            { ".swf", "application/x-shockwave-flash" },
            { ".tar", "application/x-tar" },
            { ".tif", "image/tiff" },
            { ".tiff", "image/tiff" },
            { ".txt", "text/plain" },
            { ".wav", "audio/wav" },
            { ".weba", "audio/webm" },
            { ".webm", "video/webm" },
            { ".webp", "image/webp" },
            { ".wma", "audio/x-ms-wma" },
            { ".wmv", "video/x-ms-wmv" },
            { ".woff", "font/woff" },
            { ".woff2", "font/woff2" },
            { ".xhtml", "application/xhtml+xml" },
            { ".xls", "application/vnd.ms-excel" },
            { ".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" },
            { ".xml", "text/xml" },
            { ".zip", "application/zip" }
        };

        /// <summary>
        /// Gets the MIME type name from a file extension.
        /// </summary>
        /// <param name="extension">The file extension (e.g., ".txt", "txt").</param>
        /// <returns>The corresponding MIME type, or the default "application/octet-stream" if not found.</returns>
        public static string GetMimeName(string extension)
        {
            if (string.IsNullOrWhiteSpace(extension))
            {
                return DefaultMimeType;
            }

            if (!extension.StartsWith("."))
            {
                extension = "." + extension;
            }

            return MimeTypes.TryGetValue(extension, out var mimeType) ? mimeType : DefaultMimeType;
        }

        /// <summary>
        /// Gets the MIME type name from a file path.
        /// </summary>
        /// <param name="filePath">The full path to the file.</param>
        /// <returns>The corresponding MIME type, or the default "application/octet-stream" if not found.</returns>
        public static string GetMimeNameFromFile(string filePath)
        {
            string extension = Path.GetExtension(filePath);
            return GetMimeName(extension);
        }

        /// <summary>
        /// Gets the primary file extension associated with a MIME type.
        /// </summary>
        /// <param name="mimeType">The MIME type (e.g., "image/jpeg").</param>
        /// <returns>The corresponding file extension (e.g., ".jpg") if found; otherwise, null.</returns>
        public static string? GetExtension(string mimeType)
        {
            if (string.IsNullOrWhiteSpace(mimeType))
            {
                return null;
            }

            // Find the first key (extension) that matches the given MIME type.
            // Note: Multiple extensions can map to the same MIME type (e.g., .jpg and .jpeg).
            // This method returns the first one it finds.
            return MimeTypes
                .FirstOrDefault(kvp => kvp.Value.Equals(mimeType, StringComparison.OrdinalIgnoreCase))
                .Key;
        }
    }
}