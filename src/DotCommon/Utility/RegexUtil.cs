using DotCommon.Extensions;
using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace DotCommon.Utility
{
    /// <summary>
    /// 正则表达式工具类
    /// </summary>
    public static class RegexUtil
    {
        /// <summary>
        /// 匹配正则表达式
        /// </summary>
        /// <param name="source">字符串</param>
        /// <param name="pattern">正则表达式</param>
        /// <returns></returns>
        public static bool IsMatch(string source, string pattern)
        {
            return IsMatch(source, pattern, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 匹配正则表达式
        /// </summary>
        /// <param name="source">字符串</param>
        /// <param name="pattern">正则表达式</param>
        /// <param name="options">正则表达式属性</param>
        /// <returns></returns>
        public static bool IsMatch(string source, string pattern, RegexOptions options)
        {
            var regex = new Regex(pattern, options);
            return NotNull(source) && regex.IsMatch(source);
        }


        /// <summary>
        /// 判断是否为空
        /// </summary>
        /// <param name="source">字符串</param>
        /// <returns></returns>
        private static bool NotNull(string source)
        {
            return !source.IsNullOrWhiteSpace();
        }


        /// <summary>
        /// 验证是否为手机号码
        /// </summary>
        /// <param name="source">字符串</param>
        /// <returns></returns>
        public static bool IsMobileNumber(string source)
        {
            const string pattern = @"^1+\d{10}$";
            return IsMatch(source, pattern);
        }

        /// <summary>
        /// 验证是否为Email地址
        /// </summary>
        /// <param name="source">字符串</param>
        /// <returns></returns>
        public static bool IsEmailAddress(string source)
        {
            const string pattern =
                @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            return IsMatch(source, pattern);
        }

        /// <summary>
        /// 验证是否为Url地址
        /// </summary>
        /// <param name="source">字符串</param>
        /// <returns></returns>
        public static bool IsUrl(string source)
        {
            const string pattern =
                @"^(http|https)\://([a-zA-Z0-9\.\-]+(\:[a-zA-Z0-9\.&%\$\-]+)*@)*((25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9])\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[0-9])|localhost|([a-zA-Z0-9\-]+\.)*[a-zA-Z0-9\-]+\.(com|edu|gov|int|mil|net|org|biz|arpa|info|name|pro|aero|coop|museum|[a-zA-Z]{1,10}))(\:[0-9]+)*(/($|[a-zA-Z0-9\.\,\?\'\\\+&%\$#\=~_\-]+))*$";
            return IsMatch(source, pattern);
        }

        /// <summary>
        /// 是否包含中文
        /// </summary>
        public static bool IsChinese(string source)
        {
            const string pattern = "^[/u4e00-/u9fa5]$";
            return IsMatch(source, pattern);
        }

        /// <summary>
        /// 是否为IP地址
        /// </summary>
        /// <param name="source">字符串</param>
        /// <returns></returns>
        public static bool IsIp(string source)
        {
            const string pattern =
                @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$";
            return IsMatch(source, pattern);
        }

        /// <summary>
        /// 是否为1-9的正整数
        /// </summary>
        /// <param name="source">字符串</param>
        /// <returns></returns>
        public static bool IsPositiveInteger(string source)
        {
            const string pattern = @"^[1-9]+\d*$";
            return IsMatch(source, pattern);
        }

        /// <summary>
        /// 是否为Int32类型
        /// </summary>
        /// <param name="source">字符串</param>
        /// <returns></returns>
        public static bool IsInt32(string source)
        {
            const string pattern = @"^(\-|\+)?[0-9]*$";
            return IsMatch(source, pattern);
        }

        /// <summary>
        /// 是否为double类型
        /// </summary>
        /// <param name="source">字符串</param>
        /// <param name="digit">数据位数</param>
        /// <returns></returns>
        public static bool IsDouble(string source, int digit = 3)
        {
            string pattern = $@"^\d{{1,9}}[.]?\d{{0,{digit}}}$";
            return IsMatch(source, pattern);
        }

        /// <summary>
        /// 判断是否为double类型
        /// </summary>
        /// <param name="source">字符串</param>
        /// <param name="minValue">最小值</param>
        /// <param name="maxValue">最大值</param>
        /// <param name="digit">数据位数</param>
        /// <returns></returns>
        public static bool IsDouble(string source, double minValue, double maxValue, int digit = 3)
        {
            string patten = $@"^\d{{1,9}}[.]?\d{{0,{digit}}}$";
            if (IsMatch(source, patten))
            {
                double val = Convert.ToDouble(source);
                if (val >= minValue && val <= maxValue)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 判断是否为decimal类型
        /// </summary>
        /// <param name="source">字符串</param>
        /// <param name="digit">数据位数</param>
        /// <returns></returns>
        public static bool IsDecimal(string source, int digit = 3)
        {
            string patten = $@"^\d{{1,9}}[.]?\d{{0,{digit}}}$";
            return IsMatch(source, patten);
        }

        /// <summary>
        /// 判断是否为decimal类型
        /// </summary>
        /// <param name="source">字符串</param>
        /// <param name="minValue">最小值</param>
        /// <param name="maxValue">最大值</param>
        /// <param name="digit">数据位数</param>
        /// <returns></returns>
        public static bool IsDecimal(string source, decimal minValue, decimal maxValue, int digit = 3)
        {
            string patten = $@"^\d{{1,9}}[.]?\d{{0,{digit}}}$";
            if (IsMatch(source, patten))
            {
                decimal val = Convert.ToDecimal(source);
                if (val >= minValue && val <= maxValue)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 是否为有效的日期时间
        /// </summary>
        /// <param name="source">字符串</param>
        /// <returns></returns>
        public static bool IsDataTime(string source)
        {
            return NotNull(source) && DateTime.TryParse(source, CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
        }


        /// <summary>
        /// 是否为有效的版本号 1.3,1.1.5,1.25.256
        /// </summary>
        /// <param name="source">字符串</param>
        /// <param name="len">长度</param>
        /// <returns></returns>
        public static bool IsVersion(string source, int len = 5)
        {
            string pattern = $@"^\d{{0,6}}\.(\d{{1,6}}\.){{0,{len}}}\d{{1,6}}$";
            return IsMatch(source, pattern);
        }


        /// <summary>
        /// 是否为新版本,后面的版本版是否大于前面的版本
        /// </summary>
        /// <param name="oldVersion">原版本</param>
        /// <param name="newVersion">新版本</param>
        /// <returns></returns>
        public static bool IsVersionUpper(string oldVersion, string newVersion)
        {
            if (!IsVersion(oldVersion))
            {
                throw new ArgumentException($"旧版本 oldVersion:{oldVersion}不是一个有效的版本号.");
            }
            if (!IsVersion(newVersion))
            {
                throw new ArgumentException($"新版本 newVersion:{newVersion}不是一个有效的版本号.");
            }

            string[] strOld = oldVersion.Split('.');
            string[] strNew = newVersion.Split('.');
            int length = strOld.Length > strNew.Length ? strNew.Length : strOld.Length;
            for (int i = 0; i < length; i++)
            {
                if (Convert.ToInt32(strOld[i]) == Convert.ToInt32(strNew[i]))
                {
                    continue;
                }
                //如果判断新版本比较高,则直接返回
                if (Convert.ToInt32(strOld[i]) < Convert.ToInt32(strNew[i]))
                {
                    return true;
                }
                return false;
            }
            //如果后面的版本长度大于前面的,那么就为true
            if (strNew.Length > strOld.Length)
            {
                return true;
            }

            return false;
        }

    }
}
