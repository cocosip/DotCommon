using DotCommon.Crypto.SM2;
using Microsoft.Extensions.Options;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Utilities.Encoders;
using System.Text;
using Xunit;

namespace DotCommon.Test.Encrypt
{
    public class Sm2EncryptionServiceTest
    {
        private readonly Sm2EncryptionService _sm2EncryptionService;
        private readonly IOptions<DotCommonSm2EncryptionOptions> _options;

        public Sm2EncryptionServiceTest()
        {
            _options = Options.Create(new DotCommonSm2EncryptionOptions { DefaultCurve = Sm2EncryptionNames.CurveSm2p256v1 });
            _sm2EncryptionService = new Sm2EncryptionService(_options);
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