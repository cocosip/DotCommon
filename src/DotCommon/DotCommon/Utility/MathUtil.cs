using System;
using System.Collections.Generic;
using System.Linq;

namespace DotCommon.Utility
{
    /// <summary>
    /// Mathematical utility class providing common mathematical operations.
    /// </summary>
    public static class MathUtil
    {

        #region Get two values with minimum difference in collection

        /// <summary>
        /// Gets two values from the collection with the minimum difference between them.
        /// </summary>
        /// <typeparam name="T">The type of elements in the collection.</typeparam>
        /// <param name="source">The source collection.</param>
        /// <param name="selector">A function to extract a decimal value from each element.</param>
        /// <returns>A tuple containing the two elements with minimum difference, or default if collection has less than 2 elements.</returns>
        public static (T, T) GetMinMinus<T>(List<T> source, Func<T, decimal> selector)
        {
            if (source.Count < 2)
            {
                return default;
            }
            if (source.Count == 2)
            {
                return (source[0], source[1]);
            }
            source = source.OrderBy(selector).ToList();
            var difference = Math.Abs(selector(source[0]) - selector(source[1]));
            int k = 0; // Index of the result elements
            for (int i = 1; i < source.Count - 1; i++)
            {
                var currentDifference = Math.Abs(selector(source[i]) - selector(source[i + 1]));
                if (difference > currentDifference)
                {
                    difference = currentDifference;
                    k = i;
                }
            }
            return (source[k], source[k + 1]);
        }

        /// <summary>
        /// Gets two values from the collection with the minimum difference between them.
        /// </summary>
        /// <typeparam name="T">The type of elements in the collection.</typeparam>
        /// <param name="source">The source collection.</param>
        /// <param name="selector">A function to extract a double value from each element.</param>
        /// <returns>A tuple containing the two elements with minimum difference, or default if collection has less than 2 elements.</returns>
        public static (T, T) GetMinMinus<T>(List<T> source, Func<T, double> selector)
        {
            if (source.Count < 2)
            {
                return default;
            }
            if (source.Count == 2)
            {
                return (source[0], source[1]);
            }
            source = source.OrderBy(selector).ToList();
            var difference = Math.Abs(selector(source[0]) - selector(source[1]));
            int k = 0; // Index of the result elements
            for (int i = 1; i < source.Count - 1; i++)
            {
                var currentDifference = Math.Abs(selector(source[i]) - selector(source[i + 1]));
                if (difference > currentDifference)
                {
                    difference = currentDifference;
                    k = i;
                }
            }
            return (source[k], source[k + 1]);
        }

        /// <summary>
        /// Gets two values from the collection with the minimum difference between them.
        /// </summary>
        /// <typeparam name="T">The type of elements in the collection.</typeparam>
        /// <param name="source">The source collection.</param>
        /// <param name="selector">A function to extract an integer value from each element.</param>
        /// <returns>A tuple containing the two elements with minimum difference, or default if collection has less than 2 elements.</returns>
        public static (T, T) GetMinMinus<T>(List<T> source, Func<T, int> selector)
        {
            if (source.Count < 2)
            {
                return default;
            }
            if (source.Count == 2)
            {
                return (source[0], source[1]);
            }
            source = source.OrderBy(selector).ToList();
            var difference = Math.Abs(selector(source[0]) - selector(source[1]));
            int k = 0; // Index of the result elements
            for (int i = 1; i < source.Count - 1; i++)
            {
                var currentDifference = Math.Abs(selector(source[i]) - selector(source[i + 1]));
                if (difference > currentDifference)
                {
                    difference = currentDifference;
                    k = i;
                }
            }
            return (source[k], source[k + 1]);
        }

        /// <summary>
        /// Gets two values from the collection with the minimum difference between them.
        /// </summary>
        /// <typeparam name="T">The type of elements in the collection.</typeparam>
        /// <param name="source">The source collection.</param>
        /// <param name="selector">A function to extract a float value from each element.</param>
        /// <returns>A tuple containing the two elements with minimum difference, or default if collection has less than 2 elements.</returns>
        public static (T, T) GetMinMinus<T>(List<T> source, Func<T, float> selector)
        {
            if (source.Count < 2)
            {
                return default;
            }
            if (source.Count == 2)
            {
                return (source[0], source[1]);
            }
            source = source.OrderBy(selector).ToList();
            var difference = Math.Abs(selector(source[0]) - selector(source[1]));
            int k = 0; // Index of the result elements
            for (int i = 1; i < source.Count - 1; i++)
            {
                var currentDifference = Math.Abs(selector(source[i]) - selector(source[i + 1]));
                if (difference > currentDifference)
                {
                    difference = currentDifference;
                    k = i;
                }
            }
            return (source[k], source[k + 1]);
        }

        /// <summary>
        /// Gets two values from the collection with the minimum difference between them.
        /// </summary>
        /// <typeparam name="T">The type of elements in the collection.</typeparam>
        /// <param name="source">The source collection.</param>
        /// <param name="selector">A function to extract a long value from each element.</param>
        /// <returns>A tuple containing the two elements with minimum difference, or default if collection has less than 2 elements.</returns>
        public static (T, T) GetMinMinus<T>(List<T> source, Func<T, long> selector)
        {
            if (source.Count < 2)
            {
                return default;
            }
            if (source.Count == 2)
            {
                return (source[0], source[1]);
            }
            source = source.OrderBy(selector).ToList();
            var difference = Math.Abs(selector(source[0]) - selector(source[1]));
            int k = 0; // Index of the result elements
            for (int i = 1; i < source.Count - 1; i++)
            {
                var currentDifference = Math.Abs(selector(source[i]) - selector(source[i + 1]));
                if (difference > currentDifference)
                {
                    difference = currentDifference;
                    k = i;
                }
            }
            return (source[k], source[k + 1]);
        }


        #endregion

        /// <summary>
        /// Computes the Cartesian product of multiple lists.
        /// </summary>
        /// <typeparam name="T">The type of elements in the lists.</typeparam>
        /// <param name="array">The arrays to compute the Cartesian product for.</param>
        /// <returns>A list of lists representing the Cartesian product.</returns>
        public static List<List<T>> Descartes<T>(params List<T>[] array)
        {
            int total = 1;
            foreach (var item in array)
            {
                total *= item.Count;
            }
            var result = new List<T>[total];
            int itemLoopNum = 1;
            int loopPerItem = 1;
            int now = 1;
            foreach (var arrayItem in array)
            {
                now *= arrayItem.Count;

                int index = 0;
                int currentSize = arrayItem.Count;
                itemLoopNum = total / now;
                loopPerItem = total / (itemLoopNum * currentSize);
                int myIndex = 0;

                foreach (var item in arrayItem)
                {
                    for (int i = 0; i < loopPerItem; i++)
                    {
                        if (myIndex == arrayItem.Count)
                        {
                            myIndex = 0;
                        }
                        for (int j = 0; j < itemLoopNum; j++)
                        {
                            if (result[index] == null)
                            {
                                result[index] = new List<T>();
                            }
                            result[index].Add(arrayItem[myIndex]);
                            index++;
                        }
                        myIndex++;
                    }

                }
            }

            return result.ToList();
        }
    }
}