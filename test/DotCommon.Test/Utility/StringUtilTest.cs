using Xunit;
using DotCommon.Utility;

namespace DotCommon.Test.Utility
{
    public class StringUtilTest
    {
        [Theory]
        [InlineData("Hello World", 11)]
        [InlineData("你好世界", 8)]
        [InlineData("Hello你好", 9)]
        [InlineData("", 0)]
        [InlineData(null, 0)]
        public void GetEastAsianWidthCount_ShouldReturnCorrectWidth(string input, int expectedWidth)
        {
            Assert.Equal(expectedWidth, StringUtil.GetEastAsianWidthCount(input));
        }

        [Theory]
        [InlineData("TestString,", ",", "TestString")]
        [InlineData("AnotherTest", null, "AnotherTes")]
        [InlineData("NoSuffix", "XYZ", "NoSuffix")]
        [InlineData("", ",", "")]
        public void TrimEnd_ShouldRemoveSuffix(string source, string suffix, string expected)
        {
            Assert.Equal(expected, StringUtil.TrimEnd(source, suffix));
        }

        [Fact]
        public void HtmlFiltering_ShouldStripTagsCorrectly()
        {
            string html = "<div><a href='#'>Link</a><span>Text</span><img src=''/></div>";
            Assert.Equal("<div><span>Text</span><img src=''/></div>", StringUtil.StripATags(html));
            Assert.Equal("", StringUtil.StripDivTags(html));
        }

        [Theory]
        [InlineData("SELECT * FROM Users", "  * FROM Users")]
        [InlineData("DELETE FROM Products", "  FROM Products")]
        public void SanitizeSql_ShouldRemoveKeywords(string input, string expected)
        {
            Assert.Equal(expected, StringUtil.SanitizeSql(input));
        }

        [Fact]
        public void EncodeForXml_ShouldEncodeSpecialChars()
        {
            string xml = @"<tag attribute='value'>""content"" & more</tag>";
            string expected = @"&lt;tag attribute=&apos;value&apos;&gt;&quot;content&quot; &amp; more&lt;/tag&gt;";
            Assert.Equal(expected, StringUtil.EncodeForXml(xml));
        }

        [Theory]
        [InlineData("SafeString123", true)]
        [InlineData("Bad;String", false)]
        [InlineData("Another'Bad'String", false)]
        public void IsSqlSafe_ShouldDetectUnsafeChars(string input, bool isSafe)
        {
            Assert.Equal(isSafe, StringUtil.IsSqlSafe(input));
        }

        [Fact]
        public void UnicodeConversion_ShouldWorkCorrectly()
        {
            string original = "你好, World!";
            string unicode = "\\u4f60\\u597d, World!";
            Assert.Equal(unicode, StringUtil.ToUnicode(original));
            Assert.Equal(original, StringUtil.FromUnicode(unicode));
        }

        [Theory]
        [InlineData("1234567890", 3, 4, "123***7890")]
        [InlineData("myemail@example.com", 3, 3, "mye*************com")]
        [InlineData("short", 3, 3, "short")]
        [InlineData("123", 1, 1, "1*3")]
        public void Anonymize_ShouldMaskStringCorrectly(string input, int start, int end, string expected)
        {
            Assert.Equal(expected, StringUtil.Anonymize(input, start, end));
        }
    }
}