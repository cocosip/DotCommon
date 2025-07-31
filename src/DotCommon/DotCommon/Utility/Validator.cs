using System;
using System.Globalization;
using System.Net;
using System.Text.RegularExpressions;

namespace DotCommon.Utility
{
    /// <summary>
    /// Provides a collection of static methods for data validation.
    /// This class includes methods for validating common data formats like email addresses, URLs, IP addresses,
    /// and various numeric types, as well as for performing custom regular expression matching.
    /// </summary>
    public static class Validator
    {
        private static readonly TimeSpan DefaultRegexTimeout = TimeSpan.FromMilliseconds(100);

        // Regex pattern for Chinese mobile numbers.
        private const string MobileNumberPattern = @"^1[3-9]\d{9}$";
        // Basic regex pattern for email validation.
        private const string EmailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        // Regex pattern for validating if a string contains only Chinese characters.
        private const string ChinesePattern = @"^[\u4e00-\u9fa5]+$";

        /// <summary>
        /// Determines whether the input string matches the specified regular expression pattern.
        /// </summary>
        /// <param name="source">The string to validate.</param>
        /// <param name="pattern">The regular expression pattern to use for matching.</param>
        /// <returns><c>true</c> if the input string matches the pattern; otherwise, <c>false</c>.</returns>
        /// <remarks>
        /// This method performs a case-insensitive match by default.
        /// It returns <c>false</c> if the source string is null, empty, or consists only of white-space characters.
        /// </remarks>
        public static bool IsMatch(string source, string pattern)
        {
            return IsMatch(source, pattern, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// Determines whether the input string matches the specified regular expression pattern with the given options.
        /// </summary>
        /// <param name="source">The string to validate.</param>
        /// <param name="pattern">The regular expression pattern to use for matching.</param>
        /// <param name="options">A bitwise combination of the enumeration values that provide options for matching.</param>
        /// <returns><c>true</c> if the input string matches the pattern; otherwise, <c>false</c>.</returns>
        /// <remarks>
        /// This method incorporates a default timeout to prevent Regular Expression Denial of Service (ReDoS) attacks.
        /// It returns <c>false</c> if the source string is null or a timeout occurs.
        /// </remarks>
        public static bool IsMatch(string source, string pattern, RegexOptions options)
        {
            if (string.IsNullOrWhiteSpace(source))
            {
                return false;
            }
            try
            {
                return Regex.IsMatch(source, pattern, options, DefaultRegexTimeout);
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        /// <summary>
        /// Validates if the string is a Chinese mobile phone number.
        /// </summary>
        /// <param name="source">The string to validate.</param>
        /// <returns><c>true</c> if the string is a valid mobile number; otherwise, <c>false</c>.</returns>
        public static bool IsMobileNumber(string source)
        {
            return IsMatch(source, MobileNumberPattern);
        }

        /// <summary>
        /// Validates if the string is a valid email address.
        /// </summary>
        /// <param name="source">The string to validate.</param>
        /// <returns><c>true</c> if the string is a valid email address; otherwise, <c>false</c>.</returns>
        /// <remarks>
        /// This method uses a basic regular expression that covers most common email formats but may not be fully compliant with RFC 5322.
        /// </remarks>
        public static bool IsEmailAddress(string source)
        {
            return IsMatch(source, EmailPattern, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// Validates if the string is a well-formed URL (http or https).
        /// </summary>
        /// <param name="source">The string to validate.</param>
        /// <returns><c>true</c> if the string is a valid URL; otherwise, <c>false</c>.</returns>
        /// <remarks>
        /// This method uses <see cref="Uri.TryCreate(string, UriKind, out Uri)"/> for validation, which is more robust than a regular expression.
        /// It only accepts absolute URIs with http or https schemes.
        /// </remarks>
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
        /// <returns><c>true</c> if the string consists exclusively of Chinese characters; otherwise, <c>false</c>.</returns>
        public static bool IsChinese(string source)
        {
            return IsMatch(source, ChinesePattern);
        }

        /// <summary>
        /// Validates if the string is a valid IPv4 address.
        /// </summary>
        /// <param name="source">The string to validate.</param>
        /// <returns><c>true</c> if the string is a valid IPv4 address; otherwise, <c>false</c>.</returns>
        /// <remarks>
        /// This method uses <see cref="System.Net.IPAddress.TryParse(string, out System.Net.IPAddress)"/> for robust validation.
        /// </remarks>
        public static bool IsIp(string source)
        {
            if (IPAddress.TryParse(source, out var address))
            {
                return address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork;
            }
            return false;
        }

        /// <summary>
        /// Validates if the string is a positive integer (greater than 0).
        /// </summary>
        /// <param name="source">The string to validate.</param>
        /// <returns><c>true</c> if the string is a positive integer; otherwise, <c>false</c>.</returns>
        public static bool IsPositiveInteger(string source)
        {
            return int.TryParse(source, out var number) && number > 0;
        }

        /// <summary>
        /// Validates if the string can be converted to a 32-bit signed integer.
        /// </summary>
        /// <param name="source">The string to validate.</param>
        /// <returns><c>true</c> if the string represents a valid <see cref="int"/>; otherwise, <c>false</c>.</returns>
        public static bool IsInt32(string source)
        {
            return int.TryParse(source, out _);
        }

        /// <summary>
        /// Validates if the string can be converted to a double-precision floating-point number.
        /// </summary>
        /// <param name="source">The string to validate.</param>
        /// <returns><c>true</c> if the string represents a valid <see cref="double"/>; otherwise, <c>false</c>.</returns>
        public static bool IsDouble(string source)
        {
            return double.TryParse(source, out _);
        }

        /// <summary>
        /// Validates if the string can be converted to a double and is within a specified range.
        /// </summary>
        /// <param name="source">The string to validate.</param>
        /// <param name="minValue">The minimum allowed value (inclusive).</param>
        /// <param name="maxValue">The maximum allowed value (inclusive).</param>
        /// <returns><c>true</c> if the string is a valid double within the specified range; otherwise, <c>false</c>.</returns>
        public static bool IsDouble(string source, double minValue, double maxValue)
        {
            if (!double.TryParse(source, out var value))
            {
                return false;
            }
            return value >= minValue && value <= maxValue;
        }

        /// <summary>
        /// Validates if the string can be converted to a decimal number.
        /// </summary>
        /// <param name="source">The string to validate.</param>
        /// <returns><c>true</c> if the string represents a valid <see cref="decimal"/>; otherwise, <c>false</c>.</returns>
        public static bool IsDecimal(string source)
        {
            return decimal.TryParse(source, out _);
        }

        /// <summary>
        /// Validates if the string can be converted to a decimal and is within a specified range.
        /// </summary>
        /// <param name="source">The string to validate.</param>
        /// <param name="minValue">The minimum allowed value (inclusive).</param>
        /// <param name="maxValue">The maximum allowed value (inclusive).</param>
        /// <returns><c>true</c> if the string is a valid decimal within the specified range; otherwise, <c>false</c>.</returns>
        public static bool IsDecimal(string source, decimal minValue, decimal maxValue)
        {
            if (!decimal.TryParse(source, out var value))
            {
                return false;
            }
            return value >= minValue && value <= maxValue;
        }

        /// <summary>
        /// Validates if the string is a valid <see cref="DateTime"/>.
        /// </summary>
        /// <param name="source">The string to validate.</param>
        /// <returns><c>true</c> if the string is a valid <see cref="DateTime"/>; otherwise, <c>false</c>.</returns>
        /// <remarks>
        /// This method uses the invariant culture for parsing to ensure consistency across different system settings.
        /// </remarks>
        public static bool IsDateTime(string source)
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
        /// <returns><c>true</c> if the string is a valid version; otherwise, <c>false</c>.</returns>
        public static bool IsVersion(string source)
        {
            return Version.TryParse(source, out _);
        }

        /// <summary>
        /// Compares two version strings to determine if the new version is higher than the old version.
        /// </summary>
        /// <param name="oldVersion">The old version string (e.g., "1.0.0").</param>
        /// <param name="newVersion">The new version string (e.g., "1.0.1").</param>
        /// <returns><c>true</c> if the new version is higher than the old version; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentException">Thrown if either version string is not in a valid <see cref="Version"/> format.</exception>
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
