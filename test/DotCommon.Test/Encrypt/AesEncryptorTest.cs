using DotCommon.Encrypt;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
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
            aesEncryptor.Mode = CipherMode.CBC;
            aesEncryptor.Padding = PaddingMode.PKCS7;
            aesEncryptor.KeySize = 128;


            var encryptedStr = aesEncryptor.Encrypt("helloworld");
            var actual = aesEncryptor.Decrypt(encryptedStr);
            Assert.Equal("helloworld", actual);

            //加密后的二进制
            var encryptedData = aesEncryptor.Encrypt(Encoding.UTF8.GetBytes("helloworld"));
            Assert.Equal(encryptedStr, Convert.ToBase64String(encryptedData));

            //加密的二进制
            var source = aesEncryptor.Decrypt(encryptedData);
            var actualSourceString = Encoding.GetEncoding("utf-8").GetString(source);
            Assert.Equal("helloworld", actualSourceString);
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


            var keyBytes = Convert.FromBase64String(key);
            var ivBytes = Convert.FromBase64String(iv);
            var aesEncryptor2 = new AesEncryptor(keyBytes, ivBytes);
            var encrypted2 = aesEncryptor2.Encrypt(str);
            Assert.Equal(encrypted, encrypted2);
            Assert.Equal("5vpVXOvT+drFQQSH3KXi6Q==", encrypted2);

        }

    }
}
