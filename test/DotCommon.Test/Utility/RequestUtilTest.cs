using Xunit;
using DotCommon.Utility;

namespace DotCommon.Test.Utility
{
    public class RequestUtilTest
    {
        [Theory]
        [InlineData("chrome_windows", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/46.0.2490.80 Safari/537.36")]
        [InlineData("safari_iphone", "Mozilla/5.0 (iPhone; U; CPU iPhone OS 4_3_3 like Mac OS X; en-us) AppleWebKit/533.17.9 (KHTML, like Gecko) Version/5.0.2 Mobile/8J2 Safari/6533.18.5")]
        [InlineData("non_existent_key", null)]
        [InlineData(null, null)]
        public void GetUserAgent_ShouldReturnCorrectAgent(string key, string expectedAgent)
        {
            var agent = RequestUtil.GetUserAgent(key);
            Assert.Equal(expectedAgent, agent);
        }

        [Theory]
        [InlineData("Mozilla/5.0 (iPhone; U; CPU iPhone OS 4_3_3 like Mac OS X; en-us) AppleWebKit/533.17.9 (KHTML, like Gecko) Version/5.0.2 Mobile/8J2 Safari/6533.18.5", true)]
        [InlineData("Mozilla/5.0 (Linux; U; Android 2.3.7; en-us; Nexus One Build/FRF91) AppleWebKit/533.1 (KHTML, like Gecko) Version/4.0 Mobile Safari/533.1", true)]
        [InlineData("Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/46.0.2490.80 Safari/537.36", false)]
        [InlineData("", false)]
        [InlineData(null, false)]
        public void IsMobile_ShouldCorrectlyIdentifyMobileAgents(string userAgent, bool isMobile)
        {
            Assert.Equal(isMobile, RequestUtil.IsMobile(userAgent));
        }

        [Theory]
        [InlineData("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36", MobilePlatform.Windows)]
        [InlineData("Mozilla/5.0 (iPhone; CPU iPhone OS 10_3_1 like Mac OS X) AppleWebKit/603.1.30 (KHTML, like Gecko) Version/10.0 Mobile/14E304 Safari/602.1", MobilePlatform.IPhone)]
        [InlineData("Mozilla/5.0 (Macintosh; Intel Mac OS X 10_12_6) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/61.0.3163.100 Safari/537.36", MobilePlatform.MacBook)]
        [InlineData("Mozilla/5.0 (Linux; Android 7.0; SM-G930V Build/NRD90M) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/59.0.3071.125 Mobile Safari/537.36", MobilePlatform.Android)]
        [InlineData("Unknown User Agent", "")]
        [InlineData(null, "")]
        public void GetPlatform_ShouldReturnCorrectPlatform(string userAgent, string expectedPlatform)
        {
            Assert.Equal(expectedPlatform, RequestUtil.GetPlatform(userAgent));
        }

        [Theory]
        [InlineData("Mozilla/5.0 (Linux; Android 7.0; MicroMessenger) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/59.0.3071.125 Mobile Safari/537.36", true)]
        [InlineData("Mozilla/5.0 (iPhone; CPU iPhone OS 10_3_1 like Mac OS X) AppleWebKit/603.1.30 (KHTML, like Gecko) Version/10.0 Mobile/14E304 Safari/602.1", false)]
        [InlineData("", false)]
        [InlineData(null, false)]
        public void IsWechatPlatform_ShouldCorrectlyIdentifyWechatBrowser(string userAgent, bool isWechat)
        {
            Assert.Equal(isWechat, RequestUtil.IsWechatPlatform(userAgent));
        }
    }
}