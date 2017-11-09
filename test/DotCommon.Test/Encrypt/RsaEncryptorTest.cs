using DotCommon.Encrypt;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DotCommon.Test.Encrypt
{
    /// <summary>RSA签名
    /// </summary>
    public class RsaEncryptorTest
    {
        private string PublicKey = @"MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQDNQvbAerQ0mtnJ2lytf/cBixoypElmj+btqJucP2i6/6wad5ZqpWR5FNFEBcAgbP6rN4vA83gMB6hNLpgxivCQsh7OBBcHxxJd6ncz+sp/DuaJfL4Jqdw/Cm9R3wIj9MfyFhEYlnoW3WzU7fsHTtQB8rK07qTc6z+0FspubH2gzwIDAQAB";
        private string PrivateKey = @"MIICXAIBAAKBgQDNQvbAerQ0mtnJ2lytf/cBixoypElmj+btqJucP2i6/6wad5ZqpWR5FNFEBcAgbP6rN4vA83gMB6hNLpgxivCQsh7OBBcHxxJd6ncz+sp/DuaJfL4Jqdw/Cm9R3wIj9MfyFhEYlnoW3WzU7fsHTtQB8rK07qTc6z+0FspubH2gzwIDAQABAoGAElNHKloKGqQ0i3PmUEsbZ1Te6uSF9RUl3dPuyJ1UoVVQLTC+ChloBfNY14TX6U0x02FrJbfW4OQSH4noZovr/CYr6XnBw57/Ej0AqtOJwOta/LSE4+05CgPf6bUdhqBsfkktRpKPSrBQ94sjqw4FwNgnhKxwASDZP6Ma3EsDyPkCQQDnq95Q4QYCs1MXoAWqNSJVIdtgQoWFCTpPdhnb4gry+OytZylgJ7DtqsnInLq33/DItOQ5/BcuejDhaCjAfeMVAkEA4tEb0VivyLdepskDKkYmHexnxxgLr7LOQur5OaG0uheIa51M+4YEHx0wBngmGbJEnzDAYz+nB3EXYppYqo09UwJBANWzFxaaC3ZQPjSLus3/x1SqL3dCxXErSutjcIUApMLt1Tw67dKxqiYBpbJ0yFO2saAiJGhMXoHT2uUBtJ2jQeUCQG858BRHE1ywX2AWrtCqOcLuzS1a41AztOYn6DOU0tV2+NUc/EVTwO2pGIXzoWt0eiY+d/mzmysREFCwM87fGDUCQFbG9yj9lfubm0+YvSBfeLzv/QOOWDxhUj74SOy4FaYRE/eYvVGZvbUmdngfUdWaGr9HMS/Ret+J8TvdV+LvLU0=";

        /// <summary>签名验证
        /// </summary>
        [Fact]
        public void SignDataTest()
        {
            var str = "你好,中国!";
            var rsaEncrypter = new RsaEncryptor(PrivateKey, PublicKey);
            var signed = rsaEncrypter.SignData(str);
            Assert.True(rsaEncrypter.VerifyData(str, signed));

            var rsa2Encrypter = new RsaEncryptor(PrivateKey, PublicKey);
            rsa2Encrypter.SetHashAlg("RSA2");
            var signed2 = rsa2Encrypter.SignData(str);
            Assert.True(rsa2Encrypter.VerifyData(str, signed2));

        }

        [Fact]
        public void EncryptTest()
        {
            var str = "HelloWorld!";
            var rsaEncrypter = new RsaEncryptor(PrivateKey, PublicKey);
            var encrypted = rsaEncrypter.Encrypt(str);
            Assert.Equal(str, rsaEncrypter.Decrypt(encrypted));

            var rsa2Encrypter = new RsaEncryptor(PrivateKey, PublicKey);
            rsa2Encrypter.SetHashAlg("RSA2");
            var encrypted2 = rsa2Encrypter.Encrypt(str);
            Assert.Equal(str, rsa2Encrypter.Decrypt(encrypted2));
        }

    }
}
