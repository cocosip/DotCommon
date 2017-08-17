using DotCommon.Utility;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DotCommon.Test.Utility
{
    public class PinYinUtilTest
    {

        [Theory]
        [InlineData("中国", "ZhongGuo")]
        public void ConvertChTest(string chStr, string pinyinStr)
        {
            var actual = PinYinUtil.ConvertCh(chStr);
            Assert.Equal(pinyinStr, actual);
        }

        [Theory]
        [InlineData("中国", "Z")]
        //[InlineData("美", "M")]
        public void GetFirstLetterTest(string chStr, string letter)
        {
            var actual = PinYinUtil.GetFirstLetter(chStr);
            Assert.Equal(letter, actual);
        }
    }
}
