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

        private string RSA2PublicKey = @"MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAjOeAPWmVTuGFg/QPgrB1XzmqbfiRRcvR+WtFaP2Ul3ndlp7x751xTX40YWH+TnDxBXfF+uo0lPyIQ6toAcMpp/G2ctDupyycEzujYAZZbRDpwlE41psLcL+Vi+Ng1ORD22RSPpRz3Du4L3zX9ntee0NYyYYX4zJ7e3fZT4YBARBHT7TmP6wr8PgV+pb4ihfoZ27LrXoa1z9nMLFvJ/I7cNkgRHYSeTMMhTP3h1sh/Pso/MTY63oOjufOZcNyS4jYDbY+Uv48nNkRsK/FtKnjqZyPKmq3xWtKNy5jOhJjhh9b2hb48BLRjbX7V2IQKa9rjCXj99GSlfNdjhVWeWUclwIDAQAB";
        private string RSA2PrivateKey = @"MIIEowIBAAKCAQEAjOeAPWmVTuGFg/QPgrB1XzmqbfiRRcvR+WtFaP2Ul3ndlp7x751xTX40YWH+TnDxBXfF+uo0lPyIQ6toAcMpp/G2ctDupyycEzujYAZZbRDpwlE41psLcL+Vi+Ng1ORD22RSPpRz3Du4L3zX9ntee0NYyYYX4zJ7e3fZT4YBARBHT7TmP6wr8PgV+pb4ihfoZ27LrXoa1z9nMLFvJ/I7cNkgRHYSeTMMhTP3h1sh/Pso/MTY63oOjufOZcNyS4jYDbY+Uv48nNkRsK/FtKnjqZyPKmq3xWtKNy5jOhJjhh9b2hb48BLRjbX7V2IQKa9rjCXj99GSlfNdjhVWeWUclwIDAQABAoIBAFaQt0mDf1ZJ2SQbIhhRXpqVK+6KAn4V3TdVvvvkppB1LzylA9AJMx2/xmB5uqnoWzrXvcsMbieGChVAzhIfG41xQ3zAfY45Kt3qCtIotHH8LRDTo469DEdFfJPHqqrAXiwAM0L9Iz0Pd3W9RlTIsGAcHQUaG7zaO+C73ccsdZt3vSpxVWIbuaX3G6xD6ZXnoOyxtYfs3qpop+wjc9RoIzN+Pqd29jaLYcsN7D72q81gu8k6wpqxMC24O/wy/AhI4fSaYif7XQBAjjOUe93jrxBHcV1Wy0KOSaSHwoQ1nLL5KBVp6RqYb3TGaAKQE3rgpIrJg/x1X3p93nxmt6JnPnkCgYEA6rRB+NIKtOb9S7udL9zXrCV7MnQk7adRkVNAG8OszfADzH5U1HQ11GpnuS/JZu2DeL2lwmzfsk4ef1AcpjHb/iorr9UeGabM1Zp97Ebte9d+LeNdmuNS9DM0l8/R2SGKQ+EFaIGwg5pVuF8NlB8S0CEnabf/IRlj18q27/AKIfUCgYEAmbByZpzVUU+5Z5cB76DTXXbkaEmJhrCUJqWtzhsRy5tc/BimIW4JEzYvM1fWOqGEqhxz/Wo57SttMxsTReCI3lsFtCLhTnxECAZiRuBR4XJpcnVyKonSHWWfUtNiUN95eSkhV4jrcA4BSMTXPzTftxYJFZ5KMaaxte5eMMO30NsCgYB2IYhbDo0pBGJVLfct0gATu0HI4UB9BYw+kyJfVxuxA69FzAgybtNxOKVARlceoUldCkdWFqp4+mzLM61X0RyjTuJyO9hMnPHYSUw8Em8RuCLgQeIpRWXJV8SO7KD4orMO+0FXmn8XniSrCdyxwvobG7TUtzGInVjtkjCFj9HpyQKBgGAE5iSH3Zp0dcBrjvEYiJV/P0qMjxiQX68ZmdIIBYEwqtJxz/FY3uCa3Lh2K0jsOodRSYJNCK3NkOb6BnuEwd4x/glCNYOkjZh57JKdeWqh4ZF6IP7EpnppUDYeDPG7/Reeg889ouKaTWEaYeSCczbe1IQmJfKJU8P3je9niAM7AoGBANhr1tkbmmMN/LzAOYOYxWZeSBHUDvfBIhLGIPtA83uYuHieRIzfcLfUochgJfTbXHfXDgkIrH2Zlfun6XTMobWUoDTK4buaoB9xzXf2XB6u1w9BbuDM/Z4Nkz4vJHwmgXLJ1WnJxz94cocJytatXWiRvErJVNgof3ykb5xkzGov";


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
            Assert.Equal(signed, signed2);

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

        /// <summary>签名测试
        /// </summary>
        [Fact]
        public void RSA2SignDataTest()
        {
            var str = "你好,中国!";
            var rsaEncrypter = new RsaEncryptor(RSA2PrivateKey, RSA2PublicKey);
            rsaEncrypter.SetHashAlg("RSA2");
            var signed = rsaEncrypter.SignData(str);
            Assert.True(rsaEncrypter.VerifyData(str, signed));

            var rsa2Encrypter = new RsaEncryptor(RSA2PrivateKey, RSA2PublicKey);
            rsa2Encrypter.SetHashAlg("RSA2");
            var signed2 = rsa2Encrypter.SignData(str);
            Assert.True(rsa2Encrypter.VerifyData(str, signed2));

        }
    }
}
