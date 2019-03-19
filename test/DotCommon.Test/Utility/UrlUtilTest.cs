using DotCommon.Utility;
using System.Collections.Generic;
using Xunit;

namespace DotCommon.Test.Utility
{
    public class UrlUtilTest
    {
        //[Theory]
        //[InlineData("http://www.baidu", true)]
        //[InlineData("http://127.0.0.1", true)]
        //[InlineData("https:/abp.com", false)]
        //[InlineData("www.baidu.com", false)]
        //public void IsUrlTest(string url, bool expected)
        //{
        //    var actual = UrlUtil.IsUrl(url);
        //    Assert.Equal(expected, actual);
        //}

        [Theory]
        [InlineData("http://www.baidu", true)]
        [InlineData("http://127.0.0.1", false)]
        [InlineData("http://baidu.com", true)]
        public void IsMainDomainTest(string url, bool expected)
        {
            var actual = UrlUtil.IsMainDomain(url);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("www.baidu.com", "http://", "http://www.baidu.com")]
        [InlineData("http://127.0.0.1", "", "http://127.0.0.1")]
        [InlineData("http://baidu.com", "https", "http://baidu.com")]
        public void PadLeftUrlTest(string url, string http, string expected)
        {
            var actual = UrlUtil.PadLeftUrl(url, http);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("www.baidu.com", "http://www.baidu.com", true)]
        [InlineData("http://127.0.0.1", "hTTP://127.0.0.1/2233", true)]
        [InlineData("http://baidu.com", "https://baidu.com", true)]
        public void SameDomainTest(string url, string url2, bool expected)
        {
            var actual = UrlUtil.SameDomain(url, url2);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetUrlParametersTest()
        {
            var url1 = "http://www.baidu.com?id=1&name=zhangsan&key=&value=val";
            var dict1 = UrlUtil.GetUrlParameters(url1);
            Assert.Equal("1", dict1["id"]);
            Assert.Equal("zhangsan", dict1["name"]);
            Assert.Equal("val", dict1["value"]);
            Assert.Throws<KeyNotFoundException>(() => { Assert.Equal("", dict1["key"]); });
        }

        [Fact]
        public void GetExpectUrlParametersTest()
        {
            var url1 = "http://127.0.0.1?id=3&name=n1&age=20";
            var dict1 = UrlUtil.GetExpectUrlParameters(url1, new[] { "age" });
            Assert.Equal("3", dict1["id"]);
            Assert.Equal("n1", dict1["name"]);
            Assert.Throws<KeyNotFoundException>(() => { Assert.Equal("20", dict1["age"]); });
        }

        [Theory]
        [InlineData("http://www.baidu.com#aaa", "id", "10", true, "http://www.baidu.com/?id=10#aaa")]
        [InlineData("http://www.baidu.com?id=20", "id", "10", false, "http://www.baidu.com/?id=20")]
        [InlineData("http://test.yjyj.com/Web/Product/Search?Key=沙发", "page", "2", false, "http://test.yjyj.com/Web/Product/Search?Key=沙发&page=2")]
        public void UrlAttachParameterTest(string url, string key, string value, bool replaceSame, string expected)
        {
            var actual = UrlUtil.UrlAttachParameter(url, key, value, replaceSame);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("http://www.baidu.com#aaa", false)]
        [InlineData("https://www.baidu.com?id=20", true)]
        [InlineData("HTTPs://www.taobao.com", true)]
        public void IsHttpsTest(string url, bool expected)
        {
            var actual = UrlUtil.IsHttps(url);
            Assert.Equal(expected, actual);
        }

    }
}
