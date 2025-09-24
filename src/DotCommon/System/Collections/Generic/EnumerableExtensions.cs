using System.Linq;

namespace System.Collections.Generic
{
    /// <summary>
    /// Extension methods for IEnumerable interface
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Joins the elements of an IEnumerable of strings into a single string
        /// </summary>
        /// <param name="enumerable">The IEnumerable object</param>
        /// <param name="separator">The separator string</param>
        /// <returns>A concatenated string of the enumerable elements</returns>
        public static string JoinAsString(this IEnumerable<string> enumerable, string separator)
        {
            return string.Join(separator, enumerable);
        }

        /// <summary>
        /// Joins the elements of an IEnumerable into a single string
        /// </summary>
        /// <typeparam name="T">The type of elements in the enumerable</typeparam>
        /// <param name="enumerable">The IEnumerable object</param>
        /// <param name="separator">The separator string</param>
        /// <returns>A concatenated string of the enumerable elements</returns>
        public static string JoinAsString<T>(this IEnumerable<T> enumerable, string separator)
        {
            return string.Join(separator, enumerable);
        }

        /// <summary>
        /// Conditionally filters a sequence of values based on a predicate
        /// </summary>
        /// <typeparam name="T">The type of elements in the enumerable</typeparam>
        /// <param name="enumerable">The IEnumerable object</param>
        /// <param name="condition">The condition to determine whether to apply the filter</param>
        /// <param name="predicate">The function to test each element for a condition</param>
        /// <returns>An IEnumerable that contains elements from the input sequence that satisfy the condition (if the condition is true)</returns>
        public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> enumerable, bool condition, Func<T, bool> predicate)
        {
            return condition
                ? enumerable.Where(predicate)
                : enumerable;
        }

        /// <summary>
        /// Conditionally filters a sequence of values based on a predicate that incorporates the element's index
        /// </summary>
        /// <typeparam name="T">The type of elements in the enumerable</typeparam>
        /// <param name="enumerable">The IEnumerable object</param>
        /// <param name="condition">The condition to determine whether to apply the filter</param>
        /// <param name="predicate">The function to test each element and its index for a condition</param>
        /// <returns>An IEnumerable that contains elements from the input sequence that satisfy the condition (if the condition is true)</returns>
        public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> enumerable, bool condition, Func<T, int, bool> predicate)
        {
            return condition
                ? enumerable.Where(predicate)
                : enumerable;
        }


        /// <summary>
        /// Returns a safe enumerable that is never null
        /// </summary>
        /// <typeparam name="T">The type of elements in the enumerable</typeparam>
        /// <param name="enumerable">The IEnumerable object</param>
        /// <returns>The original enumerable if not null, otherwise an empty enumerable</returns>
        public static IEnumerable<T> Safe<T>(this IEnumerable<T> enumerable)
        {
            return enumerable ?? Enumerable.Empty<T>();
        }

        /// <summary>
        /// Determines whether the enumerable contains any element that satisfies the specified condition
        /// </summary>
        /// <typeparam name="T">The type of elements in the enumerable</typeparam>
        /// <param name="enumerable">The IEnumerable object</param>
        /// <param name="condition">The condition predicate to test each element</param>
        /// <returns>True if any element satisfies the condition; otherwise, false</returns>
        public static bool Contains<T>(this IEnumerable<T> enumerable, Predicate<T> condition)
        {
            return enumerable.Any(x => condition(x));
        }

        /// <summary>
        /// Determines whether the enumerable is null or contains no elements
        /// </summary>
        /// <typeparam name="T">The type of elements in the enumerable</typeparam>
        /// <param name="enumerable">The IEnumerable object</param>
        /// <returns>True if the enumerable is null or empty; otherwise, false</returns>
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

        /// <summary>
        /// Determines whether the enumerable is not null and contains at least one element
        /// </summary>
        /// <typeparam name="T">The type of elements in the enumerable</typeparam>
        /// <param name="enumerable">The IEnumerable object</param>
        /// <returns>True if the enumerable is not null and not empty; otherwise, false</returns>
        public static bool IsNotEmpty<T>(this IEnumerable<T> enumerable)
        {
            return !enumerable.IsEmpty();
        }

    }
}
