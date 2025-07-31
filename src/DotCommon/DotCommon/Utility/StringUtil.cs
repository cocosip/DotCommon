using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace DotCommon.Utility
{
    /// <summary>
    /// Provides utility methods for string manipulation, filtering, and encoding.
    /// </summary>
    public static class StringUtil
    {
        #region Regex Definitions

        private static readonly Regex UrlFilterRegex = new Regex("['-\\;<>《》\\s]", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex ATagRegex = new Regex("<a\\b[^>]*>.*?</a>", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);
        private static readonly Regex DivTagRegex = new Regex("<div\\b[^>]*>.*?</div>", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);
        private static readonly Regex FontTagRegex = new Regex("<font\\b[^>]*>.*?</font>", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);
        private static readonly Regex SpanTagRegex = new Regex("<span\\b[^>]*>.*?</span>", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);
        private static readonly Regex TableTagRegex = new Regex("<table\\b[^>]*>.*?</table>", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);
        private static readonly Regex ImgTagRegex = new Regex("<img(.|\n)*?>", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex ObjectTagRegex = new Regex("<object((?:.|\n)*?)</object>", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex ScriptTagRegex = new Regex("<script((?:.|\n)*?)</script>", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex IframeTagRegex = new Regex("<iframe((?:.|\n)*?)</iframe>", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex StyleTagRegex = new Regex("<style((?:.|\n)*?)</style>", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex AllTagsRegex = new Regex("<([^<]|\n)+?>", RegexOptions.Compiled);
        private static readonly Regex SqlFilterRegex = new Regex("exec|insert|select|delete|'|update|chr|mid|master|truncate|char|declare|and|--", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex UnsafeSqlCharsRegex = new Regex(@"[-|;|,|\/|\(|\)|\[|\]|\}|\{|%|@|\*|!|\']", RegexOptions.Compiled);
        private static readonly Regex UnicodeRegex = new Regex(@"\\u([0-9A-F]{4})", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        #endregion

        /// <summary>
        /// Calculates the display width of a string, treating East Asian characters as 2 units wide and others as 1.
        /// </summary>
        /// <param name="source">The string to measure.</param>
        /// <returns>The calculated display width.</returns>
        public static int GetEastAsianWidthCount(string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                return 0;
            }
            int width = 0;
            foreach (char c in source)
            {
                // CJK characters and full-width forms are typically in this range.
                if (c >= 0x4E00 && c <= 0x9FA5 || c >= 0xFF00 && c <= 0xFFEF)
                {
                    width += 2;
                }
                else
                {
                    width += 1;
                }
            }
            return width;
        }

        /// <summary>
        /// Removes a specified suffix from the end of a string.
        /// </summary>
        /// <param name="source">The source string.</param>
        /// <param name="suffix">The suffix to remove. If null or empty, the last character is removed.</param>
        /// <returns>The trimmed string.</returns>
        public static string TrimEnd(string source, string suffix)
        {
            if (string.IsNullOrEmpty(source))
            {
                return source;
            }

            if (!string.IsNullOrEmpty(suffix) && source.EndsWith(suffix, StringComparison.Ordinal))
            {
                return source.Substring(0, source.Length - suffix.Length);
            }

            if (string.IsNullOrEmpty(suffix))
            {
                return source.Substring(0, source.Length - 1);
            }

            return source;
        }

        #region HTML Filtering

        /// <summary>
        /// Removes special characters to sanitize a string for use in a URL.
        /// </summary>
        public static string SanitizeForUrl(string source) => string.IsNullOrEmpty(source) ? source : UrlFilterRegex.Replace(source, "");

        /// <summary>
        /// Strips all 'a' (anchor) tags from a string.
        /// </summary>
        public static string StripATags(string source) => string.IsNullOrEmpty(source) ? source : ATagRegex.Replace(source, "");

        /// <summary>
        /// Strips all 'div' tags from a string.
        /// </summary>
        public static string StripDivTags(string source) => string.IsNullOrEmpty(source) ? source : DivTagRegex.Replace(source, "");

        /// <summary>
        /// Strips all 'font' tags from a string.
        /// </summary>
        public static string StripFontTags(string source) => string.IsNullOrEmpty(source) ? source : FontTagRegex.Replace(source, "");

        /// <summary>
        /// Strips all 'span' tags from a string.
        /// </summary>
        public static string StripSpanTags(string source) => string.IsNullOrEmpty(source) ? source : SpanTagRegex.Replace(source, "");

        /// <summary>
        /// Strips all 'table', 'tr', and 'td' tags from a string.
        /// </summary>
        public static string StripTableTags(string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                return source;
            }
            source = TableTagRegex.Replace(source, "");
            return source;
        }

        /// <summary>
        /// Strips all HTML tags from a string.
        /// </summary>
        public static string StripAllHtmlTags(string source) => string.IsNullOrEmpty(source) ? source : AllTagsRegex.Replace(source, "");

        /// <summary>
        /// Strips all 'img' tags from a string.
        /// </summary>
        public static string StripImgTags(string source) => string.IsNullOrEmpty(source) ? source : ImgTagRegex.Replace(source, "");

        /// <summary>
        /// Strips all 'object' tags from a string.
        /// </summary>
        public static string StripObjectTags(string source) => string.IsNullOrEmpty(source) ? source : ObjectTagRegex.Replace(source, "");

        /// <summary>
        /// Strips all 'script' tags from a string.
        /// </summary>
        public static string StripScriptTags(string source) => string.IsNullOrEmpty(source) ? source : ScriptTagRegex.Replace(source, "");

        /// <summary>
        /// Strips all 'iframe' tags from a string.
        /// </summary>
        public static string StripIframeTags(string source) => string.IsNullOrEmpty(source) ? source : IframeTagRegex.Replace(source, "");

        /// <summary>
        /// Strips all 'style' tags from a string.
        /// </summary>
        public static string StripStyleTags(string source) => string.IsNullOrEmpty(source) ? source : StyleTagRegex.Replace(source, "");

        /// <summary>
        /// Strips HTML elements based on a custom regex pattern.
        /// </summary>
        public static string StripHtmlByPattern(string source, string pattern)
        {
            if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(pattern))
            {
                return source;
            }
            return Regex.Replace(source, pattern, "", RegexOptions.IgnoreCase);
        }

        #endregion

        #region Data Security

        /// <summary>
        /// Filters a string to remove potentially dangerous SQL keywords.
        /// Warning: This is a basic blacklist and not a substitute for parameterized queries.
        /// </summary>
        public static string SanitizeSql(string source) => string.IsNullOrEmpty(source) ? source : SqlFilterRegex.Replace(source, " ");

        /// <summary>
        /// Encodes a string for safe use in XML by replacing special characters with their corresponding entities.
        /// </summary>
        public static string EncodeForXml(string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                return source;
            }
            var sb = new StringBuilder(source);
            sb.Replace("&", "&amp;");
            sb.Replace("<", "&lt;");
            sb.Replace(">", "&gt;");
            sb.Replace("'", "&apos;");
            sb.Replace("\"", "&quot;");
            return sb.ToString();
        }

        /// <summary>
        /// Checks for characters that are commonly used in SQL injection attacks.
        /// Warning: This is a basic check and not a comprehensive security solution.
        /// </summary>
        public static bool IsSqlSafe(string sql) => string.IsNullOrEmpty(sql) || !UnsafeSqlCharsRegex.IsMatch(sql);

        #endregion

        #region Unicode Conversion

        /// <summary>
        /// Converts a string to its Unicode escape sequence representation (e.g., "\uXXXX").
        /// </summary>
        /// <param name="source">The string to convert.</param>
        /// <param name="onlyConvertChinese">If true, only converts Chinese characters (CJK Unified Ideographs).</param>
        /// <returns>The Unicode-escaped string.</returns>
        public static string ToUnicode(string source, bool onlyConvertChinese = true)
        {
            if (string.IsNullOrEmpty(source))
            {
                return source;
            }

            var sb = new StringBuilder();
            foreach (char c in source)
            {
                if (onlyConvertChinese && !(c >= 0x4e00 && c <= 0x9fbb))
                {
                    sb.Append(c);
                }
                else
                {
                    sb.AppendFormat("\\u{0:x4}", (int)c);
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// Converts a string with Unicode escape sequences (e.g., "\uXXXX") back to its normal representation.
        /// </summary>
        public static string FromUnicode(string source) => string.IsNullOrEmpty(source) ? source : UnicodeRegex.Replace(source, m => ((char)Convert.ToUInt16(m.Groups[1].Value, 16)).ToString());

        #endregion

        /// <summary>
        /// Anonymizes a string by replacing characters with a specified symbol.
        /// </summary>
        /// <param name="source">The string to anonymize.</param>
        /// <param name="visibleStart">The number of characters to leave visible at the start.</param>
        /// <param name="visibleEnd">The number of characters to leave visible at the end.</param>
        /// <param name="replaceChar">The character to use for masking.</param>
        /// <returns>The anonymized string.</returns>
        public static string Anonymize(string source, int visibleStart, int visibleEnd, char replaceChar = '*')
        {
            if (string.IsNullOrEmpty(source))
            {
                return "";
            }

            int len = source.Length;
            if (visibleStart + visibleEnd >= len)
            {
                return source; // Not enough characters to mask
            }

            var sb = new StringBuilder();
            sb.Append(source.Substring(0, visibleStart));
            sb.Append(new string(replaceChar, len - visibleStart - visibleEnd));
            sb.Append(source.Substring(len - visibleEnd));

            return sb.ToString();
        }
    }
}