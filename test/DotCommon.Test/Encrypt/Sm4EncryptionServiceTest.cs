using DotCommon.Crypto.SM4;
using Microsoft.Extensions.Options;
using Org.BouncyCastle.Utilities.Encoders;
using System;
using System.Text;
using Xunit;

namespace DotCommon.Test.Encrypt
{
    public class Sm4EncryptionServiceTest
    {
        private readonly Sm4EncryptionService _sm4EncryptionService;
        private readonly DotCommonSm4EncryptionOptions _options;

        // Default key and IV for testing (16 bytes each)
        private static readonly byte[] TestKey = Hex.DecodeStrict("0123456789abcdeffedcba9876543210");
        private static readonly byte[] TestIv = Hex.DecodeStrict("fedcba98765432100123456789abcdef");

        public Sm4EncryptionServiceTest()
        {
            _options = new DotCommonSm4EncryptionOptions
            {
                DefaultIv = TestIv,
                DefaultMode = Sm4EncryptionNames.ModeCBC,
                DefaultPadding = Sm4EncryptionNames.PKCS7Padding
            };
            _sm4EncryptionService = new Sm4EncryptionService(Options.Create(_options));
        }

        [Theory]
        [InlineData(Sm4EncryptionNames.ModeECB, Sm4EncryptionNames.PKCS7Padding)]
        [InlineData(Sm4EncryptionNames.ModeECB, Sm4EncryptionNames.NoPadding)]
        [InlineData(Sm4EncryptionNames.ModeCBC, Sm4EncryptionNames.PKCS7Padding)]
        [InlineData(Sm4EncryptionNames.ModeCBC, Sm4EncryptionNames.NoPadding)]
        public void Encrypt_Decrypt_Bytes_Test(string mode, string padding)
        {
            var plainText = Encoding.UTF8.GetBytes("Hello, SM4 Encryption!");

            // Adjust plainText length for NoPadding in both ECB and CBC modes
            if (padding == Sm4EncryptionNames.NoPadding)
            {
                // Ensure plainText is a multiple of 16 bytes for NoPadding
                var remainder = plainText.Length % 16;
                if (remainder != 0)
                {
                    Array.Resize(ref plainText, plainText.Length + (16 - remainder));
                }
            }

            var cipherText = _sm4EncryptionService.Encrypt(plainText, TestKey, TestIv, mode, padding);
            Assert.NotNull(cipherText);
            Assert.True(cipherText.Length > 0);

            var decryptedText = _sm4EncryptionService.Decrypt(cipherText, TestKey, TestIv, mode, padding);
            Assert.NotNull(decryptedText);

            // For NoPadding, the decrypted text might contain trailing zeros if padded for encryption
            if (padding == Sm4EncryptionNames.NoPadding)
            {
                // Compare only the original length of the plaintext
                Assert.Equal(Encoding.UTF8.GetString(plainText, 0, plainText.Length), Encoding.UTF8.GetString(decryptedText, 0, plainText.Length));
            }
            else
            {
                Assert.Equal(plainText, decryptedText);
            }
        }

        [Theory]
        [InlineData(Sm4EncryptionNames.ModeECB, Sm4EncryptionNames.PKCS7Padding)]
        [InlineData(Sm4EncryptionNames.ModeECB, Sm4EncryptionNames.NoPadding)]
        [InlineData(Sm4EncryptionNames.ModeCBC, Sm4EncryptionNames.PKCS7Padding)]
        [InlineData(Sm4EncryptionNames.ModeCBC, Sm4EncryptionNames.NoPadding)]
        public void Encrypt_Decrypt_String_Extension_Test(string mode, string padding)
        {
            var plainText = "Hello, SM4 Extensions!";

            // Adjust plainText length for NoPadding in both ECB and CBC modes
            if (padding == Sm4EncryptionNames.NoPadding)
            {
                // Ensure plainText is a multiple of 16 bytes for NoPadding
                var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
                var remainder = plainTextBytes.Length % 16;
                if (remainder != 0)
                {
                    plainText += new string('\0', 16 - remainder); // Pad with null characters
                }
            }

            var cipherTextHex = _sm4EncryptionService.Encrypt(plainText, Hex.ToHexString(TestKey), Hex.ToHexString(TestIv), mode, padding);
            Assert.False(string.IsNullOrWhiteSpace(cipherTextHex));

            var decryptedText = _sm4EncryptionService.Decrypt(cipherTextHex, Hex.ToHexString(TestKey), Hex.ToHexString(TestIv), mode, padding);
            Assert.NotNull(decryptedText);

            // For NoPadding, the decrypted text might contain trailing null characters if padded for encryption
            if (padding == Sm4EncryptionNames.NoPadding)
            {
                Assert.StartsWith(plainText.TrimEnd('\0'), decryptedText);
            }
            else
            {
                Assert.Equal(plainText, decryptedText);
            }
        }

        [Fact]
        public void Encrypt_InvalidKeyLength_ThrowsArgumentException()
        {
            var plainText = Encoding.UTF8.GetBytes("test");
            var invalidKey = new byte[10]; // Invalid length
            Assert.Throws<ArgumentException>(() => _sm4EncryptionService.Encrypt(plainText, invalidKey, TestIv));
        }

        [Fact]
        public void Decrypt_InvalidKeyLength_ThrowsArgumentException()
        {
            var cipherText = Encoding.UTF8.GetBytes("test");
            var invalidKey = new byte[10]; // Invalid length
            Assert.Throws<ArgumentException>(() => _sm4EncryptionService.Decrypt(cipherText, invalidKey, TestIv));
        }

        [Fact]
        public void Encrypt_CBC_InvalidIvLength_ThrowsArgumentException()
        {
            var plainText = Encoding.UTF8.GetBytes("test");
            var invalidIv = new byte[10]; // Invalid length
            Assert.Throws<ArgumentException>(() => _sm4EncryptionService.Encrypt(plainText, TestKey, invalidIv, Sm4EncryptionNames.ModeCBC));
        }

        [Fact]
        public void Decrypt_CBC_InvalidIvLength_ThrowsArgumentException()
        {
            var cipherText = Encoding.UTF8.GetBytes("test");
            var invalidIv = new byte[10]; // Invalid length
            Assert.Throws<ArgumentException>(() => _sm4EncryptionService.Decrypt(cipherText, TestKey, invalidIv, Sm4EncryptionNames.ModeCBC));
        }

        [Fact]
        public void Encrypt_NullPlainText_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _sm4EncryptionService.Encrypt(null, TestKey, TestIv));
        }

        [Fact]
        public void Decrypt_NullCipherText_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _sm4EncryptionService.Decrypt(null, TestKey, TestIv));
        }

        [Fact]
        public void Encrypt_NullKey_ThrowsArgumentNullException()
        {
            var plainText = Encoding.UTF8.GetBytes("test");
            Assert.Throws<ArgumentNullException>(() => _sm4EncryptionService.Encrypt(plainText, null, TestIv));
        }

        [Fact]
        public void Decrypt_NullKey_ThrowsArgumentNullException()
        {
            var cipherText = Encoding.UTF8.GetBytes("test");
            Assert.Throws<ArgumentNullException>(() => _sm4EncryptionService.Decrypt(cipherText, null, TestIv));
        }
    }
}