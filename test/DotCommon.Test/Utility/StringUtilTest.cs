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

        [Fact]
        public void FetchDiv_Test()
        {
            var s1 = StringUtil.FetchDiv("<div>zhangsan</div>");
            Assert.Equal("zhangsan", s1);

        }

        [Fact]
        private void FetchA_Test()
        {
            var s1 = StringUtil.FetchA("<a>sssqqq</a>");
            Assert.Equal("sssqqq", s1);
        }

        [Fact]
        private void FetchFont_Test()
        {
            var s1 = StringUtil.FetchFont("<font>font</font>");
            Assert.Equal("font", s1);
        }

        [Fact]
        public void FetchSpanSpan_Test()
        {
            var s1 = StringUtil.FetchSpan("<span>aaa</span>");
            Assert.Equal("aaa", s1);
        }


        [Fact]
        private void FilterImg_Test()
        {
            var s1 = StringUtil.FilterImg("aaa<img src=\"pic/aa/bb\"/>");
            Assert.Equal("aaa", s1);
        }

        [Fact]
        public void FilterObject_Test()
        {
            var s1 = StringUtil.FilterObject("<object xxx>xxxxx</object>");
            Assert.Equal("", s1);
        }

        [Fact]
        public void FilterScript_Test()
        {
            var s1 = StringUtil.FilterScript("<script>aaa</script>");
            Assert.Equal("", s1);
        }

        [Fact]
        public void FilterIFrame_Test()
        {
            var s1 = StringUtil.FilterIFrame("<iframe>aaa</iframe>");
            Assert.Equal("", s1);
        }

        [Fact]
        public void FilterStyle_Test()
        {
            var s1 = StringUtil.FilterStyle("<Style>aaa</Style>");
            Assert.Equal("", s1);
        }

        [Fact]
        public void FetchTableProtery_Test()
        {
            var s1 = StringUtil.FetchTableProtery("<table><tr>11111<tr><td>xxxxx</td></table>");
            Assert.Equal("11111xxxxx", s1);
        }

        [Fact]
        public void FetchStripTags_Test()
        {
            var s1 = StringUtil.FetchStripTags("<hell/><a>xxx</a>yyy<img src=\"123\">");
            Assert.Equal("xxxyyy", s1);
        }

        [Fact]
        public void HtmlToTxt_Test()
        {
            var s1 = StringUtil.HtmlToTxt("<html><head>xxx</head><title>测试<title/><script></script></html>");
            Assert.Equal("xxx测试", s1);
        }

        [Fact]
        public void SqlFilter_Test()
        {
            var sql1 = "select * from table1 where id=3";
            var s1 = StringUtil.SqlFilter(sql1);
            Assert.Equal("  * from table1 where id=3", s1);
        }


        /// <summary>字符串转Unicode
        /// </summary>
        [Fact]
        public void StringToUnicodeTest()
        {
            var s1 = "";
            Assert.Equal("", StringUtil.StringToUnicode(s1));

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

        [Fact]
        public void FilterHtml_Test()
        {
            var s1 = "";
            Assert.Equal("", StringUtil.FilterHtml(s1));

            var s2 = "<head>zhsangsan</head>";
            Assert.Equal("zhsangsan", StringUtil.FilterHtml(s2));
        }

        [Fact]
        public void SuperiorHtml_Test()
        {
            var s1 = "";
            Assert.Equal("", StringUtil.SuperiorHtml(s1, ""));

            var s2 = "<header>zhangsan";
            Assert.Equal("zhangsan", StringUtil.SuperiorHtml(s2, @"<(.|\n)+?>"));
        }

        [Fact]
        public void XmlEncode_Test()
        {
            var s1 = "";
            Assert.Equal("", StringUtil.XmlEncode(s1));

            var s2 = "<body>zhangsan</body>";
            Assert.Equal("&lt;body&gt;zhangsan&lt;/body&gt;", StringUtil.XmlEncode(s2));
        }

        [Fact]
        public void IsSafeSqlString_Test()
        {
            var s1 = "SELECT * FROM t1 WHERE Name LIKE '%ZA%'";
            Assert.False(StringUtil.IsSafeSqlString(s1));
        }

        [Fact]
        public void Anonymous_Test()
        {
            var n1 = "周";
            var n2 = "张三";
            var n3 = "曾国藩";
            var n4 = "ZHANGJIAGANG";

            var v1 = StringUtil.Anonymous(n1, 4);
            Assert.Equal("****", v1);

            var v2 = StringUtil.Anonymous(n2, 4);
            Assert.Equal("***三", v2);

            var v3 = StringUtil.Anonymous(n3, 4);
            var v3_3 = StringUtil.Anonymous(n3, 3);
            Assert.Equal("***藩", v3);
            Assert.Equal("曾*藩", v3_3);

            var v4 = StringUtil.Anonymous(n4, 4);
            Assert.Equal("Z**G", v4);
        }

    }
}
