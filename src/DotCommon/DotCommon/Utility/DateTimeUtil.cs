using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

namespace DotCommon.Utility
{
    /// <summary>
    /// Provides utility methods for common DateTime operations, including conversions, formatting, and date calculations.
    /// </summary>
    public static class DateTimeUtil
    {
        private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// Truncates a <see cref="DateTime"/> object to the millisecond level, discarding any higher precision (e.g., microseconds, nanoseconds).
        /// </summary>
        /// <param name="datetime">The DateTime object to truncate.</param>
        /// <returns>A new DateTime object truncated to milliseconds.</returns>
        public static DateTime TruncateToMilliseconds(this DateTime datetime)
        {
            return new DateTime(datetime.Year, datetime.Month, datetime.Day, datetime.Hour, datetime.Minute, datetime.Second, datetime.Millisecond);
        }

        /// <summary>
        /// Truncates a <see cref="DateTime"/> object to the second level, discarding any higher precision (e.g., milliseconds, microseconds, nanoseconds).
        /// </summary>
        /// <param name="datetime">The DateTime object to truncate.</param>
        /// <returns>A new DateTime object truncated to seconds.</returns>
        public static DateTime TruncateToSeconds(this DateTime datetime)
        {
            return new DateTime(datetime.Year, datetime.Month, datetime.Day, datetime.Hour, datetime.Minute, datetime.Second);
        }

        /// <summary>
        /// Converts a <see cref="DateTime"/> object to a Unix timestamp (seconds since 1970-01-01 00:00:00 UTC).
        /// </summary>
        /// <param name="datetime">The DateTime object to convert.</param>
        /// <returns>An <see cref="int"/> representing the Unix timestamp in seconds.</returns>
        public static int ToInt32(DateTime datetime)
        {
            return (int)(datetime.ToUniversalTime() - UnixEpoch).TotalSeconds;
        }

        /// <summary>
        /// Converts a <see cref="DateTime"/> object to a Unix timestamp (milliseconds since 1970-01-01 00:00:00 UTC).
        /// </summary>
        /// <param name="datetime">The DateTime object to convert.</param>
        /// <returns>A <see cref="long"/> representing the Unix timestamp in milliseconds.</returns>
        public static long ToInt64(DateTime datetime)
        {
            return (long)(datetime.ToUniversalTime() - UnixEpoch).TotalMilliseconds;
        }

        /// <summary>
        /// Converts a string representation of a date and time to an <see cref="int"/> Unix timestamp (seconds).
        /// If the string is not a valid date/time, the <paramref name="defaultValue"/> is returned.
        /// </summary>
        /// <param name="datetime">The string containing the date and time.</param>
        /// <param name="defaultValue">The default value to return if conversion fails (default is 0).</param>
        /// <returns>An <see cref="int"/> Unix timestamp, or the default value if conversion fails.</returns>
        public static int ToInt32(string datetime, int defaultValue = 0)
        {
            if (DateTime.TryParse(datetime, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDateTime))
            {
                return ToInt32(parsedDateTime);
            }
            return defaultValue;
        }

        /// <summary>
        /// Converts an <see cref="int"/> Unix timestamp (seconds since 1970-01-01 00:00:00 UTC) to a <see cref="DateTime"/> object.
        /// </summary>
        /// <param name="seconds">The Unix timestamp in seconds.</param>
        /// <returns>A <see cref="DateTime"/> object representing the given Unix timestamp.</returns>
        public static DateTime ToDateTime(int seconds)
        {
            return UnixEpoch.AddSeconds(seconds).ToLocalTime();
        }

        /// <summary>
        /// Converts a <see cref="long"/> Unix timestamp (milliseconds since 1970-01-01 00:00:00 UTC) to a <see cref="DateTime"/> object.
        /// </summary>
        /// <param name="millSeconds">The Unix timestamp in milliseconds.</param>
        /// <returns>A <see cref="DateTime"/> object representing the given Unix timestamp.</returns>
        public static DateTime ToDateTime(long millSeconds)
        {
            return UnixEpoch.AddMilliseconds(millSeconds).ToLocalTime();
        }

        /// <summary>
        /// Gets a formatted string representation of a <see cref="DateTime"/> object, padded to include year, month, and day (e.g., "YYYYMMDD").
        /// </summary>
        /// <param name="time">The DateTime object to format.</param>
        /// <returns>A string in "YYYYMMDD" format.</returns>
        public static string GetPadDay(DateTime time)
        {
            return time.ToString("yyyyMMdd");
        }

        /// <summary>
        /// Gets a formatted string representation of a <see cref="DateTime"/> object, padded to include year, month, day, hour, minute, and second (e.g., "YYYYMMDDhhmmss").
        /// </summary>
        /// <param name="time">The DateTime object to format.</param>
        /// <returns>A string in "YYYYMMDDhhmmss" format.</returns>
        public static string GetPadSecond(DateTime time)
        {
            return time.ToString("yyyyMMddHHmmss");
        }

        /// <summary>
        /// Gets a formatted string representation of a <see cref="DateTime"/> object, padded to include year (last two digits), month, day, hour, minute, and second (e.g., "YYMMDDhhmmss").
        /// </summary>
        /// <param name="time">The DateTime object to format.</param>
        /// <returns>A string in "YYMMDDhhmmss" format.</returns>
        public static string GetPadSecondWithoutPrefix(DateTime time)
        {
            return time.ToString("yyMMddHHmmss");
        }

        /// <summary>
        /// Gets a formatted string representation of a <see cref="DateTime"/> object, padded to include year, month, day, hour, minute, second, and millisecond (e.g., "YYYYMMDDhhmmssfff").
        /// </summary>
        /// <param name="time">The DateTime object to format.</param>
        /// <returns>A string in "YYYYMMDDhhmmssfff" format.</returns>
        public static string GetPadMillSecond(DateTime time)
        {
            return time.ToString("yyyyMMddHHmmssfff");
        }

        /// <summary>
        /// Gets a formatted string representation of a <see cref="DateTime"/> object, padded to include year (last two digits), month, day, hour, minute, second, and millisecond (e.g., "YYMMDDhhmmssfff").
        /// </summary>
        /// <param name="time">The DateTime object to format.</param>
        /// <returns>A string in "YYMMDDhhmmssfff" format.</returns>
        public static string GetPadMillSecondWithoutPrefix(DateTime time)
        {
            return time.ToString("yyMMddHHmmssfff");
        }

        /// <summary>
        /// Calculates the days of the week that occur between two <see cref="DateTime"/> objects.
        /// </summary>
        /// <param name="begin">The start date.</param>
        /// <param name="end">The end date.</param>
        /// <returns>A comma-separated string of integer representations of <see cref="DayOfWeek"/> (0 for Sunday, 6 for Saturday) that fall within the range. Returns an empty string if the begin date is after the end date.</returns>
        public static string GetWeekCross(DateTime begin, DateTime end)
        {
            if (begin.Date > end.Date)
            {
                return string.Empty;
            }

            var daysInPeriod = new List<int>();
            for (DateTime date = begin.Date; date <= end.Date; date = date.AddDays(1))
            {
                daysInPeriod.Add((int)date.DayOfWeek);
            }

            // If the period covers a full week or more, all days of the week are present.
            if (daysInPeriod.Count >= 7)
            {
                return string.Join(",", Enumerable.Range(0, 7)); // Returns "0,1,2,3,4,5,6"
            }

            return string.Join(",", daysInPeriod.Distinct().OrderBy(d => d));
        }

        /// <summary>
        /// Gets the Chinese name of the day of the week for a given <see cref="DateTime"/> object.
        /// </summary>
        /// <param name="dateTime">The DateTime object.</param>
        /// <returns>The Chinese name of the day of the week (e.g., "星期日", "星期一").</returns>
        public static string GetChineseWeekOfDay(DateTime dateTime)
        {
            string[] weekDays = { "星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六" };
            return weekDays[(int)dateTime.DayOfWeek];
        }

        /// <summary>
        /// Gets a list of Chinese week day names.
        /// </summary>
        /// <returns>A <see cref="List{T}"/> containing the Chinese names of the days of the week.</returns>
        public static List<string> GetWeekDays()
        {
            return new List<string> { "星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六" };
        }

        /// <summary>
        /// Gets the first day of the month for a given <see cref="DateTime"/> object.
        /// </summary>
        /// <param name="time">The DateTime object.</param>
        /// <returns>A DateTime object representing the first day of the month.</returns>
        public static DateTime GetFirstDayOfMonth(DateTime time)
        {
            return new DateTime(time.Year, time.Month, 1);
        }

        /// <summary>
        /// Gets the last day of the month for a given <see cref="DateTime"/> object.
        /// </summary>
        /// <param name="time">The DateTime object.</param>
        /// <returns>A DateTime object representing the last day of the month.</returns>
        public static DateTime GetLastDayOfMonth(DateTime time)
        {
            return new DateTime(time.Year, time.Month, DateTime.DaysInMonth(time.Year, time.Month));
        }

        /// <summary>
        /// Gets the first day of the year for a given <see cref="DateTime"/> object.
        /// </summary>
        /// <param name="time">The DateTime object.</param>
        /// <returns>A DateTime object representing the first day of the year.</returns>
        public static DateTime GetFirstDayOfYear(DateTime time)
        {
            return new DateTime(time.Year, 1, 1);
        }

        /// <summary>
        /// Gets the last day of the year for a given <see cref="DateTime"/> object.
        /// </summary>
        /// <param name="time">The DateTime object.</param>
        /// <returns>A DateTime object representing the last day of the year.</returns>
        public static DateTime GetLastDayOfYear(DateTime time)
        {
            return new DateTime(time.Year, 12, 31);
        }

        /// <summary>
        /// Gets the last second of the day (23:59:59) for a given <see cref="DateTime"/> object.
        /// </summary>
        /// <param name="time">The DateTime object.</param>
        /// <returns>A DateTime object representing the last second of the day.</returns>
        public static DateTime GetLastSecondOfDay(DateTime time)
        {
            return time.Date.AddDays(1).AddSeconds(-1);
        }

        /// <summary>
        /// Determines if a given <see cref="DateTime"/> object is the first day of its month.
        /// </summary>
        /// <param name="time">The DateTime object to check.</param>
        /// <returns><c>true</c> if it's the first day of the month; otherwise, <c>false</c>.</returns>
        public static bool IsFirstDayOfMonth(DateTime time)
        {
            return time.Day == 1;
        }

        /// <summary>
        /// Determines if a given <see cref="DateTime"/> object is the last day of its month.
        /// </summary>
        /// <param name="time">The DateTime object to check.</param>
        /// <returns><c>true</c> if it's the last day of the month; otherwise, <c>false</c>.</returns>
        public static bool IsLastDayOfMonth(DateTime time)
        {
            return time.Day == DateTime.DaysInMonth(time.Year, time.Month);
        }

        /// <summary>
        /// Determines if a given <see cref="DateTime"/> object is the first day of its year.
        /// </summary>
        /// <param name="time">The DateTime object to check.</param>
        /// <returns><c>true</c> if it's the first day of the year; otherwise, <c>false</c>.</returns>
        public static bool IsFirstDayOfYear(DateTime time)
        {
            return time.Month == 1 && time.Day == 1;
        }

        /// <summary>
        /// Determines if a given <see cref="DateTime"/> object is the last day of its year.
        /// </summary>
        /// <param name="time">The DateTime object to check.</param>
        /// <returns><c>true</c> if it's the last day of the year; otherwise, <c>false</c>.</returns>
        public static bool IsLastDayOfYear(DateTime time)
        {
            return time.Month == 12 && time.Day == 31;
        }

        /// <summary>
        /// Replaces the day component of a <see cref="DateTime"/> object with a new day value from a string.
        /// </summary>
        /// <param name="day">The new day value as a string.</param>
        /// <param name="datetime">The original DateTime object.</param>
        /// <returns>A new DateTime object with the updated day. Throws <see cref="FormatException"/> if the day string is invalid.</returns>
        public static DateTime ReplaceDay(string day, DateTime datetime)
        {
            return DateTime.ParseExact($"{datetime.Year}-{datetime.Month}-{day}", "yyyy-M-d", CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Replaces the time component of a <see cref="DateTime"/> object with a new time value from a string.
        /// </summary>
        /// <param name="time">The new time value as a string (e.g., "HH:mm:ss").</param>
        /// <param name="datetime">The original DateTime object.</param>
        /// <returns>A new DateTime object with the updated time. Throws <see cref="FormatException"/> if the time string is invalid.</returns>
        public static DateTime ReplaceTime(string time, DateTime datetime)
        {
            return DateTime.ParseExact($"{datetime.Date:yyyy-MM-dd} {time}", "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
        }
    }
}