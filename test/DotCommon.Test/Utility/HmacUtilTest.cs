using DotCommon.Utility;
using Xunit;

namespace DotCommon.Test.Utility
{
    public class HmacUtilTest
    {
        [Fact]
        public void HmacSha1_GetStringHmac_Test()
        {
            var source = "helloworld";
            var hmacSha1Value = HMACUtil.GetHex16StringHMACSHA1(source, "12345678");
            Assert.Equal("0ca0559b0a2c77bd91619fa6cec9044f5e567a56", hmacSha1Value.ToLower());

            var stringBase64HmacSha1 = HMACUtil.GetBase64StringHMACSHA1(source, "111111");
            Assert.Equal("LY9OPidfBXHYmNJQ7ht4I49xYMU=", stringBase64HmacSha1);

        }

        [Fact]
        public void HmacSha256_GetStringHmac_Test()
        {
            var source = "zero1000";
            var hmacSha256Value = HMACUtil.GetHex16StringHMACSHA256(source, "111111");
            Assert.Equal("3885f54d0684044c1d5a7398346219b08ff9a9e3fe127cb7d3986516f6389d1e", hmacSha256Value.ToLower());

            var stringBase64HmacSha256 = HMACUtil.GetBase64StringHMACSHA256(source, "111111");
            Assert.Equal("OIX1TQaEBEwdWnOYNGIZsI/5qeP+Eny305hlFvY4nR4=", stringBase64HmacSha256);

        }

        [Fact]
        public void HmacMd5_GetStringHmac_Test()
        {
            var source = "zero1000";
            var hmacMd5Value = HMACUtil.GetHex16StringHMACMD5(source, "111111");
            Assert.Equal("fd6685a35748328b6306021e5f69cbd6", hmacMd5Value.ToLower());

            var stringBase64HmacMd5 = HMACUtil.GetBase64StringHMACMD5(source, "111111");
            Assert.Equal("/WaFo1dIMotjBgIeX2nL1g==", stringBase64HmacMd5);

        }
    }
}
