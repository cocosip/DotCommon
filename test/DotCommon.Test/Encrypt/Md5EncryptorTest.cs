using DotCommon.Encrypt;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DotCommon.Test.Encrypt
{
    public class Md5EncryptorTest
    {
        [Fact]
        public void GetMd5Test()
        {
            var actual = Md5Encryptor.GetMd5("helloworld");
            Assert.Equal("FC5E038D38A57032085441E7FE7010B0", actual);
        }

    }
}
