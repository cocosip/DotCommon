using DotCommon.Encrypt;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Xunit;

namespace DotCommon.Test.Encrypt
{
    public class TripleDesEncryptorTest
    {
        [Fact]
        public void Encrypt_Decrypt_Test()
        {
            var source = "helloworld";
            var sourceBytes = Encoding.UTF8.GetBytes(source);
            var key = "MTIzNDU2Nzg4NzY1NDMyMQ=="; //1234567887654321
            var iv = "MTIzNDU2Nzg5MGFiY2RlZg==";//1234567890abcdef
            var tripleDesEncryptor1 = new TripleDesEncryptor(key, iv);
            tripleDesEncryptor1.Mode = CipherMode.CBC;
            tripleDesEncryptor1.Padding = PaddingMode.PKCS7;

            var encrypted1 = tripleDesEncryptor1.Encrypt(source);
            var decrypted1 = tripleDesEncryptor1.Decrypt(encrypted1);
            Assert.Equal(source, decrypted1);

            var encryptedx = tripleDesEncryptor1.Encrypt(sourceBytes);
            Assert.Equal(encrypted1, Convert.ToBase64String(encryptedx));


            var ivBytes = Convert.FromBase64String(iv);
            var tripleDesEncryptor2 = new TripleDesEncryptor(key, ivBytes);
            tripleDesEncryptor2.Mode = CipherMode.CBC;
            tripleDesEncryptor2.Padding = PaddingMode.PKCS7;
            var encrypted2 = tripleDesEncryptor2.Encrypt(source);
            Assert.Equal(encrypted1, encrypted2);

            var tripleDesEncryptor3= new TripleDesEncryptor(key);
            tripleDesEncryptor2.Mode = CipherMode.CBC;
            tripleDesEncryptor2.Padding = PaddingMode.PKCS7;
            var encrypted3 = tripleDesEncryptor3.Encrypt(source);
            var decrypted3 = tripleDesEncryptor3.Decrypt(encrypted3);
            Assert.Equal(source, decrypted3);

        }
    }
}
