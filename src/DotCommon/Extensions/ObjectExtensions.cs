using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace DotCommon.Extensions
{
    /// <summary>
    /// 对象扩展
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// 对象强制转换
        /// </summary>
        public static T As<T>(this object o)
            where T : class
        {
            return (T)o;
        }

        /// <summary>
        /// 使用TypeDescriptor.GetConverter进行对象转换
        /// </summary>
        public static T To<T>(this object o)
            where T : struct
        {
            if (typeof(T) == typeof(Guid))
            {
                return (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromInvariantString(o.ToString());
            }

            return (T)Convert.ChangeType(o, typeof(T), CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// 判断某个值是否在集合中
        /// </summary>
        public static bool IsIn<T>(this T item, params T[] list)
        {
            return list.Contains(item);
        }
    }
}
