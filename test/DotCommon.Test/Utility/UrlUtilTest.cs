using DotCommon.Utility;
using Xunit;

namespace DotCommon.Test.Utility
{
    public class UrlUtilTest
    {
        [Theory]
        [InlineData("http://www.baidu", true)]
        [InlineData("http://127.0.0.1", true)]
        [InlineData("https:/abp.com", false)]
        [InlineData("www.baidu.com", false)]
        public void IsUrlTest(string url, bool expected)
        {
            var actual = UrlUtil.IsUrl(url);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("http://www.baidu", true)]
        [InlineData("http://127.0.0.1", true)]
        [InlineData("http://baidu.com", true)]
        public void IsMainDomainTest(string url, bool expected)
        {
            var actual = UrlUtil.IsUrl(url);
            Assert.Equal(expected, actual);
        }
    }
}
