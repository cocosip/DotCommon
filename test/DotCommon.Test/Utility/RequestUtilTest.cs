using DotCommon.Utility;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DotCommon.Test.Utility
{
    public class RequestUtilTest
    {
        [Fact]
        public void UserAgents_Count_Test()
        {
            var userAgents = RequestUtil.UserAgents;
            Assert.True(userAgents.Count > 1);
        }

        [Fact]
        public void GetPlatform_Test()
        {
            var userAgent = @"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/76.0.3809.100 Safari/537.36";
            var actual = RequestUtil.GetPlatform(userAgent);
            Assert.Equal("Windows", actual);
            Assert.False(RequestUtil.IsWechatPlatform(userAgent));
            Assert.False(RequestUtil.IsMobile(userAgent));

        }

        [Fact]
        public void GetUserAgent_Test()
        {
            var key = "chrome_windows";
            var expected = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/46.0.2490.80 Safari/537.36";
            var actual = RequestUtil.GetUserAgent(key);
            Assert.Equal(expected, actual);

        }
    }


}
