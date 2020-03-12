using DotCommon.Utility;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DotCommon.Test.Utility
{
    public class ShaUtilTest
    {
        [Fact]
        public void GetStringSha1Hash_Test()
        {
            var expected1 = "4a48f2d80b881e9ac7d607f23412823e9305e433";
            var sha1_1 = ShaUtil.GetHex16StringSha1Hash("HelloChinese");
            Assert.Equal(expected1, sha1_1, ignoreCase: true);

            var expected2 = "Skjy2AuIHprH1gfyNBKCPpMF5DM=";
            var sha1_2 = ShaUtil.GetBase64StringSha1Hash("HelloChinese");
            Assert.Equal(expected2, sha1_2, ignoreCase: true);
        }


        [Fact]
        public void GetStringSha256Hash_Test()
        {
            var expected1 = "872e4e50ce9990d8b041330c47c9ddd11bec6b503ae9386a99da8584e9bb12c4";
            var sha256_1 = ShaUtil.GetHex16StringSha256Hash("HelloWorld");
            Assert.Equal(expected1, sha256_1, ignoreCase: true);

            var expected2 = "hy5OUM6ZkNiwQTMMR8nd0Rvsa1A66ThqmdqFhOm7EsQ=";
            var sha256_2 = ShaUtil.GetBase64StringSha256Hash("HelloWorld");
            Assert.Equal(expected2, sha256_2, ignoreCase: true);
        }

        [Fact]
        public void GetStringSha512Hash_Test()
        {
            var expected1 = "ffcdc9277553ea18aae8c1260dd49c5e3bd1055f54fe194745e79f832de6402844efdea7b0c34428d75f640333cd310debf3de03b96c1f13debc8fee4ec91651";
            var sha512_1 = ShaUtil.GetHex16StringSha512Hash("Hinatahi");
            Assert.Equal(expected1, sha512_1, ignoreCase: true);

            var expected2 = "dgYH3MclklOrGdFiJZLJPj3P4L946XdpxsiyU50ZStk=";
            var sha512_2 = ShaUtil.GetBase64StringSha256Hash("Hinatahi");
            Assert.Equal(expected2, sha512_2, ignoreCase: true);
        }
    }
}
