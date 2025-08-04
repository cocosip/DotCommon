using System;
using System.Collections.Generic;
using System.Linq;

namespace DotCommon.Collections.Generic
{
    /// <summary>
    /// Utility class for collection operations
    /// </summary>
    public static class CollectionUtil
    {
        /// <summary>
        /// Groups the specified list by maximum count
        /// </summary>
        /// <typeparam name="T">The type of elements in the list</typeparam>
        /// <param name="source">The source list to group</param>
        /// <param name="maxCount">The maximum number of elements per group</param>
        /// <returns>A list of groups, each group contains no more than maxCount elements</returns>
        public static List<List<T>> GroupByMaxCount<T>(List<T> source, int maxCount)
        {
            var result = new List<List<T>>();
            List<T>? currentGroup = null;
            foreach (var item in source)
            {
                // Create a new group when current group is null or has reached max count
                if (currentGroup == null || currentGroup.Count >= maxCount)
                {
                    currentGroup = [];
                    result.Add(currentGroup);
                }
                currentGroup.Add(item);
            }
            return result;
        }

        /// <summary>
        /// Groups the specified list by maximum count after sorting by key selector
        /// </summary>
        /// <typeparam name="T">The type of elements in the list</typeparam>
        /// <typeparam name="Key">The type of the key returned by keySelector</typeparam>
        /// <param name="source">The source list to group</param>
        /// <param name="maxCount">The maximum number of elements per group</param>
        /// <param name="keySelector">A function to extract the key for each element</param>
        /// <param name="ascending">true to sort in ascending order; false for descending order</param>
        /// <returns>A list of groups, each group contains no more than maxCount elements</returns>
        public static List<List<T>> GroupByMaxCount<T, Key>(List<T> source, int maxCount, Func<T, Key> keySelector, bool ascending = true)
        {
            var result = new List<List<T>>();
            List<T>? currentGroup = null;
            // Order the source list based on the ascending parameter
            var orderedEnumerable = ascending ?
                source.OrderBy(keySelector) :
                source.OrderByDescending(keySelector);

            foreach (var item in orderedEnumerable)
            {
                // Create a new group when current group is null or has reached max count
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
