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

            var aesEncryptor = new AesEncryptor(Convert.ToBase64String(Encoding.GetEncoding("utf-8").GetBytes("1234567890abcdef")));
            var encryptedStr = aesEncryptor.Encrypt("helloworld");
            var actual = aesEncryptor.Decrypt(encryptedStr);
            Assert.Equal("helloworld", actual);
        }

        /// <summary>与第三方加密结果比对
        /// </summary>
        [Fact]
        public void EncryptResultTest()
        {
            var str = "helloworld";
            var key = "MTIzNDU2Nzg4NzY1NDMyMQ=="; //1234567887654321
            var iv = "MTIzNDU2Nzg5MGFiY2RlZg==";//1234567890abcdef
            var aesEncryptor = new AesEncryptor(key, iv);
            var encrypted = aesEncryptor.Encrypt(str);
            Assert.Equal("5vpVXOvT+drFQQSH3KXi6Q==", encrypted);
        }

    }
}
