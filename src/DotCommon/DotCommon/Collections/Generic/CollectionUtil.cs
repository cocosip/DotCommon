using System;
using System.Collections.Generic;
using System.Linq;

namespace DotCommon.Collections.Generic
{
    public static class CollectionUtil
    {
        public static List<List<T>> GroupByMaxCount<T>(List<T> source, int maxCount)
        {
            var result = new List<List<T>>();
            List<T>? currentGroup = null;
            foreach (var item in source)
            {
                if (currentGroup == null || currentGroup.Count >= maxCount)
                {
                    currentGroup = [];
                    result.Add(currentGroup);
                }
                currentGroup.Add(item);
            }
            return result;
        }

        public static List<List<T>> GroupByMaxCount<T, Key>(List<T> source, int maxCount, Func<T, Key> keySelector, bool ascending = true)
        {
            var result = new List<List<T>>();
            List<T>? currentGroup = null;
            var orderedEnumerable = ascending ?
                source.OrderBy(keySelector) :
                source.OrderByDescending(keySelector);

            foreach (var item in orderedEnumerable)
            {
                if (currentGroup == null || currentGroup.Count >= maxCount)
                {
                    currentGroup = [];
                    result.Add(currentGroup);
                }
                currentGroup.Add(item);
            }
            return result;
        }

    }
}
