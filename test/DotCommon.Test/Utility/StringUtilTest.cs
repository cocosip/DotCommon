using DotCommon.Utility;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DotCommon.Test.Utility
{
    public class StringUtilTest
    {
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
