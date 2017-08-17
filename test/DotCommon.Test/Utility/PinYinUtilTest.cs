using DotCommon.Utility;
using Xunit;

namespace DotCommon.Test.Utility
{
    public class PinYinUtilTest
    {

        [Theory]
        [InlineData("中国", "ZhongGuo")]
        [InlineData("美国", "MeiGuo")]
        [InlineData("浙江人", "ZheJiangRen")]
        [InlineData("在风光如画的天府之国", "ZaiFengGuangRuHuaDeTianFuZhiGuo")]
        [InlineData("Zh", "Zh")]
        public void ConvertToPinYin(string chStr, string pinyinStr)
        {
            var actual = PinYinUtil.ConvertToPinYin(chStr);
            Assert.Equal(pinyinStr, actual);
        }

        [Theory]
        [InlineData("中", "Z")]
        [InlineData("美", "M")]
        [InlineData("日本人", "RBR")]
        [InlineData("哈利路Y", "HLLY")]
        public void GetFirstLetterTest(string chStr, string letter)
        {
            var actual = PinYinUtil.GetCodstring(chStr);
            Assert.Equal(letter, actual);
        }
    }
}
