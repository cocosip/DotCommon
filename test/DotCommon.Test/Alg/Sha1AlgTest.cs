using DotCommon.Utility;
using Xunit;

namespace DotCommon.Test.Alg
{
    public class Sha1AlgTest
    {
        [Fact]
        public void GetStringMd5HashTest()
        {
            var str = "abcdefg&!@#12233";
            Assert.Equal("4dc660c2cf9dbed0488139de346e26de62f9fb38", Sha1Util.GetStringSha1Hash(str).ToLower());
        }
    }
}
