using DotCommon.Encrypt;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DotCommon.Test.Encrypt
{
    public class Base64EncryptorTest
    {
        [Fact]
        public void Encrypt_Decrypt_Test()
        {
            var source1 = "helloworld";
            var base64Encryptor = new Base64Encryptor();
            var encrypted1 = base64Encryptor.Base64Encrypt(source1, "utf-8");
            var decrypted1= base64Encryptor.Base64Decrypt(encrypted1, "utf-8");
            Assert.Equal(source1, decrypted1);


            var source2 = "abc1234";
            var encrypted2 = "YWJjMTIzNA==";
            Assert.Equal(encrypted2, base64Encryptor.Base64Encrypt(source2));

            var encrypted3 = "YnJvdGhlcg==";
            var source3 = "brother";
            Assert.Equal(source3, base64Encryptor.Base64Decrypt(encrypted3));

        }

    }
}
