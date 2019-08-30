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
            var keyPair = RsaUtil.GenerateKeyPair(RSAKeyFormat.PKCS1, 512);

            Assert.NotEmpty(keyPair.PublicKey);
            var rsaEncryptor = new RsaEncryptor(keyPair.PublicKey, keyPair.PrivateKey);
            var d1 = rsaEncryptor.Encrypt("hello");
            var d2 = rsaEncryptor.Decrypt(d1);
            Assert.Equal("hello", d2);


            var sign = rsaEncryptor.SignData("string1");
            Assert.True(rsaEncryptor.VerifyData("string1", sign));


        }

        [Fact]
        public void GenerateFormatKeyPair_Test()
        {
            var keyPair = RsaUtil.GenerateFormatKeyPair(RSAKeyFormat.PKCS1, 1024);
            Assert.Contains("----", keyPair.PublicKey);
            Assert.Contains("----", keyPair.PrivateKey);

        }

        [Fact]
        public void TrimKey_Test()
        {
            var keyPair = RsaUtil.GenerateFormatKeyPair();
            var trimedPublicKey = RsaUtil.TrimKey(keyPair.PublicKey);
            var trimedPrivateKey = RsaUtil.TrimKey(keyPair.PrivateKey);
            Assert.Contains("-", keyPair.PublicKey);
            Assert.Contains("-", keyPair.PrivateKey);
            Assert.DoesNotContain("-", trimedPublicKey);
            Assert.DoesNotContain("-", trimedPrivateKey);
            var rsaEncryptor = new RsaEncryptor(trimedPublicKey, trimedPrivateKey);
            var encrypted1 = rsaEncryptor.Encrypt("china");
            var decrypted1 = rsaEncryptor.Decrypt(encrypted1);
            Assert.Equal("china", decrypted1);
        }

    }
}
