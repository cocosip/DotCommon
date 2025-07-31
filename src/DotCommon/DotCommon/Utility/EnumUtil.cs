using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace DotCommon.Utility
{
    /// <summary>
    /// Provides utility methods for working with enumerations.
    /// </summary>
    public static class EnumUtil
    {
        /// <summary>
        /// Converts an enumeration value to its string representation.
        /// </summary>
        /// <param name="e">The enumeration value.</param>
        /// <returns>The string name of the enumeration value.</returns>
        public static string GetName(Enum e)
        {
            return Enum.GetName(e.GetType(), e);
        }

        /// <summary>
        /// Parses a string value to an enumeration type.
        /// </summary>
        /// <typeparam name="T">The target enumeration type.</typeparam>
        /// <param name="value">The string value to parse.</param>
        /// <returns>The parsed enumeration value.</returns>
        public static T ParseToEnum<T>(string value) where T : struct, IConvertible
        {
            return (T)Enum.Parse(typeof(T), value);
        }

        /// <summary>
        /// Gets the total number of values defined in an enumeration.
        /// </summary>
        /// <typeparam name="T">The enumeration type.</typeparam>
        /// <returns>The number of values in the enumeration.</returns>
        public static int GetEnumLength<T>() where T : struct, IConvertible
        {
            return Enum.GetNames(typeof(T)).Length;
        }

        /// <summary>
        /// Gets an array of the values of the constants in a specified enumeration.
        /// </summary>
        /// <typeparam name="T">The enumeration type.</typeparam>
        /// <returns>An array that contains the values of the constants in <typeparamref name="T"/>.</returns>
        public static Array GetValues<T>() where T : struct, IConvertible
        {
            return Enum.GetValues(typeof(T));
        }

        /// <summary>
        /// Determines whether a specified value exists in the specified enumeration.
        /// </summary>
        /// <typeparam name="T">The enumeration type.</typeparam>
        /// <param name="value">The value to check.</param>
        /// <returns><c>true</c> if the value is defined in the enumeration; otherwise, <c>false</c>.</returns>
        public static bool IsDefined<T>(object value) where T : struct, IConvertible
        {
            return Enum.IsDefined(typeof(T), value);
        }

        /// <summary>
        /// Gets a sorted list of descriptions for all values in an enumeration.
        /// If a <see cref="DescriptionAttribute"/> is present, its value is used; otherwise, the enumeration value's string representation is used.
        /// </summary>
        /// <typeparam name="T">The enumeration type.</typeparam>
        /// <param name="useInt32Key">If set to <c>true</c>, the integer value of the enum is used as the key; otherwise, the string name is used.</param>
        /// <returns>A <see cref="SortedList{TKey, TValue}"/> where keys are either integer values or string names, and values are descriptions.</returns>
        public static SortedList<string, string> GetEnumDescriptions<T>(bool useInt32Key = true) where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }

            var descriptions = new SortedList<string, string>();
            foreach (T item in Enum.GetValues(typeof(T)))
            {
                string key = useInt32Key ? Convert.ToInt32(item).ToString() : item.ToString();
                string value = item.ToString();

                FieldInfo field = typeof(T).GetField(item.ToString());
                if (field != null)
                {
                    string? description = field.GetCustomAttribute<DescriptionAttribute>(false)?.Description;
                    if (description != null)
                    {
                        value = description;
                    }
                }
                descriptions.Add(key, value);
            }
            return descriptions;
        }

        /// <summary>
        /// Gets the description of a specific enumeration value.
        /// If a <see cref="DescriptionAttribute"/> is present, its value is used; otherwise, the enumeration value's string representation is used.
        /// </summary>
        /// <param name="e">The enumeration value.</param>
        /// <returns>The description of the enumeration value.</returns>
        public static string GetEnumDescription(Enum e)
        {
            string value = e.ToString();
            FieldInfo field = e.GetType().GetField(value);
            if (field != null)
            {
                string? description = field.GetCustomAttribute<DescriptionAttribute>(false)?.Description;
                if (description != null)
                {
                    value = description;
                }
            }
            return value;
        }
    }
}
