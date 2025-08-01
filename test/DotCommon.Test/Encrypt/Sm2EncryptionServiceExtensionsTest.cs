using DotCommon.Crypto.SM2;
using Microsoft.Extensions.Options;
using Org.BouncyCastle.Crypto.Engines;
using System.Text;
using Xunit;

namespace DotCommon.Test.Encrypt
{
    public class Sm2EncryptionServiceExtensionsTest
    {
        private readonly Sm2EncryptionService _sm2EncryptionService;
        private readonly IOptions<DotCommonSm2EncryptionOptions> _options;

        public Sm2EncryptionServiceExtensionsTest()
        {
            _options = Options.Create(new DotCommonSm2EncryptionOptions { DefaultCurve = Sm2EncryptionNames.CurveSm2p256v1 });
            _sm2EncryptionService = new Sm2EncryptionService(_options);
        }

        [Theory]
        [InlineData(SM2Engine.Mode.C1C2C3)]
        [InlineData(SM2Engine.Mode.C1C3C2)]
        public void Encrypt_Decrypt_Extension_Test(SM2Engine.Mode mode)
        {
            var keyPair = _sm2EncryptionService.GenerateSm2KeyPair();
            var plainText = "Hello, SM2 Extensions!";

            var publicKeyHex = keyPair.Public.ExportPublicKey();
            var privateKeyHex = keyPair.Private.ExportPrivateKey();

            var cipherTextHex = _sm2EncryptionService.Encrypt(publicKeyHex, plainText, mode: mode);
            Assert.False(string.IsNullOrWhiteSpace(cipherTextHex));

            var decryptedText = _sm2EncryptionService.Decrypt(privateKeyHex, cipherTextHex, mode: mode);
            Assert.Equal(plainText, decryptedText);
        }

        [Fact]
        public void Sign_VerifySign_Extension_Test()
        {
            var keyPair = _sm2EncryptionService.GenerateSm2KeyPair();
            var data = "Data to be signed by extensions.";

            var privateKeyHex = keyPair.Private.ExportPrivateKey();
            var publicKeyHex = keyPair.Public.ExportPublicKey();

            var signatureHex = _sm2EncryptionService.Sign(privateKeyHex, data);
            Assert.False(string.IsNullOrWhiteSpace(signatureHex));

            var isValid = _sm2EncryptionService.VerifySign(publicKeyHex, data, signatureHex);
            Assert.True(isValid);
        }

        [Fact]
        public void Sign_VerifySign_Extension_WithID_Test()
        {
            var keyPair = _sm2EncryptionService.GenerateSm2KeyPair();
            var data = "Data to be signed by extensions with ID.";
            var id = Encoding.UTF8.GetBytes("Bob");

            var privateKeyHex = keyPair.Private.ExportPrivateKey();
            var publicKeyHex = keyPair.Public.ExportPublicKey();

            var signatureHex = _sm2EncryptionService.Sign(privateKeyHex, data, id: id);
            Assert.False(string.IsNullOrWhiteSpace(signatureHex));

            var isValid = _sm2EncryptionService.VerifySign(publicKeyHex, data, signatureHex, id: id);
            Assert.True(isValid);
        }
    }
}