using DotCommon.Utility;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DotCommon.Test.Utility
{
    public class RegexUtilTest
    {
        [Fact]
        public void IsMatch_Test()
        {
            Assert.False(RegexUtil.IsMatch("", "\\d"));
        }

        [Theory]
        [InlineData("135888787878", false)]
        [InlineData("15868702117", true)]
        [InlineData("05778785632", false)]
        public void IsMobileNumberTest(string value, bool expected)
        {
            var actual = RegexUtil.IsMobileNumber(value);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("1122@q", false)]
        [InlineData("1185513330@qq.com", true)]
        [InlineData("存储@xx.net", true)]
        public void IsEmailAddressTest(string value, bool expected)
        {
            var actual = RegexUtil.IsEmailAddress(value);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("333.0.25.54", false)]
        [InlineData("10.9.254.198", true)]
        [InlineData("10.9.254", false)]
        [InlineData("255.255.255.0", true)]
        public void IsIpTest(string value, bool expected)
        {
            var actual = RegexUtil.IsIp(value);
            Assert.Equal(expected, actual);
        }


        [Theory]
        [InlineData("333.0", false)]
        [InlineData("10", true)]
        [InlineData("-3", false)]
        [InlineData("25.5", false)]
        [InlineData("0", false)]
        public void IsPositiveIntegerTest(string value, bool expected)
        {
            var actual = RegexUtil.IsPositiveInteger(value);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("12.0", false)]
        [InlineData("10", true)]
        [InlineData("-3", true)]
        [InlineData("25.5", false)]
        [InlineData("0", true)]
        [InlineData("+8", true)]
        public void IsInt32Test(string value, bool expected)
        {
            var actual = RegexUtil.IsInt32(value);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void IsDouble_Test()
        {
            Assert.True(RegexUtil.IsDouble("12.0", 2));
            Assert.False(RegexUtil.IsDouble("15.102", 2));
            Assert.False(RegexUtil.IsDouble("12s", 2));
        }

        [Fact]
        public void IsDecimal_Test()
        {
            Assert.False(RegexUtil.IsDecimal("12.322", 2));
            Assert.False(RegexUtil.IsDecimal("15.102", 2));
            Assert.True(RegexUtil.IsDecimal("11.32", 2));
        }

        [Theory]
        [InlineData("23.523", 12.8d, 52.3d, 3, true)]
        [InlineData("35.623", 10d, 100d, 2, false)]
        [InlineData("85.3", 90d, 100d, 2, false)]
        public void IsDouble_Between_Test(string source, double minValue, double maxValue, int digit, bool expected)
        {
            var actual = RegexUtil.IsDouble(source, minValue, maxValue, digit);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void IsDecimal_Between_Test()
        {
            Assert.True(RegexUtil.IsDecimal("12.55", 12m, 13m, 2));
            Assert.False(RegexUtil.IsDecimal("12.55", 13m, 14m, 2));
            Assert.False(RegexUtil.IsDecimal("12.55", 12m, 14m, 1));
        }

        [Fact]
        public void IsDataTime_Test()
        {
            Assert.False(RegexUtil.IsDataTime("11.32"));
            Assert.False(RegexUtil.IsDataTime(""));
            Assert.True(RegexUtil.IsDataTime("2019-05-23"));
            Assert.True(RegexUtil.IsDataTime("2019-1-3 18:00"));
        }


        [Theory]
        [InlineData("http://baiduc.com", true)]
        public void IsUrl_Test(string url, bool expected)
        {
            var actual = RegexUtil.IsUrl(url);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("388.223.124.2", 3, true)]
        [InlineData("388.223.4242.33.234.2", 2, false)]
        public void IsVersion_Test(string version, int length, bool expected)
        {
            var actual = RegexUtil.IsVersion(version, length);
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void IsVersionUpper_Test()
        {
            var oldVersion1 = "388.223.124.2";
            var newVersion1 = "388.524.2";
            Assert.True(RegexUtil.IsVersionUpper(oldVersion1, newVersion1));

            var oldVersion2 = "10.10.20";
            var newVersion2 = "10.10.20.3";
            Assert.True(RegexUtil.IsVersionUpper(oldVersion2, newVersion2));


            var oldVersion3 = "10.20.80";
            var newVersion3 = "10.10.3";
            Assert.False(RegexUtil.IsVersionUpper(oldVersion3, newVersion3));

            var oldVersion4 = "10.20";
            var newVersion4 = "10";
            Assert.Throws<ArgumentException>(() =>
            {
                RegexUtil.IsVersionUpper(oldVersion4, newVersion4);
            });

            var oldVersion5 = "10.";
            var newVersion5 = "10.222";
            Assert.Throws<ArgumentException>(() =>
            {
                RegexUtil.IsVersionUpper(oldVersion5, newVersion5);
            });


            var oldVersion6 = "10.10.20.32";
            var newVersion6 = "10.10";
            Assert.False(RegexUtil.IsVersionUpper(oldVersion6, newVersion6));

        }
    }
}
