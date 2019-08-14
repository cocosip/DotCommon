using DotCommon.Utility;
using System.Text;
using Xunit;

namespace DotCommon.Test.Utility
{
    public class PinYinUtilTest
    {
        static PinYinUtilTest()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }
        [Theory]
        [InlineData("中华人民共和国", "ZhongHuaRenMinGongHeGuo")]
        [InlineData("美国", "MeiGuo")]
        [InlineData("真正浙江人", "ZhenZhengZheJiangRen")]
        [InlineData("在风光如画的天府之国", "ZaiFengGuangRuHuaDeTianFuZhiGuo")]
        [InlineData("Zh", "Zh")]
        [InlineData("千山鸟飞绝万径人踪灭", "QianShanNiaoFeiJueWanJingRenZongMie")]
        [InlineData("茅盾文学", "MaoDunWenXue")]
        public void ConvertToPinYin(string chStr, string pinyinStr)
        {
            var actual = PinYinUtil.ConvertToPinYin(chStr);
            Assert.Equal(pinyinStr, actual);
        }

        [Theory]
        [InlineData("安格拉阿哦", "AGLAO")]
        [InlineData("詹姆斯邦德", "ZMSBD")]
        [InlineData("德国警察说韩语", "DGJCSHY")]
        [InlineData("嘭膨膨作响", "?PPZX")]
        [InlineData("俄国犯人克能庆", "EGFRKNQ")]
        [InlineData("台湾", "TW")]
        public void GetFirstLetterTest(string chStr, string letter)
        {
            var actual = PinYinUtil.GetCodstring(chStr);
            Assert.Equal(letter, actual);
        }
    }
}
