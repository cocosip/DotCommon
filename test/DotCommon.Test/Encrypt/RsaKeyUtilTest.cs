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


            var rsaParams1 = RsaUtil.ReadPrivateKeyInfo(keyPair.PrivateKey);

            var rsaParams2 = RsaUtil.ReadPrivateKeyInfo(keyPair.PrivateKey);
            var rsaPrivateKey2 = RsaUtil.ExportPrivateKeyPkcs8(rsaParams2);
            var rsaEncryptor2 = new RsaEncryptor();
            rsaEncryptor2.LoadPrivateKey(rsaPrivateKey2);

            var d3 = rsaEncryptor2.Decrypt(d1);
            Assert.Equal("hello", d3);

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

        /// <summary>PKCS1与PKCS8密钥转换
        /// </summary>
        [Fact]
        public void Pkcs8_Pkcs1_Conver_Test()
        {
            var keyPair = RsaUtil.GenerateKeyPair();
            var rsaEncryptor1 = new RsaEncryptor(keyPair.PublicKey, keyPair.PrivateKey);

            var pkcs8Key = RsaUtil.Pkcs1ToPkcs8(keyPair.PrivateKey);
            var rsaEncryptor2 = new RsaEncryptor(keyPair.PublicKey, pkcs8Key);

            var d1 = rsaEncryptor1.Encrypt("123456");
            var d2 = rsaEncryptor2.Encrypt("123456");
            //Assert.Equal(d1, d2);

            var d3 = rsaEncryptor1.Decrypt(d1);
            Assert.Equal("123456", d3);
            var d4 = rsaEncryptor2.Decrypt(d2);

            var pkcs1Key = RsaUtil.Pkcs8ToPkcs1(pkcs8Key);
            Assert.Equal(keyPair.PrivateKey, pkcs1Key);

        }

    }
}
