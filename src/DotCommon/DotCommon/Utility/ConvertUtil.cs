using System;

namespace DotCommon.Utility
{
    /// <summary>
    /// Conversion utility class.
    /// </summary>
    public static class ConvertUtil
    {
        /// <summary>
        /// Converts to Int32 type.
        /// </summary>
        /// <param name="source">The source object.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The converted value.</returns>
        public static int ToInt32(object source, int defaultValue)
        {
            if (source != null)
            {
                if (int.TryParse(source.ToString(), out int value))
                {
                    return value;
                }
            }
            return defaultValue;
        }

        /// <summary>
        /// Converts to Int64 type.
        /// </summary>
        /// <param name="source">The source object.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The converted value.</returns>
        public static long ToInt64(object source, long defaultValue)
        {
            if (source != null)
            {
                if (long.TryParse(source.ToString(), out long value))
                {
                    return value;
                }
            }
            return defaultValue;
        }

        /// <summary>
        /// Converts to Double type.
        /// </summary>
        /// <param name="source">The source object.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The converted value.</returns>
        public static double ToDouble(object source, double defaultValue)
        {
            if (source != null)
            {
                if (double.TryParse(source.ToString(), out double value))
                {
                    return value;
                }
            }
            return defaultValue;
        }

        /// <summary>
        /// Converts to double type and keeps the specified number of digits.
        /// </summary>
        /// <param name="source">The source object.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="digit">The number of digits to keep.</param>
        /// <returns>The converted value.</returns>
        public static double ToDouble(object source, double defaultValue, int digit)
        {
            if (source != null)
            {
                if (double.TryParse(source.ToString(), out double value))
                {
                    return Math.Round(value, digit);
                }
            }
            return Math.Round(defaultValue, digit);
        }

        /// <summary>
        /// Converts to DateTime.
        /// </summary>
        /// <param name="source">The source object.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The converted value.</returns>
        public static DateTime ToDateTime(object source, DateTime defaultValue)
        {
            if (source != null)
            {
                if (DateTime.TryParse(source.ToString(), out DateTime dateTime))
                {
                    return dateTime;
                }
            }
            return defaultValue;
        }

        /// <summary>
        /// Converts to Bool type.
        /// </summary>
        /// <param name="source">The source object.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The converted value.</returns>
        public static bool ToBool(object source, bool defaultValue)
        {
            if (source != null)
            {
                if (bool.TryParse(source.ToString(), out bool value))
                {
                    return value;
                }
            }
            return defaultValue;
        }

        /// <summary>
        /// Converts a string to the corresponding Guid.
        /// </summary>
        /// <param name="source">The string to convert.</param>
        /// <returns>The converted Guid.</returns>
        public static Guid ToGuid(string source)
        {
            return new Guid(source);
        }

    }
}