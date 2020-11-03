using System;

namespace DotCommon.Utility
{
    /// <summary>
    /// 转换工厂类
    /// </summary>
    public static class ConvertFactory
    {
        /// <summary>
        /// 转换成Int32类型
        /// </summary>
        /// <param name="source">源对象</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static int ToInt32(object source, int defaultValue)
        {
            if (source != null)
            {
                if (int.TryParse(source.ToString(), out int value))
                {
                    return value;
                }
            }
            return defaultValue;
        }

        /// <summary>
        /// 转换成Int64类型
        /// </summary>
        /// <param name="source">源对象</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static long ToInt64(object source, long defaultValue)
        {
            if (source != null)
            {
                if (long.TryParse(source.ToString(), out long value))
                {
                    return value;
                }
            }
            return defaultValue;
        }

        /// <summary>
        /// 转换成Double类型
        /// </summary>
        /// <param name="source">源对象</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static double ToDouble(object source, double defaultValue)
        {
            if (source != null)
            {
                if (double.TryParse(source.ToString(), out double value))
                {
                    return value;
                }
            }
            return defaultValue;
        }

        /// <summary>
        /// 转换成double类型,并保留有效的位数
        /// </summary>
        /// <param name="source">源对象</param>
        /// <param name="defaultValue">默认值</param>
        /// <param name="digit">有效位数</param>
        /// <returns></returns>
        public static double ToDouble(object source, double defaultValue, int digit)
        {
            if (source != null)
            {
                if (double.TryParse(source.ToString(), out double value))
                {
                    return Math.Round(value, digit);
                }
            }
            return Math.Round(defaultValue, digit);
        }

        /// <summary>
        /// 转换成Datetime
        /// </summary>
        /// <param name="source">源对象</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static DateTime ToDateTime(object source, DateTime defaultValue)
        {
            if (source != null)
            {
                if (DateTime.TryParse(source.ToString(), out DateTime dateTime))
                {
                    return dateTime;
                }
            }
            return defaultValue;
        }

        /// <summary>
        /// 转换成Bool类型
        /// </summary>
        /// <param name="source">源对象</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static bool ToBool(object source, bool defaultValue)
        {
            if (source != null)
            {
                if (bool.TryParse(source.ToString(), out bool value))
                {
                    return value;
                }
            }
            return defaultValue;
        }

        /// <summary>
        /// 将string类型字符串转换成对应的guid
        /// </summary>
        /// <param name="source">字符串</param>
        /// <returns></returns>
        public static Guid ToGuid(string source)
        {
            return new Guid(source);
        }

    }
}
