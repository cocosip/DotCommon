using DotCommon.Utility;
using Xunit;

namespace DotCommon.Test.Alg
{
    public class HmacAlgTest
    {
        [Fact]
        public void HmacSha1_GetStringHmacSha1_Test()
        {
            var source = "helloworld";
            var hmacSha1Value = HmacUtil.GetStringHmacSha1(source, "12345678");
            Assert.Equal("0ca0559b0a2c77bd91619fa6cec9044f5e567a56", hmacSha1Value.ToLower());
        }

        [Fact]
        public void HmacSha256_GetStringHmacSha1_Test()
        {
            var source = "zero1000";
            var hmacSha256Value = HmacUtil.GetStringHmacSha256(source, "111111");
            Assert.Equal("3885f54d0684044c1d5a7398346219b08ff9a9e3fe127cb7d3986516f6389d1e", hmacSha256Value.ToLower());
        }

         [Fact]
        public void HmacMd5_GetStringHmacSha1_Test()
        {
            var source = "zero1000";
            var hmacMd5Value = HmacUtil.GetStringHmacMd5(source, "111111");
            Assert.Equal("fd6685a35748328b6306021e5f69cbd6", hmacMd5Value.ToLower());
        }
    }
}
