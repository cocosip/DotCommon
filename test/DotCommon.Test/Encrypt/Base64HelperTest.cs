using DotCommon.Encrypt;
using System;
using System.Text;
using Xunit;

namespace DotCommon.Test.Encrypt
{
    public class Base64HelperTest
    {
        [Fact]
        public void Base64Encode_String_Should_EncodeCorrectly()
        {
            // Arrange
            const string source = "helloworld";
            const string expected = "aGVsbG93b3JsZA==";

            // Act
            var result = Base64Helper.Base64Encode(source);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Base64Encode_String_WithEncoding_Should_EncodeCorrectly()
        {
            // Arrange
            const string source = "helloworld";
            const string encoding = "utf-8";
            const string expected = "aGVsbG93b3JsZA==";

            // Act
            var result = Base64Helper.Base64Encode(source, encoding);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Base64Encode_ByteArray_Should_EncodeCorrectly()
        {
            // Arrange
            var source = Encoding.UTF8.GetBytes("helloworld");
            const string expected = "aGVsbG93b3JsZA==";

            // Act
            var result = Base64Helper.Base64Encode(source);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Base64Decode_String_Should_DecodeCorrectly()
        {
            // Arrange
            const string source = "aGVsbG93b3JsZA==";
            const string expected = "helloworld";

            // Act
            var result = Base64Helper.Base64Decode(source);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Base64Decode_String_WithEncoding_Should_DecodeCorrectly()
        {
            // Arrange
            const string source = "aGVsbG93b3JsZA==";
            const string encoding = "utf-8";
            const string expected = "helloworld";

            // Act
            var result = Base64Helper.Base64Decode(source, encoding);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Base64Decode_ToBytes_Should_DecodeCorrectly()
        {
            // Arrange
            const string source = "aGVsbG93b3JsZA==";
            var expected = Encoding.UTF8.GetBytes("helloworld");

            // Act
            var result = Base64Helper.Base64DecodeToBytes(source);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Base64Encode_And_Decode_Should_PreserveOriginalData()
        {
            // Arrange
            const string original = "Hello, World! 你好世界！";

            // Act
            var encoded = Base64Helper.Base64Encode(original);
            var decoded = Base64Helper.Base64Decode(encoded);

            // Assert
            Assert.Equal(original, decoded);
        }

        [Fact]
        public void Base64Encode_NullString_Should_ThrowArgumentNullException()
        {
            // Arrange
            string data = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => Base64Helper.Base64Encode(data));
        }

        [Fact]
        public void Base64Encode_NullByteArray_Should_ThrowArgumentNullException()
        {
            // Arrange
            byte[] data = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => Base64Helper.Base64Encode(data));
        }

        [Fact]
        public void Base64Encode_EmptyEncoding_Should_ThrowArgumentException()
        {
            // Arrange
            const string data = "test";
            const string encoding = "";

            // Act & Assert
            Assert.Throws<ArgumentException>(() => Base64Helper.Base64Encode(data, encoding));
        }

        [Fact]
        public void Base64Decode_NullString_Should_ThrowArgumentNullException()
        {
            // Arrange
            string data = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => Base64Helper.Base64Decode(data));
        }

        [Fact]
        public void Base64Decode_InvalidBase64String_Should_ThrowFormatException()
        {
            // Arrange
            const string invalidBase64 = "invalid base64!";

            // Act & Assert
            Assert.Throws<FormatException>(() => Base64Helper.Base64Decode(invalidBase64));
        }

        [Fact]
        public void Base64DecodeToBytes_NullString_Should_ThrowArgumentNullException()
        {
            // Arrange
            string data = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => Base64Helper.Base64DecodeToBytes(data));
        }

        [Fact]
        public void Base64DecodeToBytes_InvalidBase64String_Should_ThrowFormatException()
        {
            // Arrange
            const string invalidBase64 = "invalid base64!";

            // Act & Assert
            Assert.Throws<FormatException>(() => Base64Helper.Base64DecodeToBytes(invalidBase64));
        }

        [Fact]
        public void FormatAsDataUri_Should_FormatCorrectly()
        {
            // Arrange
            const string base64 = "YnJvdGhlcg==";
            const string mimeType = "image/jpg";
            const string expected = "data:image/jpg;base64,YnJvdGhlcg==";

            // Act
            var result = Base64Helper.FormatAsDataUri(base64, mimeType);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void FormatAsDataUri_WithDefaultMimeType_Should_FormatCorrectly()
        {
            // Arrange
            const string base64 = "YnJvdGhlcg==";
            const string expected = "data:image/jpg;base64,YnJvdGhlcg==";

            // Act
            var result = Base64Helper.FormatAsDataUri(base64);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void FormatAsDataUri_NullBase64_Should_ThrowArgumentNullException()
        {
            // Arrange
            string base64 = null;
            const string mimeType = "image/png";

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => Base64Helper.FormatAsDataUri(base64, mimeType));
        }

        [Fact]
        public void FormatAsDataUri_EmptyMimeType_Should_ThrowArgumentException()
        {
            // Arrange
            const string base64 = "YnJvdGhlcg==";
            const string mimeType = "";

            // Act & Assert
            Assert.Throws<ArgumentException>(() => Base64Helper.FormatAsDataUri(base64, mimeType));
        }

        [Fact]
        public void BackwardCompatibility_Test()
        {
            // Arrange
            const string source1 = "helloworld";
            const string source2 = "abc1234";
            const string encrypted2 = "YWJjMTIzNA==";
            const string encrypted3 = "YnJvdGhlcg==";
            const string source3 = "brother";

            // Act
            var encrypted1 = Base64Helper.Base64Encode(source1, "utf-8");
            var decrypted1 = Base64Helper.Base64Decode(encrypted1, "utf-8");
            var imageBase64 = Base64Helper.FormatAsDataUri(encrypted3, "image/jpg");

            // Assert
            Assert.Equal(source1, decrypted1);
            Assert.Equal(encrypted2, Base64Helper.Base64Encode(source2));
            Assert.Equal(source3, Base64Helper.Base64Decode(encrypted3));
            Assert.Equal("data:image/jpg;base64,YnJvdGhlcg==", imageBase64);
        }
    }
}