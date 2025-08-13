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
using Microsoft.Extensions.DependencyInjection;

namespace DotCommon.Test.Encrypt
{
    /// <summary>
    /// Unit tests for RSA encryption service using dependency injection
    /// </summary>
    public class RSAEncryptionServiceTest : IDisposable
    {
        private readonly IRSAEncryptionService _rsaEncryptionService;
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Initializes a new instance of the RSAEncryptionServiceTest class
        /// Sets up dependency injection container with RSA encryption service
        /// </summary>
        public RSAEncryptionServiceTest()
        {
            var services = new ServiceCollection();
            services.AddDotCommonCrypto();
            _serviceProvider = services.BuildServiceProvider();
            _rsaEncryptionService = _serviceProvider.GetRequiredService<IRSAEncryptionService>();
        }

        /// <summary>
        /// Disposes the service provider to release resources
        /// </summary>
        public void Dispose()
        {
            if (_serviceProvider is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }

        [Fact]
        public void DependencyInjection_ServiceResolution_Test()
        {
            // Verify that the service is properly injected and not null
            Assert.NotNull(_rsaEncryptionService);
            Assert.NotNull(_serviceProvider);
            
            // Verify that we can resolve the service multiple times and get the same type
            var anotherService = _serviceProvider.GetRequiredService<IRSAEncryptionService>();
            Assert.NotNull(anotherService);
            Assert.IsType<RSAEncryptionService>(anotherService);
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

        [Fact]
        public void ImportPublicKey_Test()
        {
            var keyPair = _rsaEncryptionService.GenerateRSAKeyPair();
            var publicKeyBase64 = keyPair.Public.ExportPublicKey();
            var importedPublicKey = _rsaEncryptionService.ImportPublicKey(publicKeyBase64);
            Assert.NotNull(importedPublicKey);
            Assert.False(importedPublicKey.IsPrivate);
            
            // Test encryption/decryption to verify the key works correctly
            var plainText = Encoding.UTF8.GetBytes("Hello World");
            var encrypted = _rsaEncryptionService.Encrypt(importedPublicKey, plainText, RSAPaddingNames.PKCS1Padding);
            var decrypted = _rsaEncryptionService.Decrypt(keyPair.Private, encrypted, RSAPaddingNames.PKCS1Padding);
            Assert.Equal(plainText, decrypted);
        }

        [Fact]
        public void ImportPrivateKey_Test()
        {
            var keyPair = _rsaEncryptionService.GenerateRSAKeyPair();
            var privateKeyBase64 = keyPair.Private.ExportPrivateKey();
            var importedPrivateKey = _rsaEncryptionService.ImportPrivateKey(privateKeyBase64);
            Assert.NotNull(importedPrivateKey);
            Assert.True(importedPrivateKey.IsPrivate);
            
            // Test encryption/decryption to verify the key works correctly
            var plainText = Encoding.UTF8.GetBytes("Hello World");
            var encrypted = _rsaEncryptionService.Encrypt(keyPair.Public, plainText, RSAPaddingNames.PKCS1Padding);
            var decrypted = _rsaEncryptionService.Decrypt(importedPrivateKey, encrypted, RSAPaddingNames.PKCS1Padding);
            Assert.Equal(plainText, decrypted);
        }

        [Fact]
        public void ExportImportPublicKey_RoundTrip_Test()
        {
            var keyPair = _rsaEncryptionService.GenerateRSAKeyPair();
            var exportedPublicKey = keyPair.Public.ExportPublicKey();
            var importedPublicKey = _rsaEncryptionService.ImportPublicKey(exportedPublicKey);
            
            // Verify both keys can encrypt the same data and produce the same result
            var plainText = Encoding.UTF8.GetBytes("Test Message");
            var encrypted1 = _rsaEncryptionService.Encrypt(keyPair.Public, plainText, RSAPaddingNames.PKCS1Padding);
            var encrypted2 = _rsaEncryptionService.Encrypt(importedPublicKey, plainText, RSAPaddingNames.PKCS1Padding);
            
            // Both should decrypt to the same plaintext
            var decrypted1 = _rsaEncryptionService.Decrypt(keyPair.Private, encrypted1, RSAPaddingNames.PKCS1Padding);
            var decrypted2 = _rsaEncryptionService.Decrypt(keyPair.Private, encrypted2, RSAPaddingNames.PKCS1Padding);
            Assert.Equal(plainText, decrypted1);
            Assert.Equal(plainText, decrypted2);
        }

        [Fact]
        public void ExportImportPrivateKey_RoundTrip_Test()
        {
            var keyPair = _rsaEncryptionService.GenerateRSAKeyPair();
            var exportedPrivateKey = keyPair.Private.ExportPrivateKey();
            var importedPrivateKey = _rsaEncryptionService.ImportPrivateKey(exportedPrivateKey);
            
            // Verify both keys can decrypt the same data
            var plainText = Encoding.UTF8.GetBytes("Test Message");
            var encrypted = _rsaEncryptionService.Encrypt(keyPair.Public, plainText, RSAPaddingNames.PKCS1Padding);
            
            var decrypted1 = _rsaEncryptionService.Decrypt(keyPair.Private, encrypted, RSAPaddingNames.PKCS1Padding);
            var decrypted2 = _rsaEncryptionService.Decrypt(importedPrivateKey, encrypted, RSAPaddingNames.PKCS1Padding);
            Assert.Equal(plainText, decrypted1);
            Assert.Equal(plainText, decrypted2);
        }

        [Fact]
        public void EncryptFromBase64_Test()
        {
            var keyPair = _rsaEncryptionService.GenerateRSAKeyPair(2048);
            var publicKeyBase64 = keyPair.Public.ExportPublicKey();
            var plainText = "Hello, World!";
            
            var encrypted = _rsaEncryptionService.EncryptFromBase64(publicKeyBase64, plainText);
            var decrypted = _rsaEncryptionService.Decrypt(keyPair.Private, encrypted);
            
            Assert.Equal(plainText, decrypted);
        }

        [Fact]
        public void DecryptFromBase64_Test()
        {
            var keyPair = _rsaEncryptionService.GenerateRSAKeyPair(2048);
            var privateKeyBase64 = keyPair.Private.ExportPrivateKey();
            var plainText = "Hello, World!";
            var encrypted = _rsaEncryptionService.Encrypt(keyPair.Public, plainText);
            
            var decrypted = _rsaEncryptionService.DecryptFromBase64(privateKeyBase64, encrypted);
            
            Assert.Equal(plainText, decrypted);
        }

        [Fact]
        public void SignFromBase64_Test()
        {
            var keyPair = _rsaEncryptionService.GenerateRSAKeyPair(2048);
            var privateKeyBase64 = keyPair.Private.ExportPrivateKey();
            var data = "Hello, World!";
            
            var signature = _rsaEncryptionService.SignFromBase64(privateKeyBase64, data);
            var isValid = _rsaEncryptionService.VerifySign(keyPair.Public, data, signature);
            
            Assert.True(isValid);
        }

        [Fact]
        public void VerifySignFromBase64_Test()
        {
            var keyPair = _rsaEncryptionService.GenerateRSAKeyPair(2048);
            var publicKeyBase64 = keyPair.Public.ExportPublicKey();
            var data = "Hello, World!";
            var signature = _rsaEncryptionService.Sign(keyPair.Private, data);
            
            var isValid = _rsaEncryptionService.VerifySignFromBase64(publicKeyBase64, data, signature);
            
            Assert.True(isValid);
        }

        [Fact]
        public void EncryptDecryptFromBase64_RoundTrip_Test()
        {
            var keyPair = _rsaEncryptionService.GenerateRSAKeyPair(2048);
            var publicKeyBase64 = keyPair.Public.ExportPublicKey();
            var privateKeyBase64 = keyPair.Private.ExportPrivateKey();
            var originalData = "Hello, World!";
            
            var encrypted = _rsaEncryptionService.EncryptFromBase64(publicKeyBase64, originalData);
            var decrypted = _rsaEncryptionService.DecryptFromBase64(privateKeyBase64, encrypted);
            
            Assert.Equal(originalData, decrypted);
        }

        [Fact]
        public void SignVerifyFromBase64_RoundTrip_Test()
        {
            var keyPair = _rsaEncryptionService.GenerateRSAKeyPair(2048);
            var publicKeyBase64 = keyPair.Public.ExportPublicKey();
            var privateKeyBase64 = keyPair.Private.ExportPrivateKey();
            var data = "Hello, World!";
            
            var signature = _rsaEncryptionService.SignFromBase64(privateKeyBase64, data);
            var isValid = _rsaEncryptionService.VerifySignFromBase64(publicKeyBase64, data, signature);
            
            Assert.True(isValid);
        }
    }
}
