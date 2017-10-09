using DotCommon.Utility;
using Xunit;

namespace DotCommon.Test.Utility
{
    public class HexUtilTest
    {
        [Theory]
        [InlineData("5", 10, 2, "101")]
        [InlineData("15", 10, 8, "17")]
        [InlineData("101001", 2, 16, "29")]
        public void HexConvertTest(string value, int fromBase, int toBase, string expected)
        {
            var actual = HexUtil.HexConvert(value, fromBase, toBase);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("123", 8, 83)]
        [InlineData("10011001", 2, 153)]
        [InlineData("12d1e", 16, 77086)]
        public void ToHex10Test(string value, int fromBase, int expected)
        {
            var actual = HexUtil.ToHex10(value, fromBase);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(13, 2, "1101")]
        [InlineData(234, 8, "352")]
        [InlineData(521, 16, "209")]
        public void ToTargetHexTest(int hex10, int toBase, string expected)
        {
            var actual = HexUtil.ToTargetHex(hex10, toBase);
            Assert.Equal(expected, actual);
        }
    }
}
