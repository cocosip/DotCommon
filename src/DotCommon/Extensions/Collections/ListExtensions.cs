using System;
using System.Collections.Generic;
using System.Linq;

namespace DotCommon.Extensions
{
    public static class ListExtensions
    {
        /// <summary>移除某个项
        /// </summary>
        public static bool Remove<T>(this List<T> collection, Func<T, bool> predicate)
        {
            return collection.Remove(collection.FirstOrDefault(predicate));
        }

    }
}
