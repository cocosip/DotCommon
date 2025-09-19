using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace DotCommon.Utility
{
    /// <summary>
    /// Utility class for generating random numbers and strings.
    /// </summary>
    public static class RandomUtil
    {
        private static readonly char[] RandomArray =
        {
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c',
            'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y',
            'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U',
            'V', 'W', 'X', 'Y', 'Z'
        };

        private static readonly Dictionary<RandomStringType, Tuple<int, int>> RandomRanges = new Dictionary
            <RandomStringType, Tuple<int, int>>()
        {
            [RandomStringType.Number] = new Tuple<int, int>(0, 9),
            [RandomStringType.LowerLetter] = new Tuple<int, int>(10, 35),
            [RandomStringType.UpperLetter] = new Tuple<int, int>(36, 61),
            [RandomStringType.Fix] = new Tuple<int, int>(0, 61),
            [RandomStringType.NumberLower] = new Tuple<int, int>(0, 35),
            [RandomStringType.NumberUpper] = new Tuple<int, int>(36, 61),
            [RandomStringType.Letter] = new Tuple<int, int>(10, 61)
        };

        /// <summary>
        /// Generates a random seed.
        /// </summary>
        /// <param name="len">The length of the seed in bytes.</param>
        /// <returns>A random integer seed.</returns>
        public static int GetRandomSeed(int len = 8)
        {
            var bytes = new byte[len];
            using (var generator = RandomNumberGenerator.Create())
            {
                generator.GetBytes(bytes);
            }
            return BitConverter.ToInt32(bytes, 0);
        }

        /// <summary>
        /// Generates a random integer within a specified range.
        /// </summary>
        /// <param name="minNum">The minimum value (inclusive).</param>
        /// <param name="maxNum">The maximum value (exclusive).</param>
        /// <returns>A random integer between minNum and maxNum.</returns>
        public static int GetRandomInt(int minNum, int maxNum)
        {
            var rd = new Random(GetRandomSeed());
            return rd.Next(minNum, maxNum);
        }

        /// <summary>
        /// Randomly shuffles an array using the Fisher-Yates shuffle algorithm.
        /// </summary>
        /// <typeparam name="T">The type of elements in the array.</typeparam>
        /// <param name="arr">The array to shuffle.</param>
        public static void GetRandomArray<T>(T[] arr)
        {
            if (arr == null || arr.Length <= 1)
                return;

            var rd = new Random(GetRandomSeed());
            // Fisher-Yates shuffle algorithm
            for (int i = arr.Length - 1; i > 0; i--)
            {
                // Generate a random index between 0 and i (inclusive)
                var randomIndex = rd.Next(0, i + 1);
                // Swap elements at positions i and randomIndex
                var temp = arr[i];
                arr[i] = arr[randomIndex];
                arr[randomIndex] = temp;
            }
        }

        /// <summary>
        /// Generates a random string of specified length and type.
        /// </summary>
        /// <param name="len">The length of the random string.</param>
        /// <param name="randomStringType">The type of characters to include in the random string.</param>
        /// <returns>A random string of the specified length and type.</returns>
        public static string GetRandomStr(int len, RandomStringType randomStringType = RandomStringType.Number)
        {
            if (len <= 0)
                return string.Empty;

            var rd = new Random(GetRandomSeed());
            var sb = new StringBuilder(len);
            var range = RandomRanges[randomStringType];
            
            for (var i = 0; i < len; i++)
            {
                // Generate a random index within the specified range
                var index = rd.Next(range.Item1, range.Item2 + 1);
                sb.Append(RandomArray[index]);
            }
            return sb.ToString();
        }
    }

    /// <summary>
    /// Enumeration of random string types.
    /// </summary>
    public enum RandomStringType
    {
        /// <summary>
        /// Numeric characters only (0-9).
        /// </summary>
        Number = 1,

        /// <summary>
        /// Lowercase letters only (a-z).
        /// </summary>
        LowerLetter = 2,

        /// <summary>
        /// Uppercase letters only (A-Z).
        /// </summary>
        UpperLetter = 4,

        /// <summary>
        /// Mixed characters (0-9, a-z, A-Z).
        /// </summary>
        Fix = 8,

        /// <summary>
        /// Numeric and lowercase letters (0-9, a-z).
        /// </summary>
        NumberLower = 16,

        /// <summary>
        /// Numeric and uppercase letters (0-9, A-Z).
        /// </summary>
        NumberUpper = 32,

        /// <summary>
        /// Letters only (a-z, A-Z).
        /// </summary>
        Letter = 64
    }
}