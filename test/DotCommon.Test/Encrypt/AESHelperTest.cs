using DotCommon.Encrypt;
using System;
using System.Security.Cryptography;
using System.Text;
using Xunit;

namespace DotCommon.Test.Encrypt
{
    public class AESHelperTest
    {
        [Fact]
        public void GenerateKey_Should_Create_Valid_Key()
        {
            // Arrange & Act
            var key128 = AESHelper.GenerateKey(128);
            var key192 = AESHelper.GenerateKey(192);
            var key256 = AESHelper.GenerateKey(256);

            // Assert
            Assert.Equal(16, key128.Length); // 128 bits = 16 bytes
            Assert.Equal(24, key192.Length); // 192 bits = 24 bytes
            Assert.Equal(32, key256.Length); // 256 bits = 32 bytes
        }

        [Fact]
        public void GenerateKey_WithInvalidKeySize_Should_ThrowArgumentException()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentException>(() => AESHelper.GenerateKey(100));
        }

        [Fact]
        public void GenerateIV_Should_Create_Valid_IV()
        {
            // Arrange & Act
            var iv = AESHelper.GenerateIV();

            // Assert
            Assert.Equal(16, iv.Length); // AES block size is 128 bits = 16 bytes
        }

        [Fact]
        public void Encrypt_And_Decrypt_ByteArray_Should_WorkCorrectly()
        {
            // Arrange
            var plainText = "Hello, World!";
            var data = Encoding.UTF8.GetBytes(plainText);
            var key = AESHelper.GenerateKey();
            var iv = AESHelper.GenerateIV();

            // Act
            var encrypted = AESHelper.Encrypt(data, key, iv);
            var decrypted = AESHelper.Decrypt(encrypted, key, iv);
            var decryptedText = Encoding.UTF8.GetString(decrypted);

            // Assert
            Assert.NotEqual(data, encrypted); // Encrypted data should be different
            Assert.Equal(plainText, decryptedText); // Decrypted text should match original
        }

        [Fact]
        public void Encrypt_And_Decrypt_String_Should_WorkCorrectly()
        {
            // Arrange
            var plainText = "Hello, World!";
            var key = AESHelper.GenerateKey();
            var iv = AESHelper.GenerateIV();

            // Act
            var encrypted = AESHelper.Encrypt(plainText, key, iv);
            var decryptedText = AESHelper.DecryptToString(encrypted, key, iv);

            // Assert
            Assert.NotEqual(plainText, Convert.ToBase64String(encrypted)); // Encrypted data should be different
            Assert.Equal(plainText, decryptedText); // Decrypted text should match original
        }

        [Fact]
        public void Encrypt_WithNullData_Should_ThrowArgumentNullException()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentNullException>(() => AESHelper.Encrypt((byte[])null));
        }

        [Fact]
        public void Encrypt_WithNullString_Should_ThrowArgumentNullException()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentNullException>(() => AESHelper.Encrypt((string)null));
        }

        [Fact]
        public void Decrypt_WithNullData_Should_ThrowArgumentNullException()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentNullException>(() => AESHelper.Decrypt(null));
        }

        [Fact]
        public void DecryptToString_WithNullData_Should_ThrowArgumentNullException()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentNullException>(() => AESHelper.DecryptToString(null));
        }

        [Fact]
        public void Encrypt_WithInvalidKeySize_Should_ThrowArgumentException()
        {
            // Arrange
            var data = Encoding.UTF8.GetBytes("test");

            // Act & Assert
            Assert.Throws<ArgumentException>(() => AESHelper.Encrypt(data, keySize: 100));
        }

        [Fact]
        public void Decrypt_WithInvalidKeySize_Should_ThrowArgumentException()
        {
            // Arrange
            var data = Encoding.UTF8.GetBytes("test");
            var encrypted = AESHelper.Encrypt(data); // Encrypt with default key size first

            // Act & Assert
            Assert.Throws<ArgumentException>(() => AESHelper.Decrypt(encrypted, keySize: 100));
        }

        /// <summary>
        /// Compatibility test with previous implementation and third-party encryption results.
        /// </summary>
        [Fact]
        public void EncryptResult_Compatibility_Test()
        {
            // Arrange
            const string str = "helloworld";
            var strBytes = Encoding.UTF8.GetBytes(str);
            var keyBytes = Convert.FromBase64String("MTIzNDU2Nzg4NzY1NDMyMQ=="); // 1234567887654321
            var ivBytes = Convert.FromBase64String("MTIzNDU2Nzg5MGFiY2RlZg=="); // 1234567890abcdef

            // Act
            var encryptedBytes = AESHelper.Encrypt(
                strBytes,
                keyBytes,
                ivBytes,
                128,
                CipherMode.CBC,
                PaddingMode.PKCS7);
            var encryptedBase64 = Convert.ToBase64String(encryptedBytes);

            // Assert - This value should match the expected result from the previous implementation
            Assert.Equal("5vpVXOvT+drFQQSH3KXi6Q==", encryptedBase64);
        }
    }
}