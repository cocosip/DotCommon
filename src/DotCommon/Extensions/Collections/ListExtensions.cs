using System;
using System.Collections.Generic;
using System.Linq;

namespace DotCommon.Extensions
{
    /// <summary>List扩展
    /// </summary>
    public static class ListExtensions
    {
        /// <summary>移除某个项
        /// </summary>
        public static bool Remove<T>(this List<T> collection, Func<T, bool> predicate)
        {
            return collection.Remove(collection.FirstOrDefault(predicate));
        }


        /// <summary>比较两个集合是否相等
        /// </summary>
        public static bool EqualList<T>(this List<T> source, List<T> other)
        {
            if (source.Count != other.Count)
            {
                return false;
            }

            for (int i = 0; i < source.Count; i++)
            {
                if (!source[i].Equals(other[i]))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
