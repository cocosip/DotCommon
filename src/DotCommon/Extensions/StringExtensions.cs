using System;

namespace DotCommon.Extensions
{
    /// <summary>String类型扩展
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>获取字符串平台无关的Hashcode
        /// </summary>
        /// <param name="s">字符串</param>
        /// <returns></returns>
        public static int GetStringHashcode(this string s)
        {
            if (string.IsNullOrEmpty(s)) return 0;

            unchecked
            {
                int hash = 23;
                // ReSharper disable once LoopCanBeConvertedToQuery
                foreach (char c in s)
                {
                    hash = (hash << 5) - hash + c;
                }
                if (hash < 0)
                {
                    hash = Math.Abs(hash);
                }
                return hash;
            }
        }

        /// <summary>判断字符串是否为空
        /// </summary>
        /// <param name="source">字符串</param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string source)
        {
            return string.IsNullOrEmpty(source);
        }

        /// <summary>判断字符串是否为空或者空格
        /// </summary>
        /// <param name="source">字符串</param>
        /// <returns></returns>
        public static bool IsNullOrWhiteSpace(this string source)
        {
            return string.IsNullOrWhiteSpace(source);
        }


        /// <summary>截取指定长度的字符串
        /// </summary>
        /// <param name="source">字符串</param>
        /// <param name="len">截取长度</param>
        /// <returns></returns>
        public static string Left(this string source, int len)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (source.Length < len)
            {
                throw new ArgumentException("len argument can not be bigger than given string's length!");
            }

            return source.Substring(0, len);
        }

        /// <summary>按照正常行结尾格式化字符串
        /// </summary>
        /// <param name="source">字符串</param>
        /// <returns></returns>
        public static string NormalizeLineEndings(this string source)
        {
            return source.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", Environment.NewLine);
        }

        /// <summary>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="c"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static int NthIndexOf(this string source, char c, int n)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var count = 0;
            for (var i = 0; i < source.Length; i++)
            {
                if (source[i] != c)
                {
                    continue;
                }

                if ((++count) == n)
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>移除字符串中指定结尾格式的字符
        /// </summary>
        /// <param name="source">字符串</param>
        /// <param name="postFixes">结尾字符串数组</param>
        /// <returns></returns>
        public static string RemovePostFix(this string source, params string[] postFixes)
        {
            if (source.IsNullOrEmpty())
            {
                return null;
            }
            if (postFixes.IsNullOrEmpty())
            {
                return source;
            }

            foreach (var postFix in postFixes)
            {
                if (source.EndsWith(postFix))
                {
                    return source.Left(source.Length - postFix.Length);
                }
            }

            return source;
        }

        /// <summary>移除字符串中指定开始格式的前缀
        /// </summary>
        /// <param name="source">字符串</param>
        /// <param name="preFixes">前缀数组</param>
        /// <returns></returns>
        public static string RemovePreFix(this string source, params string[] preFixes)
        {
            if (source.IsNullOrEmpty())
            {
                return null;
            }

            if (preFixes.IsNullOrEmpty())
            {
                return source;
            }

            foreach (var preFix in preFixes)
            {
                if (source.StartsWith(preFix))
                {
                    return source.Right(source.Length - preFix.Length);
                }
            }

            return source;
        }

        /// <summary>截取字符串右侧指定长度的字符串
        /// </summary>
        public static string Right(this string source, int len)
        {
            if (source == null)
            {
                throw new ArgumentNullException("str");
            }

            if (source.Length < len)
            {
                throw new ArgumentException("len argument can not be bigger than given string's length!");
            }

            return source.Substring(source.Length - len, len);
        }

        /// <summary>
        /// Uses string.Split method to split given string by given separator.
        /// </summary>
        public static string[] Split(this string source, string separator)
        {
            return source.Split(new[] { separator }, StringSplitOptions.None);
        }

        /// <summary>
        /// Uses string.Split method to split given string by given separator.
        /// </summary>
        public static string[] Split(this string str, string separator, StringSplitOptions options)
        {
            return str.Split(new[] { separator }, options);
        }

        /// <summary>
        /// Uses string.Split method to split given string by <see cref="Environment.NewLine"/>.
        /// </summary>
        public static string[] SplitToLines(this string str)
        {
            return str.Split(Environment.NewLine);
        }

        /// <summary>
        /// Uses string.Split method to split given string by <see cref="Environment.NewLine"/>.
        /// </summary>
        public static string[] SplitToLines(this string str, StringSplitOptions options)
        {
            return str.Split(Environment.NewLine, options);
        }

        /// <summary>
        /// Converts PascalCase string to camelCase string.
        /// </summary>
        /// <param name="str">String to convert</param>
        /// <returns>camelCase of the string</returns>
        public static string ToCamelCase(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return str;
            }

            if (str.Length == 1)
            {
                return str.ToLowerInvariant();
            }

            return char.ToLowerInvariant(str[0]) + str.Substring(1);
        }

        /// <summary>转换成指定格式枚举
        /// </summary>
        public static T ToEnum<T>(this string value) where T : struct
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            return (T)Enum.Parse(typeof(T), value);
        }

        /// <summary>转换成指定格式枚举
        /// </summary>
        public static T ToEnum<T>(this string value, bool ignoreCase) where T : struct
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            return (T)Enum.Parse(typeof(T), value, ignoreCase);
        }
    }
}
