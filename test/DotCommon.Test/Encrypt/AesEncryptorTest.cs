using DotCommon.Encrypt;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DotCommon.Test.Encrypt
{
    public class AesEncryptorTest
    {
        [Fact]
        public void EncryptTest()
        {
            var aesEncryptor = new AesEncryptor("1234567890abcdef");
            var encryptedStr = aesEncryptor.Encrypt("helloworld");
            var actual = aesEncryptor.Decrypt(encryptedStr);
            Assert.Equal("helloworld", actual);
        }

    }
}
