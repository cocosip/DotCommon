using DotCommon.Crypto.SM3;
using System.Text;
using Xunit;

namespace DotCommon.Test.Encrypt
{
    public class Sm3EncryptionServiceTest
    {
        private readonly Sm3EncryptionService _sm3EncryptionService;

        public Sm3EncryptionServiceTest()
        {
            _sm3EncryptionService = new Sm3EncryptionService();
        }

        [Fact]
        public void GetHash_Bytes_Test()
        {
            var plainText = Encoding.UTF8.GetBytes("abc");
            var hashBytes = _sm3EncryptionService.GetHash(plainText);
            var actualHash = Org.BouncyCastle.Utilities.Encoders.Hex.ToHexString(hashBytes);

            // Assert that the hash is not null or empty and has the correct length (64 characters for 256-bit hash in hex)
            Assert.False(string.IsNullOrEmpty(actualHash));
            Assert.Equal(64, actualHash.Length);
        }

        [Fact]
        public void GetHash_String_Test()
        {
            var plainText = "abc";
            var actualHash = _sm3EncryptionService.GetHash(plainText);

            // Assert that the hash is not null or empty and has the correct length (64 characters for 256-bit hash in hex)
            Assert.False(string.IsNullOrEmpty(actualHash));
            Assert.Equal(64, actualHash.Length);
        }

        [Fact]
        public void GetHash_EmptyString_Test()
        {
            var plainText = "";
            var actualHash = _sm3EncryptionService.GetHash(plainText);

            // Assert that the hash is not null or empty and has the correct length (64 characters for 256-bit hash in hex)
            Assert.False(string.IsNullOrEmpty(actualHash));
            Assert.Equal(64, actualHash.Length);
        }
    }
}