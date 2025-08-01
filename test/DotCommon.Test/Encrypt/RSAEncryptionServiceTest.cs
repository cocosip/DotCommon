using DotCommon.Crypto.RSA;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using System;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Pkcs;

namespace DotCommon.Test.Encrypt
{
    public class RSAEncryptionServiceTest
    {
        private readonly RSAEncryptionService _rsaEncryptionService;

        public RSAEncryptionServiceTest()
        {
            _rsaEncryptionService = new RSAEncryptionService();
        }

        [Fact]
        public void GenerateRSAKeyPair_Test()
        {
            var keyPair = _rsaEncryptionService.GenerateRSAKeyPair(2048);
            Assert.NotNull(keyPair);
            Assert.NotNull(keyPair.Public);
            Assert.NotNull(keyPair.Private);
            Assert.False(keyPair.Public.IsPrivate); // Public key should not be private
            Assert.True(keyPair.Private.IsPrivate);  // Private key should be private
        }

        [Fact]
        public void ImportPublicKeyPem_Test()
        {
            var keyPair = _rsaEncryptionService.GenerateRSAKeyPair();
            var publicKeyPem = GetPublicKeyPem(keyPair.Public);
            var publicKey = _rsaEncryptionService.ImportPublicKeyPem(publicKeyPem);
            Assert.NotNull(publicKey);
            Assert.False(publicKey.IsPrivate);
        }

        [Fact]
        public void ImportPrivateKeyPem_Test()
        {
            var keyPair = _rsaEncryptionService.GenerateRSAKeyPair();
            var privateKeyPem = GetPrivateKeyPem(keyPair.Private);
            var privateKey = _rsaEncryptionService.ImportPrivateKeyPem(privateKeyPem);
            Assert.NotNull(privateKey);
            Assert.True(privateKey.IsPrivate);
        }

        [Fact]
        public void ImportPrivateKeyPkcs8Pem_Test()
        {
            var keyPair = _rsaEncryptionService.GenerateRSAKeyPair();
            var privateKeyPkcs8Pem = GetPrivateKeyPkcs8Pem(keyPair.Private);
            var privateKey = _rsaEncryptionService.ImportPrivateKeyPkcs8Pem(privateKeyPkcs8Pem);
            Assert.NotNull(privateKey);
            Assert.True(privateKey.IsPrivate);
        }

        [Theory]
        [InlineData("PKCS1Padding")]
        [InlineData("OAEPPadding")]
        [InlineData("OAEPSHA256Padding")]
        public void Encrypt_Decrypt_Test(string padding)
        {
            var keyPair = _rsaEncryptionService.GenerateRSAKeyPair();
            var plainText = Encoding.UTF8.GetBytes("Hello, RSA!");

            var encryptedText = _rsaEncryptionService.Encrypt(keyPair.Public, plainText, padding);
            Assert.NotNull(encryptedText);
            Assert.True(encryptedText.Length > 0);

            var decryptedText = _rsaEncryptionService.Decrypt(keyPair.Private, encryptedText, padding);
            Assert.NotNull(decryptedText);
            Assert.True(plainText.SequenceEqual(decryptedText));
        }

        [Fact]
        public void Encrypt_WithPrivateKey_ThrowsArgumentException()
        {
            var keyPair = _rsaEncryptionService.GenerateRSAKeyPair();
            var plainText = Encoding.UTF8.GetBytes("Hello, RSA!");
            Assert.Throws<ArgumentException>(() => _rsaEncryptionService.Encrypt(keyPair.Private, plainText, "PKCS1Padding"));
        }

        [Fact]
        public void Decrypt_WithPublicKey_ThrowsArgumentException()
        {
            var keyPair = _rsaEncryptionService.GenerateRSAKeyPair();
            var cipherText = Encoding.UTF8.GetBytes("Encrypted data");
            Assert.Throws<ArgumentException>(() => _rsaEncryptionService.Decrypt(keyPair.Public, cipherText, "PKCS1Padding"));
        }

        [Theory]
        [InlineData("SHA256WITHRSA")]
        [InlineData("SHA1WITHRSA")]
        public void Sign_VerifySign_Test(string algorithm)
        {
            var keyPair = _rsaEncryptionService.GenerateRSAKeyPair();
            var data = Encoding.UTF8.GetBytes("Data to be signed.");

            var signature = _rsaEncryptionService.Sign(keyPair.Private, data, algorithm);
            Assert.NotNull(signature);
            Assert.True(signature.Length > 0);

            var isValid = _rsaEncryptionService.VerifySign(keyPair.Public, data, signature, algorithm);
            Assert.True(isValid);
        }

        [Fact]
        public void Sign_WithPublicKey_ThrowsArgumentException()
        {
            var keyPair = _rsaEncryptionService.GenerateRSAKeyPair();
            var data = Encoding.UTF8.GetBytes("Data to be signed.");
            Assert.Throws<ArgumentException>(() => _rsaEncryptionService.Sign(keyPair.Public, data, "SHA256WITHRSA"));
        }

        [Fact]
        public void VerifySign_WithPrivateKey_ThrowsArgumentException()
        {
            var keyPair = _rsaEncryptionService.GenerateRSAKeyPair();
            var data = Encoding.UTF8.GetBytes("Data to be signed.");
            var signature = Encoding.UTF8.GetBytes("Signature");
            Assert.Throws<ArgumentException>(() => _rsaEncryptionService.VerifySign(keyPair.Private, data, signature, "SHA256WITHRSA"));
        }

        private string GetPublicKeyPem(AsymmetricKeyParameter publicKey)
        {
            using var stringWriter = new StringWriter();
            var pemWriter = new PemWriter(stringWriter);
            pemWriter.WriteObject(publicKey);
            return stringWriter.ToString();
        }

        private string GetPrivateKeyPem(AsymmetricKeyParameter privateKey)
        {
            using var stringWriter = new StringWriter();
            var pemWriter = new PemWriter(stringWriter);
            pemWriter.WriteObject(privateKey);
            return stringWriter.ToString();
        }

        private string GetPrivateKeyPkcs8Pem(AsymmetricKeyParameter privateKey)
        {
            using var stringWriter = new StringWriter();
            var pemWriter = new PemWriter(stringWriter);
            pemWriter.WriteObject(PrivateKeyInfoFactory.CreatePrivateKeyInfo(privateKey));
            return stringWriter.ToString();
        }
    }
}
