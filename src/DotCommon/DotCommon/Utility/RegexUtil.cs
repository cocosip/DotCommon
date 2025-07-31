using System;
using System.Globalization;
using System.Net;
using System.Text.RegularExpressions;

namespace DotCommon.Utility
{
    /// <summary>
    /// A utility class for regular expression and string validation operations.
    /// </summary>
    public static class RegexUtil
    {
        // Using a timeout for all regex operations to prevent ReDoS attacks.
        private static readonly TimeSpan RegexTimeout = TimeSpan.FromMilliseconds(100);

        /// <summary>
        /// Checks if the input string matches the given regular expression pattern.
        /// </summary>
        /// <param name="source">The string to validate.</param>
        /// <param name="pattern">The regular expression pattern to use.</param>
        /// <returns>True if the input string matches the pattern; otherwise, false.</returns>
        public static bool IsMatch(string source, string pattern)
        {
            return IsMatch(source, pattern, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// Checks if the input string matches the given regular expression pattern with the specified options.
        /// </summary>
        /// <param name="source">The string to validate.</param>
        /// <param name="pattern">The regular expression pattern to use.</param>
        /// <param name="options">The regular expression options.</param>
        /// <returns>True if the input string matches the pattern; otherwise, false.</returns>
        public static bool IsMatch(string source, string pattern, RegexOptions options)
        {
            if (string.IsNullOrWhiteSpace(source))
            {
                return false;
            }
            try
            {
                return Regex.IsMatch(source, pattern, options, RegexTimeout);
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        /// <summary>
        /// Validates if the string is a mobile phone number.
        /// This pattern is a general one for Chinese mobile numbers.
        /// </summary>
        /// <param name="source">The string to validate.</param>
        /// <returns>True if the string is a valid mobile number; otherwise, false.</returns>
        public static bool IsMobileNumber(string source)
        {
            const string pattern = @"^1[3-9]\d{9}$";
            return IsMatch(source, pattern);
        }

        /// <summary>
        /// Validates if the string is a valid email address.
        /// </summary>
        /// <param name="source">The string to validate.</param>
        /// <returns>True if the string is a valid email address; otherwise, false.</returns>
        public static bool IsEmailAddress(string source)
        {
            // This regex is a widely used pattern for email validation.
            const string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return IsMatch(source, pattern, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// Validates if the string is a valid URL (http or https).
        /// </summary>
        /// <param name="source">The string to validate.</param>
        /// <returns>True if the string is a valid URL; otherwise, false.</returns>
        public static bool IsUrl(string source)
        {
            if (string.IsNullOrWhiteSpace(source))
            {
                return false;
            }
            return Uri.TryCreate(source, UriKind.Absolute, out var uriResult)
                   && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }

        /// <summary>
        /// Validates if the string contains only Chinese characters.
        /// </summary>
        /// <param name="source">The string to validate.</param>
        /// <returns>True if the string consists only of Chinese characters; otherwise, false.</returns>
        public static bool IsChinese(string source)
        {
            const string pattern = @"^[\u4e00-\u9fa5]+$";
            return IsMatch(source, pattern);
        }

        /// <summary>
        /// Validates if the string is a valid IP address (IPv4).
        /// </summary>
        /// <param name="source">The string to validate.</param>
        /// <returns>True if the string is a valid IP address; otherwise, false.</returns>
        public static bool IsIp(string source)
        {
            if (string.IsNullOrWhiteSpace(source))
            {
                return false;
            }
            return IPAddress.TryParse(source, out _);
        }

        /// <summary>
        /// Validates if the string is a positive integer (greater than 0).
        /// </summary>
        /// <param name="source">The string to validate.</param>
        /// <returns>True if the string is a positive integer; otherwise, false.</returns>
        public static bool IsPositiveInteger(string source)
        {
            if (!IsInt32(source))
            {
                return false;
            }
            return int.Parse(source) > 0;
        }

        /// <summary>
        /// Validates if the string can be converted to an Int32.
        /// </summary>
        /// <param name="source">The string to validate.</param>
        /// <returns>True if the string represents a valid Int32; otherwise, false.</returns>
        public static bool IsInt32(string source)
        {
            if (string.IsNullOrWhiteSpace(source))
            {
                return false;
            }
            return int.TryParse(source, out _);
        }

        /// <summary>
        /// Validates if the string can be converted to a double.
        /// </summary>
        /// <param name="source">The string to validate.</param>
        /// <returns>True if the string represents a valid double; otherwise, false.</returns>
        public static bool IsDouble(string source)
        {
            if (string.IsNullOrWhiteSpace(source))
            {
                return false;
            }
            return double.TryParse(source, out _);
        }

        /// <summary>
        /// Validates if the string can be converted to a double and is within a specified range.
        /// </summary>
        /// <param name="source">The string to validate.</param>
        /// <param name="minValue">The minimum allowed value.</param>
        /// <param name="maxValue">The maximum allowed value.</param>
        /// <returns>True if the string is a valid double within the range; otherwise, false.</returns>
        public static bool IsDouble(string source, double minValue, double maxValue)
        {
            if (!IsDouble(source))
            {
                return false;
            }
            var val = double.Parse(source);
            return val >= minValue && val <= maxValue;
        }

        /// <summary>
        /// Validates if the string can be converted to a decimal.
        /// </summary>
        /// <param name="source">The string to validate.</param>
        /// <returns>True if the string represents a valid decimal; otherwise, false.</returns>
        public static bool IsDecimal(string source)
        {
            if (string.IsNullOrWhiteSpace(source))
            {
                return false;
            }
            return decimal.TryParse(source, out _);
        }

        /// <summary>
        /// Validates if the string can be converted to a decimal and is within a specified range.
        /// </summary>
        /// <param name="source">The string to validate.</param>
        /// <param name="minValue">The minimum allowed value.</param>
        /// <param name="maxValue">The maximum allowed value.</param>
        /// <returns>True if the string is a valid decimal within the range; otherwise, false.</returns>
        public static bool IsDecimal(string source, decimal minValue, decimal maxValue)
        {
            if (!IsDecimal(source))
            {
                return false;
            }
            var val = decimal.Parse(source);
            return val >= minValue && val <= maxValue;
        }

        /// <summary>
        /// Validates if the string is a valid DateTime.
        /// </summary>
        /// <param name="source">The string to validate.</param>
        /// <returns>True if the string is a valid DateTime; otherwise, false.</returns>
        public static bool IsDataTime(string source)
        {
            if (string.IsNullOrWhiteSpace(source))
            {
                return false;
            }
            return DateTime.TryParse(source, CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
        }

        /// <summary>
        /// Validates if the string is a valid version string (e.g., "1.0.0").
        /// </summary>
        /// <param name="source">The string to validate.</param>
        /// <returns>True if the string is a valid version; otherwise, false.</returns>
        public static bool IsVersion(string source)
        {
            if (string.IsNullOrWhiteSpace(source))
            {
                return false;
            }
            return Version.TryParse(source, out _);
        }

        /// <summary>
        /// Compares two version strings to determine if the new version is higher than the old version.
        /// </summary>
        /// <param name="oldVersion">The old version string.</param>
        /// <param name="newVersion">The new version string.</param>
        /// <returns>True if the new version is higher than the old version; otherwise, false.</returns>
        /// <exception cref="ArgumentException">Thrown if either version string is not a valid version format.</exception>
        public static bool IsVersionUpper(string oldVersion, string newVersion)
        {
            if (!Version.TryParse(oldVersion, out var oldVer))
            {
                throw new ArgumentException($"Invalid version format: {oldVersion}", nameof(oldVersion));
            }
            if (!Version.TryParse(newVersion, out var newVer))
            {
                throw new ArgumentException($"Invalid version format: {newVersion}", nameof(newVersion));
            }

            return newVer > oldVer;
        }
    }
}