using DotCommon.Encrypt;
using Xunit;

namespace DotCommon.Test.Encrypt
{
    public class MD5HelperTest
    {
        [Fact]
        public void GetMd5Test()
        {
            var actual = MD5Helper.GetMD5("helloworld");
            Assert.Equal("FC5E038D38A57032085441E7FE7010B0", actual);
        }
    }
}
