using System;
using Xunit;

namespace DotCommon.Utility.Test
{
    /// <summary>
    /// Contains unit tests for the <see cref="RMBConverter"/> class.
    /// </summary>
    public class RMBConverterTest
    {
        [Theory]
        // Standard cases
        [InlineData("123.45", "壹佰贰拾叁元肆角伍分")]
        [InlineData("1001.01", "壹仟零壹元零壹分")]
        [InlineData("54321.99", "伍万肆仟叁佰贰拾壹元玖角玖分")]
        [InlineData("100000000", "壹亿元整")]
        [InlineData("100000001.50", "壹亿零壹元伍角")]

        // Edge cases with zeros
        [InlineData("0.12", "壹角贰分")]
        [InlineData("0.03", "叁分")]
        [InlineData("1.05", "壹元零伍分")]
        [InlineData("10.00", "壹拾元整")]
        [InlineData("1010.00", "壹仟零壹拾元整")]
        [InlineData("1010.10", "壹仟零壹拾元壹角")]
        [InlineData("100.00", "壹佰元整")]
        [InlineData("100010001.01", "壹亿零壹万零壹元零壹分")]
        [InlineData("10000.01", "壹万元零壹分")]

        // Boundary and special values
        [InlineData("0", "零元整")]
        [InlineData("not a number", "")]
        public void ToRmb_ShouldReturnExpectedResult(string input, string expected)
        {
            Assert.Equal(expected, RMBConverter.ToRmb(input));
        }

        [Fact]
        public void ToRmb_WithLargeNumber_ShouldReturnOverflow()
        {
            decimal largeNumber = 10000000000000000m;
            Assert.Throws<ArgumentOutOfRangeException>(() => RMBConverter.ToRmb(largeNumber));
        }
    }
}