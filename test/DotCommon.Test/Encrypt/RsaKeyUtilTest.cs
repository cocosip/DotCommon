using DotCommon.Encrypt;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DotCommon.Test.Encrypt
{
    /// <summary>RSA工具类测试
    /// </summary>
    public class RsaKeyUtilTest
    {
        /// <summary>生成密钥对测试
        /// </summary>
        [Fact]
        public void GenerateKeyPair_Test()
        {
            (string publicKey, string privateKey) = RsaKeyUtil.GenerateKeyPair(RSAKeyFormat.PKCS1, 512);

            Assert.NotEmpty(publicKey);
            var rsaEncryptor = new RsaEncryptor(publicKey, privateKey);
            var d1 = rsaEncryptor.Encrypt("hello");
            var d2 = rsaEncryptor.Decrypt(d1);
            Assert.Equal("hello", d2);
        }

        [Fact]
        public void GenerateFormatKeyPair_Test()
        {
            (string publicKey, string privateKey) = RsaKeyUtil.GenerateFormatKeyPair(RSAKeyFormat.PKCS1, 1024);
            Assert.Contains("----", publicKey);
            Assert.Contains("----", privateKey);

        }
    }
}
