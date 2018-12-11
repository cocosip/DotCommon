using DotCommon.Alg;
using Xunit;

namespace DotCommon.Test.Alg
{
    public class HmacSha1AlgTest
    {
        [Fact]
        public void HmacSha1_GetStringHmacSha1_Test()
        {
            var source = "helloworld";
            var hmacSha1Value = HmacSha1Alg.GetStringHmacSha1(source, "12345678");
            Assert.Equal("0ca0559b0a2c77bd91619fa6cec9044f5e567a56", hmacSha1Value.ToLower());
        }
    }
}
