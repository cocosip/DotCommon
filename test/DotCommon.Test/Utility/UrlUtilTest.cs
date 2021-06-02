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
        [InlineData("", false)]
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
        [InlineData("", "https://baidu.com", false)]
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
        [InlineData("http://test.yjyj.com/Web/Product/Search?Key=沙发", "page", "2", false, "http://test.yjyj.com/Web/Product/Search?Key=%25E6%25B2%2599%25E5%258F%2591&page=2")]
        [InlineData("http://127.0.0.1?id=3", "id", "10", true, "http://127.0.0.1/?id=10")]
        [InlineData("http://127.0.0.1/", "", "30", true, "http://127.0.0.1/")]
        [InlineData("http://127.0.0.1/", "name", "", false, "http://127.0.0.1/")]
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

        [Theory]
        [InlineData("https://www.baidu.com#aaa", "https://www.baidu.com:443/")]
        [InlineData("http://www.baidu.com/search=1", "http://www.baidu.com:80/")]
        [InlineData("https://192.168.0.100:999/home?id=20", "https://192.168.0.100:999/")]
        [InlineData("HTTP://127.0.0.1/home", "http://127.0.0.1:80/")]
        public void GetHostUrlTest(string url, string expected)
        {
            var actual = UrlUtil.GetHostUrl(url);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("https://www.baidu.com#aaa", "https://www.baidu.com:443/#aaa")]
        [InlineData("http://www.baidu.com/search=1", "http://www.baidu.com:80/search=1")]
        [InlineData("https://192.168.0.100:999/home?id=20", "https://192.168.0.100:999/home?id=20")]
        [InlineData("HTTP://127.0.0.1/home", "http://127.0.0.1:80/home")]
        public void ParseUrl(string url, string expected)
        {
            var actual = UrlUtil.ParseUrl(url);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("http://www.baidu.com#aaa", "www.baidu.com")]
        [InlineData("https://192.168.0.1:8081?id=20", "192.168.0.1:8081")]
        [InlineData("HTTPs://10.9.254.168:80?id=3&name=4", "10.9.254.168:80")]
        public void GetAuthority_Test(string url, string authority)
        {
            var actual = UrlUtil.GetAuthority(url);
            Assert.Equal(authority, actual);
        }

        [Theory]
        [InlineData("http://www.baidu.com/abc/def", "http://www.baidu.com", "/abc", "def/")]
        [InlineData("http://10.9.254.19/qos/?id=3", "http://10.9.254.19", "", "/qos", "?id=3")]
        [InlineData("http://192.168.0.100/group1/01/0A/file1.jpg", "http://192.168.0.100/", "group1", "01/0A/file1.jpg")]
        public void CombineUrl_Test(string expected, params string[] parameters)
        {
            var actual = UrlUtil.CombineUrl(parameters);
            Assert.Equal(expected, actual);
        }

    }
}
