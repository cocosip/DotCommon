using Xunit;

namespace DotCommon.Utility.Test
{
    /// <summary>
    /// Contains unit tests for the <see cref="Validator"/> class.
    /// </summary>
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
    }
}
