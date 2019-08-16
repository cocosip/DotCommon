using DotCommon.Utility;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DotCommon.Test.Utility
{
    public class StringUtilTest
    {
        [Fact]
        public void GetStringByteLength_Test()
        {
            var source1 = "helloworld";
            Assert.Equal(10, StringUtil.GetStringByteLength(source1));
            var source2 = "中国?";
            Assert.Equal(6, StringUtil.GetStringByteLength(source2));

        }
        [Theory]
        [InlineData("helloeter", "ter", "helloe")]
        [InlineData("", "ter", "")]
        [InlineData("teacher", "", "teache")]
        public void RemoveEnd_Test(string input, string splitStr, string expected)
        {
            var actual = StringUtil.RemoveEnd(input, splitStr);
            Assert.Equal(expected, actual);
        }

        //[Fact]
        //public void Filter_Test()
        //{
        //    var s1 = StringUtil.FilterSpecial("heell:wqqq");
        //    Assert.Equal("heell:wqqq", s1);

        //}



        /// <summary>字符串转Unicode
        /// </summary>
        [Fact]
        public void StringToUnicodeTest()
        {
            var str = "你好,hello";
            var unicodeStr = StringUtil.StringToUnicode(str);
            Assert.Equal(@"\u4f60\u597d,hello", unicodeStr);
        }

        /// <summary>Unicode转中文字符串
        /// </summary>
        [Fact]
        public void UnicodeToStringTest()
        {
            var unicodeStr = @"a\u6211\u662f\u4e2d\u56fd\u4eba\u3002\u54c8\u54c8hello.";
            var str = StringUtil.UnicodeToString(unicodeStr);
            Assert.Equal("a我是中国人。哈哈hello.", str);

            var str2 = "Thisis我,哈哈";

            var unicodeStr2 = StringUtil.StringToUnicode(str2, false);
            var str2_result = StringUtil.UnicodeToString(unicodeStr2);
            Assert.Equal(str2, str2_result);

        }


    }
}
