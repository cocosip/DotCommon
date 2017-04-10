using System;

namespace DotCommon.Utility
{
    /// <summary>枚举工具类
    /// </summary>
    public class EnumUtil
    {
        /// <summary>将枚举转换成String类型
        /// </summary>
        public static string ToStr(Enum e)
        {
            return Enum.GetName(e.GetType(), e);
        }

        /// <summary>将String类型转换成枚举类型
        /// </summary>
        public static T FromStr<T>(string str)
        {
            return (T) Enum.Parse(typeof (T), str);
        }

        /// <summary>获取枚举中的全部类型的数量
        /// </summary>
        public static int GetEnumCount<T>()
        {
            return Enum.GetNames(typeof (T)).Length;
        }

        /// <summary>获取全部的枚举
        /// </summary>
        public static Array GetValues<T>()
        {
            return Enum.GetValues(typeof (T));
        }

        /// <summary>判断该值是否为指定枚举中的值
        /// </summary>
        public static bool InEnum<T>(object value)
        {
            return Enum.IsDefined(typeof (T), value);
        }
    }
}
