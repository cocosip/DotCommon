using DotCommon.Utility;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DotCommon.Test.Utility
{
    public class Sha1UtilTest
    {
        [Fact]
        public void GetStringSha1Hash_Test()
        {
            var expected1 = "4a48f2d80b881e9ac7d607f23412823e9305e433";
            var sha1_1 = ShaUtil.GetStringSha1Hash("HelloChinese");
            Assert.Equal(expected1, sha1_1, ignoreCase: true);

            var expected2 = "Skjy2AuIHprH1gfyNBKCPpMF5DM=";
            var sha1_2 = ShaUtil.GetBase64StringSha1Hash("HelloChinese");
            Assert.Equal(expected2, sha1_2, ignoreCase: true);
        }
    }
}
