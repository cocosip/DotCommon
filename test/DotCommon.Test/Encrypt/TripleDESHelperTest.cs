using DotCommon.Encrypt;
using System;
using System.Text;
using Xunit;

namespace DotCommon.Test.Encrypt
{
    public class TripleDESHelperTest
    {
        [Fact]
        public void GenerateKey_Should_Create_Valid_Key()
        {
            // Arrange & Act
            var key = TripleDESHelper.GenerateKey();

            // Assert
            Assert.Equal(24, key.Length); // TripleDES key size is 192 bits = 24 bytes
        }

        [Fact]
        public void GenerateIV_Should_Create_Valid_IV()
        {
            // Arrange & Act
            var iv = TripleDESHelper.GenerateIV();

            // Assert
            Assert.Equal(8, iv.Length); // TripleDES block size is 64 bits = 8 bytes
        }

        [Fact]
        public void Encrypt_And_Decrypt_ByteArray_Should_WorkCorrectly()
        {
            // Arrange
            var plainText = "Hello, World!";
            var data = Encoding.UTF8.GetBytes(plainText);
            var key = TripleDESHelper.GenerateKey();
            var iv = TripleDESHelper.GenerateIV();

            // Act
            var encrypted = TripleDESHelper.Encrypt(data, key, iv);
            var decrypted = TripleDESHelper.Decrypt(encrypted, key, iv);
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
            var key = TripleDESHelper.GenerateKey();
            var iv = TripleDESHelper.GenerateIV();

            // Act
            var encrypted = TripleDESHelper.Encrypt(plainText, key, iv);
            var decryptedText = TripleDESHelper.DecryptToString(encrypted, key, iv);

            // Assert
            Assert.NotEqual(plainText, Convert.ToBase64String(encrypted)); // Encrypted data should be different
            Assert.Equal(plainText, decryptedText); // Decrypted text should match original
        }

        [Fact]
        public void Encrypt_WithNullData_Should_ThrowArgumentNullException()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentNullException>(() => TripleDESHelper.Encrypt((byte[])null));
        }

        [Fact]
        public void Encrypt_WithNullString_Should_ThrowArgumentNullException()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentNullException>(() => TripleDESHelper.Encrypt((string)null));
        }

        [Fact]
        public void Decrypt_WithNullData_Should_ThrowArgumentNullException()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentNullException>(() => TripleDESHelper.Decrypt(null));
        }

        [Fact]
        public void DecryptToString_WithNullData_Should_ThrowArgumentNullException()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentNullException>(() => TripleDESHelper.DecryptToString(null));
        }

        [Fact]
        public void Encrypt_Decrypt_With_Default_Key_IV_Should_Work()
        {
            // Arrange
            const string source = "helloworld";
            var sourceBytes = Encoding.UTF8.GetBytes(source);

            // Act
            var encrypted = TripleDESHelper.Encrypt(sourceBytes);
            var decrypted = TripleDESHelper.Decrypt(encrypted);
            var decryptedText = Encoding.UTF8.GetString(decrypted);

            // Assert
            Assert.NotEqual(sourceBytes, encrypted); // Encrypted data should be different
            Assert.Equal(source, decryptedText); // Decrypted text should match original
        }

        [Fact]
        public void BackwardCompatibility_Test()
        {
            // Arrange
            const string source = "helloworld";
            var sourceBytes = Encoding.UTF8.GetBytes(source);
            const string keyBase64 = "MTIzNDU2Nzg4NzY1NDMyMQ=="; // 1234567887654321
            const string ivBase64 = "MTIzNDU2Nzg5MGFiY2RlZg=="; // 1234567890abcdef

            var keyBytes = Convert.FromBase64String(keyBase64);
            var ivBytes = Convert.FromBase64String(ivBase64);

            // Act
            var encrypted = TripleDESHelper.Encrypt(sourceBytes, keyBytes, ivBytes);
            var decrypted = TripleDESHelper.Decrypt(encrypted, keyBytes, ivBytes);
            var decryptedText = Encoding.UTF8.GetString(decrypted);

            // Assert
            Assert.Equal(source, decryptedText);
        }
    }
}