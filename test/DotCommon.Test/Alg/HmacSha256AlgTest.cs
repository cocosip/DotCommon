using DotCommon.Alg;
using Xunit;

namespace DotCommon.Test.Alg
{
    public class HmacSha256AlgTest
    {
        [Fact]
        public void HmacSha256_GetStringHmacSha1_Test()
        {
            var source = "zero1000";
            var hmacSha256Value = HmacSha256Alg.GetStringHmacSha256(source, "111111");
            Assert.Equal("3885f54d0684044c1d5a7398346219b08ff9a9e3fe127cb7d3986516f6389d1e", hmacSha256Value.ToLower());
        }
    }
}