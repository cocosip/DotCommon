using System;
using System.Text.RegularExpressions;
using Xunit;

namespace DotCommon.Utility.Test
{
    public class ValidatorTest
    {
        [Theory]
        [InlineData("18688888888", true)]
        [InlineData("12345678901", false)]
        [InlineData("not a number", false)]
        [InlineData(null, false)]
        public void IsMobileNumber_ShouldReturnExpectedResult(string input, bool expected)
        {
            Assert.Equal(expected, Validator.IsMobileNumber(input));
        }

        [Theory]
        [InlineData("test@example.com", true)]
        [InlineData("invalid-email", false)]
        [InlineData("test@.com", false)]
        [InlineData(null, false)]
        public void IsEmailAddress_ShouldReturnExpectedResult(string input, bool expected)
        {
            Assert.Equal(expected, Validator.IsEmailAddress(input));
        }

        [Theory]
        [InlineData("http://example.com", true)]
        [InlineData("https://example.com", true)]
        [InlineData("ftp://example.com", false)]
        [InlineData("not a url", false)]
        [InlineData(null, false)]
        public void IsUrl_ShouldReturnExpectedResult(string input, bool expected)
        {
            Assert.Equal(expected, Validator.IsUrl(input));
        }

        [Theory]
        [InlineData("中文", true)]
        [InlineData("中文 and english", false)]
        [InlineData("", false)]
        [InlineData(null, false)]
        public void IsChinese_ShouldReturnExpectedResult(string input, bool expected)
        {
            Assert.Equal(expected, Validator.IsChinese(input));
        }

        [Theory]
        [InlineData("192.168.1.1", true)]
        [InlineData("256.0.0.1", false)]
        [InlineData("not an ip", false)]
        [InlineData(null, false)]
        public void IsIp_ShouldReturnExpectedResult(string input, bool expected)
        {
            Assert.Equal(expected, Validator.IsIp(input));
        }

        [Theory]
        [InlineData("123", true)]
        [InlineData("0", false)]
        [InlineData("-1", false)]
        [InlineData("not a number", false)]
        [InlineData(null, false)]
        public void IsPositiveInteger_ShouldReturnExpectedResult(string input, bool expected)
        {
            Assert.Equal(expected, Validator.IsPositiveInteger(input));
        }

        [Theory]
        [InlineData("1.0.0", "1.0.1", true)]
        [InlineData("1.0.0", "1.0.0", false)]
        [InlineData("1.0.1", "1.0.0", false)]
        public void IsVersionUpper_ShouldReturnExpectedResult(string oldVersion, string newVersion, bool expected)
        {
            Assert.Equal(expected, Validator.IsVersionUpper(oldVersion, newVersion));
        }

        [Theory]
        [InlineData("invalid", "invalid")]
        public void IsVersionUpper_WithInvalidVersion_ShouldThrow(string oldVersion, string newVersion)
        {
            Assert.Throws<ArgumentException>(() => Validator.IsVersionUpper(oldVersion, newVersion));
        }

        [Theory]
        [InlineData("123", true)]
        [InlineData("-456", true)]
        [InlineData("abc", false)]
        [InlineData("", false)]
        public void IsInt32_ShouldReturnExpectedResult(string input, bool expected)
        {
            Assert.Equal(expected, Validator.IsInt32(input));
        }

        [Theory]
        [InlineData("3.14", true)]
        [InlineData("123", true)]
        [InlineData("abc", false)]
        [InlineData("", false)]
        public void IsDouble_ShouldReturnExpectedResult(string input, bool expected)
        {
            Assert.Equal(expected, Validator.IsDouble(input));
        }

        [Theory]
        [InlineData("5.5", 0.0, 10.0, true)]
        [InlineData("15.0", 0.0, 10.0, false)]
        [InlineData("abc", 0.0, 10.0, false)]
        public void IsDouble_WithRange_ShouldReturnExpectedResult(string input, double min, double max, bool expected)
        {
            Assert.Equal(expected, Validator.IsDouble(input, min, max));
        }

        [Theory]
        [InlineData("123.45", true)]
        [InlineData("abc", false)]
        [InlineData("", false)]
        public void IsDecimal_ShouldReturnExpectedResult(string input, bool expected)
        {
            Assert.Equal(expected, Validator.IsDecimal(input));
        }

        [Theory]
        [InlineData("50.5", 0.0, 100.0, true)]
        [InlineData("150.0", 0.0, 100.0, false)]
        [InlineData("abc", 0.0, 100.0, false)]
        public void IsDecimal_WithRange_ShouldReturnExpectedResult(string input, decimal min, decimal max, bool expected)
        {
            Assert.Equal(expected, Validator.IsDecimal(input, min, max));
        }

        [Theory]
        [InlineData("2024-01-01", true)]
        [InlineData("01/01/2024", true)]
        [InlineData("invalid date", false)]
        [InlineData(null, false)]
        public void IsDateTime_ShouldReturnExpectedResult(string input, bool expected)
        {
            Assert.Equal(expected, Validator.IsDateTime(input));
        }

        [Theory]
        [InlineData("1.0.0", true)]
        [InlineData("2.0.0.0", true)]
        [InlineData("invalid", false)]
        public void IsVersion_ShouldReturnExpectedResult(string input, bool expected)
        {
            Assert.Equal(expected, Validator.IsVersion(input));
        }

        [Theory]
        [InlineData("test123", "test\\d+", true)]
        [InlineData("test123", "test\\d{4}", false)]
        [InlineData("", "test", false)]
        public void IsMatch_ShouldReturnExpectedResult(string input, string pattern, bool expected)
        {
            Assert.Equal(expected, Validator.IsMatch(input, pattern));
        }

        [Fact]
        public void IsMatch_WithRegexOptions_ShouldWork()
        {
            Assert.True(Validator.IsMatch("TEST123", "test\\d+", RegexOptions.IgnoreCase));
            Assert.False(Validator.IsMatch("TEST123", "test\\d+", RegexOptions.None));
        }
    }
}
