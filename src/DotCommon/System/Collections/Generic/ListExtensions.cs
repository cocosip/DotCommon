﻿﻿﻿using System.Linq;
using DotCommon;
using JetBrains.Annotations;

namespace System.Collections.Generic
{
    /// <summary>
    /// Extension methods for List collections
    /// </summary>
    public static class ListExtensions
    {
        /// <summary>
        /// Removes the first occurrence of an item that matches the specified predicate
        /// </summary>
        /// <typeparam name="T">The type of elements in the list</typeparam>
        /// <param name="collection">The list to remove from</param>
        /// <param name="predicate">The function to test each element for a condition</param>
        /// <returns>True if an item was removed; otherwise, false</returns>
        public static bool Remove<T>(this List<T> collection, Func<T, bool> predicate)
        {
            return collection.Remove(collection.FirstOrDefault(predicate));
        }

        /// <summary>
        /// Compares two lists for equality
        /// </summary>
        /// <typeparam name="T">The type of elements in the lists</typeparam>
        /// <param name="source">The first list to compare</param>
        /// <param name="other">The second list to compare</param>
        /// <param name="comparer">The equality comparer to use for comparing elements</param>
        /// <returns>True if the lists are equal; otherwise, false</returns>
        public static bool EqualList<T>(
            this List<T> source,
            List<T> other,
            IEqualityComparer<T>? comparer = null)
        {
            // Handle the scenario where both lists are null or the same reference
            if (ReferenceEquals(source, other)) return true;
            // Handle the scenario where one of them is null
            if (source == null || other == null) return false;
            // Quick length check
            if (source.Count != other.Count) return false;

            // Use custom comparer or default comparer
            var equalityComparer = comparer ?? EqualityComparer<T>.Default;
            // Use LINQ's SequenceEqual to optimize comparison logic
            return source.SequenceEqual(other, equalityComparer);
        }

        /// <summary>
        /// Inserts a range of items into the list at the specified index
        /// </summary>
        /// <typeparam name="T">The type of elements in the list</typeparam>
        /// <param name="source">The list to insert into</param>
        /// <param name="index">The zero-based index at which to insert the items</param>
        /// <param name="items">The collection of items to insert</param>
        public static void InsertRange<T>(this IList<T> source, int index, IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                source.Insert(index++, item);
            }
        }

        /// <summary>
        /// Searches for an element that matches the conditions defined by the specified predicate and returns the zero-based index of the first occurrence
        /// </summary>
        /// <typeparam name="T">The type of elements in the list</typeparam>
        /// <param name="source">The list to search</param>
        /// <param name="selector">The predicate that defines the conditions of the element to search for</param>
        /// <returns>The zero-based index of the first occurrence of an element that matches the conditions; otherwise, -1</returns>
        public static int FindIndex<T>(this IList<T> source, Predicate<T> selector)
        {
            for (var i = 0; i < source.Count; ++i)
            {
                if (selector(source[i]))
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// Adds an item to the beginning of the list
        /// </summary>
        /// <typeparam name="T">The type of elements in the list</typeparam>
        /// <param name="source">The list to add to</param>
        /// <param name="item">The item to add</param>
        public static void AddFirst<T>(this IList<T> source, T item)
        {
            source.Insert(0, item);
        }

        /// <summary>
        /// Adds an item to the end of the list
        /// </summary>
        /// <typeparam name="T">The type of elements in the list</typeparam>
        /// <param name="source">The list to add to</param>
        /// <param name="item">The item to add</param>
        public static void AddLast<T>(this IList<T> source, T item)
        {
            source.Insert(source.Count, item);
        }

        /// <summary>
        /// Inserts an item after the specified existing item
        /// </summary>
        /// <typeparam name="T">The type of elements in the list</typeparam>
        /// <param name="source">The list to insert into</param>
        /// <param name="existingItem">The existing item to insert after</param>
        /// <param name="item">The item to insert</param>
        public static void InsertAfter<T>(this IList<T> source, T existingItem, T item)
        {
            var index = source.IndexOf(existingItem);
            if (index < 0)
            {
                source.AddFirst(item);
                return;
            }

            source.Insert(index + 1, item);
        }

        /// <summary>
        /// Inserts an item after the first item that matches the specified predicate
        /// </summary>
        /// <typeparam name="T">The type of elements in the list</typeparam>
        /// <param name="source">The list to insert into</param>
        /// <param name="selector">The predicate to find the item to insert after</param>
        /// <param name="item">The item to insert</param>
        public static void InsertAfter<T>(this IList<T> source, Predicate<T> selector, T item)
        {
            var index = source.FindIndex(selector);
            if (index < 0)
            {
                source.AddFirst(item);
                return;
            }

            source.Insert(index + 1, item);
        }

        /// <summary>
        /// Inserts an item before the specified existing item
        /// </summary>
        /// <typeparam name="T">The type of elements in the list</typeparam>
        /// <param name="source">The list to insert into</param>
        /// <param name="existingItem">The existing item to insert before</param>
        /// <param name="item">The item to insert</param>
        public static void InsertBefore<T>(this IList<T> source, T existingItem, T item)
        {
            var index = source.IndexOf(existingItem);
            if (index < 0)
            {
                source.AddLast(item);
                return;
            }

            source.Insert(index, item);
        }

        /// <summary>
        /// Inserts an item before the first item that matches the specified predicate
        /// </summary>
        /// <typeparam name="T">The type of elements in the list</typeparam>
        /// <param name="source">The list to insert into</param>
        /// <param name="selector">The predicate to find the item to insert before</param>
        /// <param name="item">The item to insert</param>
        public static void InsertBefore<T>(this IList<T> source, Predicate<T> selector, T item)
        {
            var index = source.FindIndex(selector);
            if (index < 0)
            {
                source.AddLast(item);
                return;
            }

            source.Insert(index, item);
        }

        /// <summary>
        /// Replaces all items that match the specified predicate with the given item
        /// </summary>
        /// <typeparam name="T">The type of elements in the list</typeparam>
        /// <param name="source">The list to modify</param>
        /// <param name="selector">The predicate to find items to replace</param>
        /// <param name="item">The replacement item</param>
        public static void ReplaceWhile<T>(this IList<T> source, Predicate<T> selector, T item)
        {
            for (int i = 0; i < source.Count; i++)
            {
                if (selector(source[i]))
                {
                    source[i] = item;
                }
            }
        }

        /// <summary>
        /// Replaces all items that match the specified predicate with items generated by the factory function
        /// </summary>
        /// <typeparam name="T">The type of elements in the list</typeparam>
        /// <param name="source">The list to modify</param>
        /// <param name="selector">The predicate to find items to replace</param>
        /// <param name="itemFactory">The function to generate replacement items</param>
        public static void ReplaceWhile<T>(this IList<T> source, Predicate<T> selector, Func<T, T> itemFactory)
        {
            for (int i = 0; i < source.Count; i++)
            {
                var item = source[i];
                if (selector(item))
                {
                    source[i] = itemFactory(item);
                }
            }
        }

        /// <summary>
        /// Replaces the first item that matches the specified predicate with the given item
        /// </summary>
        /// <typeparam name="T">The type of elements in the list</typeparam>
        /// <param name="source">The list to modify</param>
        /// <param name="selector">The predicate to find the item to replace</param>
        /// <param name="item">The replacement item</param>
        public static void ReplaceOne<T>(this IList<T> source, Predicate<T> selector, T item)
        {
            for (int i = 0; i < source.Count; i++)
            {
                if (selector(source[i]))
                {
                    source[i] = item;
                    return;
                }
            }
        }

        /// <summary>
        /// Replaces the first item that matches the specified predicate with an item generated by the factory function
        /// </summary>
        /// <typeparam name="T">The type of elements in the list</typeparam>
        /// <param name="source">The list to modify</param>
        /// <param name="selector">The predicate to find the item to replace</param>
        /// <param name="itemFactory">The function to generate the replacement item</param>
        public static void ReplaceOne<T>(this IList<T> source, Predicate<T> selector, Func<T, T> itemFactory)
        {
            for (int i = 0; i < source.Count; i++)
            {
                var item = source[i];
                if (selector(item))
                {
                    source[i] = itemFactory(item);
                    return;
                }
            }
        }

        /// <summary>
        /// Replaces the first occurrence of a specific item with another item
        /// </summary>
        /// <typeparam name="T">The type of elements in the list</typeparam>
        /// <param name="source">The list to modify</param>
        /// <param name="item">The item to replace</param>
        /// <param name="replaceWith">The replacement item</param>
        public static void ReplaceOne<T>(this IList<T> source, T item, T replaceWith)
        {
            for (int i = 0; i < source.Count; i++)
            {
                if (Comparer<T>.Default.Compare(source[i], item) == 0)
                {
                    source[i] = replaceWith;
                    return;
                }
            }
        }

        /// <summary>
        /// Moves an item that matches the specified predicate to the target index
        /// </summary>
        /// <typeparam name="T">The type of elements in the list</typeparam>
        /// <param name="source">The list to modify</param>
        /// <param name="selector">The predicate to find the item to move</param>
        /// <param name="targetIndex">The target index to move the item to</param>
        /// <exception cref="IndexOutOfRangeException">Thrown when targetIndex is out of range</exception>
        public static void MoveItem<T>(this List<T> source, Predicate<T> selector, int targetIndex)
        {
            if (!targetIndex.IsBetween(0, source.Count - 1))
            {
                throw new IndexOutOfRangeException("targetIndex should be between 0 and " + (source.Count - 1));
            }

            var currentIndex = source.FindIndex(0, selector);
            if (currentIndex == targetIndex)
            {
                return;
            }

            var item = source[currentIndex];
            source.RemoveAt(currentIndex);
            source.Insert(targetIndex, item);
        }

        /// <summary>
        /// Gets an item from the list that matches the selector, or adds a new item if not found
        /// </summary>
        /// <typeparam name="T">The type of elements in the list</typeparam>
        /// <param name="source">The list to search and potentially add to</param>
        /// <param name="selector">The function to test each element for a condition</param>
        /// <param name="factory">The function to create a new item if none is found</param>
        /// <returns>The existing item if found, or the newly created and added item</returns>
        [NotNull]
        public static T GetOrAdd<T>([NotNull] this IList<T> source, Func<T, bool> selector, Func<T> factory)
        {
            Check.NotNull(source, nameof(source));

            var item = source.FirstOrDefault(selector);

            if (item == null)
            {
                item = factory();
                source.Add(item);
            }

            return item;
        }

        /// <summary>
        /// Sorts a list using topological sorting, considering their dependencies
        /// </summary>
        /// <typeparam name="T">The type of the members of values</typeparam>
        /// <param name="source">A list of objects to sort</param>
        /// <param name="getDependencies">Function to resolve the dependencies</param>
        /// <param name="comparer">Equality comparer for dependencies</param>
        /// <returns>
        /// Returns a new list ordered by dependencies.
        /// If A depends on B, then B will come before A in the resulting list.
        /// </returns>
        public static List<T> SortByDependencies<T>(
            this IEnumerable<T> source,
            Func<T, IEnumerable<T>> getDependencies,
            IEqualityComparer<T>? comparer = null) where T : notnull
        {
            /* See: http://www.codeproject.com/Articles/869059/Topological-sorting-in-Csharp
             *      http://en.wikipedia.org/wiki/Topological_sorting
             */

            var sorted = new List<T>();
            var visited = new Dictionary<T, bool>(comparer);

            foreach (var item in source)
            {
                SortByDependenciesVisit(item, getDependencies, sorted, visited);
            }

            return sorted;
        }

        /// <summary>
        /// Visits an item and its dependencies recursively for topological sorting
        /// </summary>
        /// <typeparam name="T">The type of the members of values</typeparam>
        /// <param name="item">Item to resolve</param>
        /// <param name="getDependencies">Function to resolve the dependencies</param>
        /// <param name="sorted">List with the sorted items</param>
        /// <param name="visited">Dictionary with the visited items</param>
        /// <exception cref="ArgumentException">Thrown when a cyclic dependency is found</exception>
        private static void SortByDependenciesVisit<T>(T item, Func<T, IEnumerable<T>> getDependencies, List<T> sorted,
            Dictionary<T, bool> visited) where T : notnull
        {
            bool inProcess;
            var alreadyVisited = visited.TryGetValue(item, out inProcess);

            if (alreadyVisited)
            {
                if (inProcess)
                {
                    throw new ArgumentException("Cyclic dependency found! Item: " + item);
                }
            }
            else
            {
                visited[item] = true;

                var dependencies = getDependencies(item);
                if (dependencies != null)
                {
                    foreach (var dependency in dependencies)
                    {
                        SortByDependenciesVisit(dependency, getDependencies, sorted, visited);
                    }
                }

                visited[item] = false;
                sorted.Add(item);
            }
        }
    }
}
