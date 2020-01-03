using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DotCommon.Extensions
{
    /// <summary>IEnumerable接口的扩展
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>将IEnumerable对象拼接成字符串
        /// </summary>
        /// <param name="enumerable">IEnumerable对象</param>
        /// <param name="separator">分隔符</param>
        /// <returns></returns>
        public static string JoinAsString(this IEnumerable<string> enumerable, string separator)
        {
            return string.Join(separator, enumerable);
        }

        /// <summary>将IEnumerable对象拼接成字符串
        /// </summary>
        /// <param name="enumerable">IEnumerable对象</param>
        /// <param name="separator">分隔符</param>
        /// <returns></returns>
        public static string JoinAsString<T>(this IEnumerable<T> enumerable, string separator)
        {
            return string.Join(separator, enumerable);
        }

        /// <summary>根据条件执行Func
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="enumerable">IEnumerable对象</param>
        /// <param name="condition">条件</param>
        /// <param name="predicate">Func委托函数</param>
        /// <returns></returns>
        public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> enumerable, bool condition, Func<T, bool> predicate)
        {
            return condition
                ? enumerable.Where(predicate)
                : enumerable;
        }

        /// <summary>根据条件执行Func
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="enumerable">IEnumerable对象</param>
        /// <param name="condition">条件</param>
        /// <param name="predicate">Func委托函数</param>
        /// <returns></returns>
        public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> enumerable, bool condition, Func<T, int, bool> predicate)
        {
            return condition
                ? enumerable.Where(predicate)
                : enumerable;
        }


        /// <summary>是否为安全的IEnumerable对象
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="enumerable">IEnumerable对象</param>
        /// <returns></returns>
        public static IEnumerable<T> Safe<T>(this IEnumerable<T> enumerable)
        {
            return enumerable ?? Enumerable.Empty<T>();
        }

        /// <summary>判断IEnumerable对象是否包含某一些元素
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="enumerable">IEnumerable对象</param>
        /// <param name="condition">条件委托</param>
        /// <returns></returns>
        public static bool Contains<T>(this IEnumerable<T> enumerable, Predicate<T> condition)
        {
            return enumerable.Any(x => condition(x));
        }

        /// <summary>判断IEnumerable对象是否为空
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="enumerable">IEnumerable对象</param>
        /// <returns></returns>
        public static bool IsEmpty<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable == null)
            {
                return true;
            }
            var coll = enumerable as ICollection;
            if (coll != null)
            {
                return coll.Count == 0;
            }
            return !enumerable.Any();
        }

        /// <summary>判断IEnumerable对象是否不为空
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="enumerable">IEnumerable对象</param>
        /// <returns></returns>
        public static bool IsNotEmpty<T>(this IEnumerable<T> enumerable)
        {
            return !IsEmpty(enumerable);
        }

    }
}
