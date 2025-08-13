using DotCommon.Crypto.SM2;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Utilities.Encoders;
using System;
using System.Text;
using Xunit;

namespace DotCommon.Test.Encrypt
{
    /// <summary>
    /// Unit tests for SM2 encryption service using dependency injection
    /// </summary>
    public class Sm2EncryptionServiceTest : IDisposable
    {
        private readonly ISm2EncryptionService _sm2EncryptionService;
        private readonly ServiceProvider _serviceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="Sm2EncryptionServiceTest"/> class.
        /// Sets up dependency injection container and resolves ISm2EncryptionService.
        /// </summary>
        public Sm2EncryptionServiceTest()
        {
            var services = new ServiceCollection();
            
            // Configure SM2 encryption options
            services.Configure<DotCommonSm2EncryptionOptions>(options =>
            {
                options.DefaultCurve = Sm2EncryptionNames.CurveSm2p256v1;
            });
            
            // Register SM2 encryption service
            services.AddTransient<ISm2EncryptionService, Sm2EncryptionService>();
            
            _serviceProvider = services.BuildServiceProvider();
            _sm2EncryptionService = _serviceProvider.GetRequiredService<ISm2EncryptionService>();
        }

        /// <summary>
        /// Tests that ISm2EncryptionService can be resolved from dependency injection container
        /// </summary>
        [Fact]
        public void DependencyInjection_ServiceResolution_Test()
        {
            // Assert that the service was resolved successfully
            Assert.NotNull(_sm2EncryptionService);
            Assert.IsAssignableFrom<ISm2EncryptionService>(_sm2EncryptionService);
            Assert.IsType<Sm2EncryptionService>(_sm2EncryptionService);
        }

        /// <summary>
        /// Disposes the service provider to release resources
        /// </summary>
        public void Dispose()
        {
            _serviceProvider?.Dispose();
        }

        [Fact]
        public void GenerateSm2KeyPair_Test()
        {
            var keyPair = _sm2EncryptionService.GenerateSm2KeyPair();
            Assert.NotNull(keyPair);
            Assert.NotNull(keyPair.Public);
            Assert.NotNull(keyPair.Private);
            Assert.False(keyPair.Public.IsPrivate);
            Assert.True(keyPair.Private.IsPrivate);
        }

        [Fact]
        public void ExportPublicKey_Test()
        {
            var keyPair = _sm2EncryptionService.GenerateSm2KeyPair();
            var publicKeyHex = keyPair.Public.ExportPublicKey();
            Assert.False(string.IsNullOrWhiteSpace(publicKeyHex));
        }

        [Fact]
        public void ExportPrivateKey_Test()
        {
            var keyPair = _sm2EncryptionService.GenerateSm2KeyPair();
            var privateKeyHex = keyPair.Private.ExportPrivateKey();
            Assert.False(string.IsNullOrWhiteSpace(privateKeyHex));
        }

        [Theory]
        [InlineData(SM2Engine.Mode.C1C2C3)]
        [InlineData(SM2Engine.Mode.C1C3C2)]
        public void Encrypt_Decrypt_Test(SM2Engine.Mode mode)
        {
            var keyPair = _sm2EncryptionService.GenerateSm2KeyPair();
            var plainText = Encoding.UTF8.GetBytes("Hello, SM2!");

            var publicKeyBytes = ((Org.BouncyCastle.Crypto.Parameters.ECPublicKeyParameters)keyPair.Public).Q.GetEncoded();
            var privateKeyBytes = ((Org.BouncyCastle.Crypto.Parameters.ECPrivateKeyParameters)keyPair.Private).D.ToByteArray();

            var cipherText = _sm2EncryptionService.Encrypt(publicKeyBytes, plainText, mode: mode);
            Assert.NotNull(cipherText);
            Assert.True(cipherText.Length > 0);

            var decryptedText = _sm2EncryptionService.Decrypt(privateKeyBytes, cipherText, mode: mode);
            Assert.NotNull(decryptedText);
            Assert.Equal(plainText, decryptedText);
        }

        [Theory]
        [InlineData(SM2Engine.Mode.C1C2C3)]
        [InlineData(SM2Engine.Mode.C1C3C2)]
        public void Encrypt_Decrypt_WithDifferentCurve_Test(SM2Engine.Mode mode)
        {
            var keyPair = _sm2EncryptionService.GenerateSm2KeyPair(Sm2EncryptionNames.CurveWapip192v1);
            var plainText = Encoding.UTF8.GetBytes("Hello, SM2 with different curve!");

            var publicKeyBytes = ((Org.BouncyCastle.Crypto.Parameters.ECPublicKeyParameters)keyPair.Public).Q.GetEncoded();
            var privateKeyBytes = ((Org.BouncyCastle.Crypto.Parameters.ECPrivateKeyParameters)keyPair.Private).D.ToByteArray();

            var cipherText = _sm2EncryptionService.Encrypt(publicKeyBytes, plainText, Sm2EncryptionNames.CurveWapip192v1, mode: mode);
            Assert.NotNull(cipherText);
            Assert.True(cipherText.Length > 0);

            var decryptedText = _sm2EncryptionService.Decrypt(privateKeyBytes, cipherText, Sm2EncryptionNames.CurveWapip192v1, mode: mode);
            Assert.NotNull(decryptedText);
            Assert.Equal(plainText, decryptedText);
        }

        [Fact]
        public void Sign_VerifySign_Test()
        {
            var keyPair = _sm2EncryptionService.GenerateSm2KeyPair();
            var data = Encoding.UTF8.GetBytes("Data to be signed.");

            var privateKeyBytes = ((Org.BouncyCastle.Crypto.Parameters.ECPrivateKeyParameters)keyPair.Private).D.ToByteArray();
            var publicKeyBytes = ((Org.BouncyCastle.Crypto.Parameters.ECPublicKeyParameters)keyPair.Public).Q.GetEncoded();

            var signature = _sm2EncryptionService.Sign(privateKeyBytes, data);
            Assert.NotNull(signature);
            Assert.True(signature.Length > 0);

            var isValid = _sm2EncryptionService.VerifySign(publicKeyBytes, data, signature);
            Assert.True(isValid);
        }

        [Fact]
        public void Sign_VerifySign_WithID_Test()
        {
            var keyPair = _sm2EncryptionService.GenerateSm2KeyPair();
            var data = Encoding.UTF8.GetBytes("Data to be signed with ID.");
            var id = Encoding.UTF8.GetBytes("Alice");

            var privateKeyBytes = ((Org.BouncyCastle.Crypto.Parameters.ECPrivateKeyParameters)keyPair.Private).D.ToByteArray();
            var publicKeyBytes = ((Org.BouncyCastle.Crypto.Parameters.ECPublicKeyParameters)keyPair.Public).Q.GetEncoded();

            var signature = _sm2EncryptionService.Sign(privateKeyBytes, data, id: id);
            Assert.NotNull(signature);
            Assert.True(signature.Length > 0);

            var isValid = _sm2EncryptionService.VerifySign(publicKeyBytes, data, signature, id: id);
            Assert.True(isValid);
        }

        [Fact]
        public void C123ToC132_C132ToC123_Test()
        {
            var keyPair = _sm2EncryptionService.GenerateSm2KeyPair();
            var plainText = Encoding.UTF8.GetBytes("Hello, SM2 conversion!");

            var publicKeyBytes = ((Org.BouncyCastle.Crypto.Parameters.ECPublicKeyParameters)keyPair.Public).Q.GetEncoded();
            var privateKeyBytes = ((Org.BouncyCastle.Crypto.Parameters.ECPrivateKeyParameters)keyPair.Private).D.ToByteArray();

            var c1c2c3CipherText = _sm2EncryptionService.Encrypt(publicKeyBytes, plainText, mode: SM2Engine.Mode.C1C2C3);
            var c1c3c2CipherText = _sm2EncryptionService.C123ToC132(c1c2c3CipherText);
            var convertedBackToC1c2c3 = _sm2EncryptionService.C132ToC123(c1c3c2CipherText);

            Assert.Equal(c1c2c3CipherText, convertedBackToC1c2c3);

            var decryptedFromC1c3c2 = _sm2EncryptionService.Decrypt(privateKeyBytes, c1c3c2CipherText, mode: SM2Engine.Mode.C1C3C2);
            Assert.Equal(plainText, decryptedFromC1c3c2);
        }
    }
}