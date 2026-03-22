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

        [Fact]
        public void StripFontTags_ShouldRemoveFontTags()
        {
            var html = "<font size='2'>Text</font>";
            Assert.Equal("", StringUtil.StripFontTags(html));
        }

        [Fact]
        public void StripSpanTags_ShouldRemoveSpanTags()
        {
            var html = "<span class='test'>Content</span>";
            Assert.Equal("", StringUtil.StripSpanTags(html));
        }

        [Fact]
        public void StripTableTags_ShouldRemoveTableTags()
        {
            var html = "<table><tr><td>Cell</td></tr></table>";
            Assert.Equal("", StringUtil.StripTableTags(html));
        }

        [Fact]
        public void StripAllHtmlTags_ShouldRemoveAllTags()
        {
            var html = "<div><span>Text</span><a href='#'>Link</a></div>";
            Assert.Equal("TextLink", StringUtil.StripAllHtmlTags(html));
        }

        [Fact]
        public void StripImgTags_ShouldRemoveImgTags()
        {
            var html = "<img src='test.jpg' />Text";
            Assert.Equal("Text", StringUtil.StripImgTags(html));
        }

        [Fact]
        public void StripObjectTags_ShouldRemoveObjectTags()
        {
            var html = "<object data='test.swf'>Content</object>Text";
            Assert.Equal("Text", StringUtil.StripObjectTags(html));
        }

        [Fact]
        public void StripScriptTags_ShouldRemoveScriptTags()
        {
            var html = "<script>alert('xss')</script>Safe";
            Assert.Equal("Safe", StringUtil.StripScriptTags(html));
        }

        [Fact]
        public void StripIframeTags_ShouldRemoveIframeTags()
        {
            var html = "<iframe src='test.html'>Content</iframe>Text";
            Assert.Equal("Text", StringUtil.StripIframeTags(html));
        }

        [Fact]
        public void StripStyleTags_ShouldRemoveStyleTags()
        {
            var html = "<style>.class { color: red; }</style>Text";
            Assert.Equal("Text", StringUtil.StripStyleTags(html));
        }

        [Fact]
        public void StripHtmlByPattern_ShouldRemoveMatchingPattern()
        {
            var html = "<custom>Text</custom>";
            Assert.Equal("", StringUtil.StripHtmlByPattern(html, "<custom[^>]*>.*?</custom>"));
        }

        [Fact]
        public void SanitizeForUrl_ShouldRemoveSpecialChars()
        {
            var input = "Hello-World;Test<>《》";
            Assert.Equal("HelloWorldTest", StringUtil.SanitizeForUrl(input));
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData("", "")]
        public void StripMethods_WithNullOrEmpty_ShouldReturnInput(string input, string expected)
        {
            Assert.Equal(expected, StringUtil.StripATags(input));
            Assert.Equal(expected, StringUtil.StripDivTags(input));
            Assert.Equal(expected, StringUtil.StripFontTags(input));
            Assert.Equal(expected, StringUtil.StripSpanTags(input));
            Assert.Equal(expected, StringUtil.StripTableTags(input));
            Assert.Equal(expected, StringUtil.StripAllHtmlTags(input));
            Assert.Equal(expected, StringUtil.StripImgTags(input));
            Assert.Equal(expected, StringUtil.StripObjectTags(input));
            Assert.Equal(expected, StringUtil.StripScriptTags(input));
            Assert.Equal(expected, StringUtil.StripIframeTags(input));
            Assert.Equal(expected, StringUtil.StripStyleTags(input));
            Assert.Equal(expected, StringUtil.SanitizeForUrl(input));
            Assert.Equal(expected, StringUtil.SanitizeSql(input));
            Assert.Equal(expected, StringUtil.EncodeForXml(input));
        }

        [Fact]
        public void TrimEnd_WithNullSource_ShouldReturnNull()
        {
            Assert.Null(StringUtil.TrimEnd(null, "test"));
        }
    }
}