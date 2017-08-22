using DotCommon.Utility;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DotCommon.Test.Utility
{
    public class RegexUtilTest
    {
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
    }
}
