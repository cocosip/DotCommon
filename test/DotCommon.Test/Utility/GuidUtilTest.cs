using DotCommon.Utility;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DotCommon.Test.Utility
{
    public class GuidUtilTest
    {
        [Fact]
        public void NewSequentialGuid_Test()
        {
            var guid1 = GuidUtil.NewSequentialGuid().ToString();
            Assert.Equal(36, guid1.Length);
            var guid2 = GuidUtil.NewSequentialGuid().ToString("N");
            Assert.Equal(32, guid2.Length);
        }


        [Fact]
        public void NewGuidString_Test()
        {
            var guid1 = GuidUtil.NewGuidString("N");
            var isContains = guid1.Contains("-");
            Assert.False(isContains);
        }


    }
}
