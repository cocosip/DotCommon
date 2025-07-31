using DotCommon.Utility;
using System.Text;
using Xunit;

namespace DotCommon.Test.Utility
{
    public class HMACUtilTest
    {
        [Fact]
        public void ComputeHmacSha1ToHex_Test()
        {
            var source = "helloworld";
            var hmacSha1Value = HMACUtil.ComputeHmacSha1ToHex(source, "12345678");
            Assert.Equal("0ca0559b0a2c77bd91619fa6cec9044f5e567a56", hmacSha1Value.ToLower());
        }

        [Fact]
        public void ComputeHmacSha1ToBase64_Test()
        {
            var source = "helloworld";
            var stringBase64HmacSha1 = HMACUtil.ComputeHmacSha1ToBase64(source, "111111");
            Assert.Equal("LY9OPidfBXHYmNJQ7ht4I49xYMU=", stringBase64HmacSha1);
        }

        [Fact]
        public void ComputeHmacSha256ToHex_Test()
        {
            var source = "zero1000";
            var hmacSha256Value = HMACUtil.ComputeHmacSha256ToHex(source, "111111");
            Assert.Equal("3885f54d0684044c1d5a7398346219b08ff9a9e3fe127cb7d3986516f6389d1e", hmacSha256Value.ToLower());
        }

        [Fact]
        public void ComputeHmacSha256ToBase64_Test()
        {
            var source = "zero1000";
            var stringBase64HmacSha256 = HMACUtil.ComputeHmacSha256ToBase64(source, "111111");
            Assert.Equal("OIX1TQaEBEwdWnOYNGIZsI/5qeP+Eny305hlFvY4nR4=", stringBase64HmacSha256);
        }

        [Fact]
        public void ComputeHmacMd5ToHex_Test()
        {
            var source = "zero1000";
            var hmacMd5Value = HMACUtil.ComputeHmacMd5ToHex(source, "111111");
            Assert.Equal("fd6685a35748328b6306021e5f69cbd6", hmacMd5Value.ToLower());
        }

        [Fact]
        public void ComputeHmacMd5ToBase64_Test()
        {
            var source = "zero1000";
            var stringBase64HmacMd5 = HMACUtil.ComputeHmacMd5ToBase64(source, "111111");
            Assert.Equal("/WaFo1dIMotjBgIeX2nL1g==", stringBase64HmacMd5);
        }

        [Fact]
        public void ComputeHmacSha1ToBase64_WithEncoding_Test()
        {
            var source = "你好世界";
            var key = "密钥";
            var result = HMACUtil.ComputeHmacSha1ToBase64(source, key, Encoding.UTF8, Encoding.UTF8);
            Assert.NotNull(result);
        }

        [Fact]
        public void ComputeHmacSha1ToHex_WithEncoding_Test()
        {
            var source = "你好世界";
            var key = "密钥";
            var result = HMACUtil.ComputeHmacSha1ToHex(source, key, Encoding.UTF8, Encoding.UTF8);
            Assert.NotNull(result);
        }

        [Fact]
        public void ComputeHmacSha256ToBase64_WithEncoding_Test()
        {
            var source = "你好世界";
            var key = "密钥";
            var result = HMACUtil.ComputeHmacSha256ToBase64(source, key, Encoding.UTF8, Encoding.UTF8);
            Assert.NotNull(result);
        }

        [Fact]
        public void ComputeHmacSha256ToHex_WithEncoding_Test()
        {
            var source = "你好世界";
            var key = "密钥";
            var result = HMACUtil.ComputeHmacSha256ToHex(source, key, Encoding.UTF8, Encoding.UTF8);
            Assert.NotNull(result);
        }

        [Fact]
        public void ComputeHmacMd5ToBase64_WithEncoding_Test()
        {
            var source = "你好世界";
            var key = "密钥";
            var result = HMACUtil.ComputeHmacMd5ToBase64(source, key, Encoding.UTF8, Encoding.UTF8);
            Assert.NotNull(result);
        }

        [Fact]
        public void ComputeHmacMd5ToHex_WithEncoding_Test()
        {
            var source = "你好世界";
            var key = "密钥";
            var result = HMACUtil.ComputeHmacMd5ToHex(source, key, Encoding.UTF8, Encoding.UTF8);
            Assert.NotNull(result);
        }
    }
}