using System;

namespace DotCommon.Utility
{
    /// <summary>
    /// Represents a range of values with a minimum and maximum value.
    /// </summary>
    /// <typeparam name="T">The type of values in the range, which must implement IComparable.</typeparam>
    public class ValueRange<T> where T : IComparable<T>
    {
        /// <summary>
        /// Gets or sets the minimum value of the range.
        /// </summary>
        public T MinValue { get; set; }

        /// <summary>
        /// Gets or sets the maximum value of the range.
        /// </summary>
        public T MaxValue { get; set; }

        /// <summary>
        /// Initializes a new instance of the ValueRange class with the specified minimum and maximum values.
        /// </summary>
        /// <param name="min">The minimum value of the range.</param>
        /// <param name="max">The maximum value of the range.</param>
        public ValueRange(T min, T max)
        {
            MinValue = min;
            MaxValue = max;
        }

        /// <summary>
        /// Determines whether the specified value is within the range (inclusive).
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns>true if the value is within the range; otherwise, false.</returns>
        public bool Contains(T value)
        {
            return MinValue.CompareTo(value) <= 0 && MaxValue.CompareTo(value) >= 0;
        }

        /// <summary>
        /// Expands the range to include the specified value if it's outside the current range.
        /// </summary>
        /// <param name="value">The value to include in the range.</param>
        public void Join(T value)
        {
            if (MinValue.CompareTo(value) > 0)
            {
                MinValue = value;
            }
            if (MaxValue.CompareTo(value) < 0)
            {
                MaxValue = value;
            }
        }

    }

    /// <summary>
    /// Represents a date range, which is a specialized ValueRange for DateTime values.
    /// </summary>
    public class DateRange : ValueRange<DateTime>
    {
        /// <summary>
        /// Initializes a new instance of the DateRange class with default minimum and maximum values.
        /// </summary>
        public DateRange() : base(DateTime.MinValue, DateTime.MaxValue)
        {

        }

        /// <summary>
        /// Initializes a new instance of the DateRange class with the specified minimum and maximum dates.
        /// </summary>
        /// <param name="min">The minimum date of the range.</param>
        /// <param name="max">The maximum date of the range.</param>
        public DateRange(DateTime min, DateTime max)
           : base(min, max)
        {
        }

        /// <summary>
        /// Returns a string representation of the date range using the default format (yyyyMMddHHmmss).
        /// </summary>
        /// <returns>A string representation of the date range.</returns>
        public override string ToString()
        {
            return ToString("yyyyMMddHHmmss");
        }

        /// <summary>
        /// Returns a string representation of the date range using the specified format.
        /// </summary>
        /// <param name="format">The format string to use for date formatting.</param>
        /// <returns>A string representation of the date range.</returns>
        public string ToString(string format)
        {
            var value = (MinValue == DateTime.MinValue ? string.Empty : MinValue.ToString(format)) + "-"
                        + (MaxValue == DateTime.MaxValue ? string.Empty : MaxValue.ToString(format));
            if (value == "-")
            {
                return string.Empty;
            }
            return value;
        }
    }
}