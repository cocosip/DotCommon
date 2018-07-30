using System;
using System.Text;
using System.Web;

namespace DotCommon.Http
{
    public static class StringExtensions
    {
        public static string UrlDecode(this string input) => HttpUtility.UrlDecode(input);

        public static string UrlEncode(this string input)
        {
            const int maxLength = 32766;

            if (input == null)
                throw new ArgumentNullException("input");

            if (input.Length <= maxLength)
                return Uri.EscapeDataString(input);

            var sb = new StringBuilder(input.Length * 2);
            var index = 0;

            while (index < input.Length)
            {
                var length = Math.Min(input.Length - index, maxLength);
                var subString = input.Substring(index, length);

                sb.Append(Uri.EscapeDataString(subString));
                index += subString.Length;
            }

            return sb.ToString();
        }

        public static string HtmlDecode(this string input) => HttpUtility.HtmlDecode(input);

        public static string HtmlEncode(this string input) => HttpUtility.HtmlEncode(input);

        public static string UrlEncode(this string input, Encoding encoding) => HttpUtility.UrlEncode(input, encoding);

        public static string HtmlAttributeEncode(this string input) => HttpUtility.HtmlAttributeEncode(input);

        /// <summary>检测是否为空
        /// </summary>
        public static bool HasValue(this string input) => !string.IsNullOrEmpty(input);

        /// <summary>移除下划线
        /// </summary>
        public static string RemoveUnderscoresAndDashes(this string input) =>
            input.Replace("_", "").Replace("-", "");



    }
}
