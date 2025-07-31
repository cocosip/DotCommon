using DotCommon.Utility;
using System.Text;
using Xunit;

namespace DotCommon.Test.Utility
{
    public class SHAUtilTest
    {
        [Fact]
        public void ComputeSha1ToHex_Test()
        {
            var expected = "4a48f2d80b881e9ac7d607f23412823e9305e433";
            var sha1 = SHAUtil.ComputeSha1ToHex("HelloChinese");
            Assert.Equal(expected, sha1, ignoreCase: true);
        }

        [Fact]
        public void ComputeSha1ToBase64_Test()
        {
            var expected = "Skjy2AuIHprH1gfyNBKCPpMF5DM=";
            var sha1 = SHAUtil.ComputeSha1ToBase64("HelloChinese");
            Assert.Equal(expected, sha1, ignoreCase: true);
        }

        [Fact]
        public void ComputeSha256ToHex_Test()
        {
            var expected = "872e4e50ce9990d8b041330c47c9ddd11bec6b503ae9386a99da8584e9bb12c4";
            var sha256 = SHAUtil.ComputeSha256ToHex("HelloWorld");
            Assert.Equal(expected, sha256, ignoreCase: true);
        }

        [Fact]
        public void ComputeSha256ToBase64_Test()
        {
            var expected = "hy5OUM6ZkNiwQTMMR8nd0Rvsa1A66ThqmdqFhOm7EsQ=";
            var sha256 = SHAUtil.ComputeSha256ToBase64("HelloWorld");
            Assert.Equal(expected, sha256, ignoreCase: true);
        }

        [Fact]
        public void ComputeSha512ToHex_Test()
        {
            var expected = "ffcdc9277553ea18aae8c1260dd49c5e3bd1055f54fe194745e79f832de6402844efdea7b0c34428d75f640333cd310debf3de03b96c1f13debc8fee4ec91651";
            var sha512 = SHAUtil.ComputeSha512ToHex("Hinatahi");
            Assert.Equal(expected, sha512, ignoreCase: true);
        }

        [Fact]
        public void ComputeSha512ToBase64_Test()
        {
            var expected = "ZQp2BVSIKdSMldXMxZozrUm7elOK3W3NwEKjkmpdZ8A2YTwGEYJxdihov55phjUXGaLixs+IjA8vNoktL8PIHg==";
            var sha512 = SHAUtil.ComputeSha512ToBase64("Helloword");
            Assert.Equal(expected, sha512, ignoreCase: true);
        }

        [Fact]
        public void ComputeSha1ToHex_WithEncoding_Test()
        {
            var source = "你好世界";
            var result = SHAUtil.ComputeSha1ToHex(source, Encoding.UTF8);
            Assert.NotNull(result);
        }

        [Fact]
        public void ComputeSha1ToBase64_WithEncoding_Test()
        {
            var source = "你好世界";
            var result = SHAUtil.ComputeSha1ToBase64(source, Encoding.UTF8);
            Assert.NotNull(result);
        }

        [Fact]
        public void ComputeSha256ToHex_WithEncoding_Test()
        {
            var source = "你好世界";
            var result = SHAUtil.ComputeSha256ToHex(source, Encoding.UTF8);
            Assert.NotNull(result);
        }

        [Fact]
        public void ComputeSha256ToBase64_WithEncoding_Test()
        {
            var source = "你好世界";
            var result = SHAUtil.ComputeSha256ToBase64(source, Encoding.UTF8);
            Assert.NotNull(result);
        }

        [Fact]
        public void ComputeSha512ToHex_WithEncoding_Test()
        {
            var source = "你好世界";
            var result = SHAUtil.ComputeSha512ToHex(source, Encoding.UTF8);
            Assert.NotNull(result);
        }

        [Fact]
        public void ComputeSha512ToBase64_WithEncoding_Test()
        {
            var source = "你好世界";
            var result = SHAUtil.ComputeSha512ToBase64(source, Encoding.UTF8);
            Assert.NotNull(result);
        }
    }
}