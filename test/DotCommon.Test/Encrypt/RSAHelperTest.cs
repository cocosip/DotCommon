using DotCommon.Encrypt;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Xunit;

namespace DotCommon.Test.Encrypt
{
    public class RSAHelperTest
    {
        [Fact]
        public void GenerateKeyPair_Test()
        {
            var keyPair = RSAHelper.GenerateKeyPair(RSAKeyFormat.PKCS1, 512);

            Assert.NotEmpty(keyPair.PublicKey);
            var encrypted1 = RSAHelper.EncryptAsBase64("hello", keyPair.PublicKey);
            var decrypted1 = RSAHelper.DecryptFromBase64(encrypted1, keyPair.PrivateKey);
            Assert.Equal("hello", decrypted1);

            var signed1 = RSAHelper.SignDataAsBase64("string1", keyPair.PrivateKey);
            Assert.True(RSAHelper.VerifyBase64Data("string1", signed1, keyPair.PublicKey));
        }

        [Fact]
        public void KeyPair_Equal_Test()
        {
            var keyPair = RSAHelper.GenerateKeyPair(RSAKeyFormat.PKCS8, 2048);
            var keyPair2 = new RSAKeyPair()
            {
                PrivateKey = keyPair.PrivateKey,
                PublicKey = keyPair.PublicKey
            };

            Assert.Equal(keyPair, keyPair2);
        }




        /// <summary>PKCS1与PKCS8密钥转换
        /// </summary>
        [Fact]
        public void PKCS8_PKCS1_Conver_Test()
        {
            var keyPair1 = RSAHelper.GenerateKeyPair(RSAKeyFormat.PKCS1);
            var pkcs8Key1 = RSAHelper.PKCS1ToPKCS8(keyPair1.PrivateKey);

            var encrypted1 = RSAHelper.EncryptAsBase64("123456", keyPair1.PublicKey);

            var decrypted1 = RSAHelper.DecryptFromBase64(encrypted1, keyPair1.PrivateKey);
            Assert.Equal("123456", decrypted1);

            var pkcs1Key = RSAHelper.PKCS8ToPKCS1(pkcs8Key1);
            Assert.Equal(keyPair1.PrivateKey, pkcs1Key);
        }


        [Fact]
        public void PKCS8KeyPair_Generate_Test()
        {
            var keyPair = RSAHelper.GenerateKeyPair(RSAKeyFormat.PKCS8);
            var pkcs1Key = RSAHelper.PKCS8ToPKCS1(keyPair.PrivateKey);
            var pkcs8Key = RSAHelper.PKCS1ToPKCS8(pkcs1Key);
            Assert.Equal(pkcs8Key, keyPair.PrivateKey);
        }



        [Fact]
        public void Encrypt_Decrypt_Test()
        {
            var privateKey1 = @"MIIEpAIBAAKCAQEAoiuclnTJCSvh/78lk9FvKTVGA1RbgJQ1jliwW1wNWSPkat/ENJup45wJxydlyksLo5V8LcieLw2XZPSxfR+TSLdZ40vTRtCSofrHazCH8lD2oCnVnE/oLJecw94n51WLCiVOWcgy6PV+M5l0EuuyQKfmkClNWCsaB6oH4PpRF5to3STa8p1hOrwzFnrosV2shpTiuH/NrUd80uFina95axuT6lFowGARafsLABPq4RQr7JN4jsKlEi9h9F5vGgCnsjKkOWgulwtrTxVfdaHh79IyGEBjd1nE/5aBTW4JiHbjYF8zBxgsAOj2yDL5kLDzInrni8iP4KbXCdSgEOw5lwIDAQABAoIBAHTNmpwXg5DShZWxtDwSddElaMcPJFSo6yYapPM4iviwTF3Zy7TkhWnFwpkpvDwG71wkV+uV+rEDRg7V21y2uFuAjAKVY5g2s9ZtDOMccl5KiEf1ucA7IYu0q5m70dviFiH5msQ52MlHwF0j4qGaF6pQBFXDGc0uRjJM+h3FzVCFXC0z0iPIPQ4IyjddY4YQRRIuKjDeyHcJlaxix6QM7s1zdhFwYy2nbdw4NEak+N2v/tjhjemGPlHH2f1UgN2POTw9L0VYaeHKK2T6eIUYpxnTc4yJgDK8ekVqv1M/UpooyRCN8oUXfs6KhYqQxElSarrX7zDcCGOjy8OiQ57S9pkCgYEA5qp0FjQ/HT7taMdeNT+YEUF+aZv59OezIUtzXXcsgzo6BmcomyBro80iBggk2ASJyFNaBhpMjsVOofffVNoO5dB+AE5wLmQjvS8cgGaJyzvRyitPVON6LP32C/71hJAl7FV9YrOj9lamAYmG49mnGN+gjuyYGV+N6uzOWXzcXJUCgYEAs/tLy0qzoAW7wLmeYU4IbWQaU70anSG+hqJdgXodh2fH0klszFsDzlzCXmEUw3AYaIdwXu4pQqLnGLeiE66FcVCVnaPesawtUL/rWR63yj3dzuN43Yb6dg/c8lZWKyPN5ZlcGCtadb9iREjkw/I28p8fbdbXxeCV+73B15EPRnsCgYEAwPeUCgdHTYYZQnCXBWDgLH9n653K6/Dx0Ea4ilZqbJXHeRfPxobGxc+USQuDFxwkz+u7AiP6K+4wLBubP/b0Q5m57zOvcX/gziGdbGVbnSO8C69TvKZYzU7gCYUPjAizURTrwiiaYDh4xAxzRiYNPifGwAp567Aen4vfzMlB6EkCgYAs1/sdjuJMh8EtFtKgefndj6iDnsVny0WWdBQKM6vx1ejSu1qxXCgJndEMBPJQc7iRKtXTwVFkegRMg0yNzNkQz7xHh9HvwO+VhAwWq3GtoVaHuuOdhokXK4KomtxjaUte3qQ0nXCvj9zjGKpvLAWG1CDXlEU0121nSPNoS4tbywKBgQDl9UQqmaWyxf0jQk9wQXIm/w5n2e0dUcpuWX2An3d1r+O/CJ7oaeFK2gk7cDJxq4CYebkCBnuxgje8NcwPMOffxLTJJ2FnTIQVdzEU6/2+9rhX9GUHniWA/1V+izEbMV/21gq8QdDHC3Cxznr0VxQIDpOMpnNWLloTWU9sfagRAg==";
            var publicKey1 = @"MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAoiuclnTJCSvh/78lk9FvKTVGA1RbgJQ1jliwW1wNWSPkat/ENJup45wJxydlyksLo5V8LcieLw2XZPSxfR+TSLdZ40vTRtCSofrHazCH8lD2oCnVnE/oLJecw94n51WLCiVOWcgy6PV+M5l0EuuyQKfmkClNWCsaB6oH4PpRF5to3STa8p1hOrwzFnrosV2shpTiuH/NrUd80uFina95axuT6lFowGARafsLABPq4RQr7JN4jsKlEi9h9F5vGgCnsjKkOWgulwtrTxVfdaHh79IyGEBjd1nE/5aBTW4JiHbjYF8zBxgsAOj2yDL5kLDzInrni8iP4KbXCdSgEOw5lwIDAQAB";

            var str = "HelloWorld!";
            var encrypted1 = RSAHelper.EncryptAsBase64(str, publicKey1, RSAEncryptionPadding.Pkcs1);
            var decrypted1 = RSAHelper.DecryptFromBase64(encrypted1, privateKey1, RSAEncryptionPadding.Pkcs1);
            Assert.Equal(str, decrypted1);

        }

        [Fact]
        public void Sign_VerifySign_Test()
        {
            var str = "你好,中国!";
            var privateKey1 = @"MIIEvgIBADANBgkqhkiG9w0BAQEFAASCBKgwggSkAgEAAoIBAQCiK5yWdMkJK+H/vyWT0W8pNUYDVFuAlDWOWLBbXA1ZI+Rq38Q0m6njnAnHJ2XKSwujlXwtyJ4vDZdk9LF9H5NIt1njS9NG0JKh+sdrMIfyUPagKdWcT+gsl5zD3ifnVYsKJU5ZyDLo9X4zmXQS67JAp+aQKU1YKxoHqgfg+lEXm2jdJNrynWE6vDMWeuixXayGlOK4f82tR3zS4WKdr3lrG5PqUWjAYBFp+wsAE+rhFCvsk3iOwqUSL2H0Xm8aAKeyMqQ5aC6XC2tPFV91oeHv0jIYQGN3WcT/loFNbgmIduNgXzMHGCwA6PbIMvmQsPMieueLyI/gptcJ1KAQ7DmXAgMBAAECggEAdM2anBeDkNKFlbG0PBJ10SVoxw8kVKjrJhqk8ziK+LBMXdnLtOSFacXCmSm8PAbvXCRX65X6sQNGDtXbXLa4W4CMApVjmDaz1m0M4xxyXkqIR/W5wDshi7SrmbvR2+IWIfmaxDnYyUfAXSPioZoXqlAEVcMZzS5GMkz6HcXNUIVcLTPSI8g9DgjKN11jhhBFEi4qMN7IdwmVrGLHpAzuzXN2EXBjLadt3Dg0RqT43a/+2OGN6YY+UcfZ/VSA3Y85PD0vRVhp4corZPp4hRinGdNzjImAMrx6RWq/Uz9SmijJEI3yhRd+zoqFipDESVJqutfvMNwIY6PLw6JDntL2mQKBgQDmqnQWND8dPu1ox141P5gRQX5pm/n057MhS3NddyyDOjoGZyibIGujzSIGCCTYBInIU1oGGkyOxU6h999U2g7l0H4ATnAuZCO9LxyAZonLO9HKK09U43os/fYL/vWEkCXsVX1is6P2VqYBiYbj2acY36CO7JgZX43q7M5ZfNxclQKBgQCz+0vLSrOgBbvAuZ5hTghtZBpTvRqdIb6Gol2Beh2HZ8fSSWzMWwPOXMJeYRTDcBhoh3Be7ilCoucYt6ITroVxUJWdo96xrC1Qv+tZHrfKPd3O43jdhvp2D9zyVlYrI83lmVwYK1p1v2JESOTD8jbynx9t1tfF4JX7vcHXkQ9GewKBgQDA95QKB0dNhhlCcJcFYOAsf2frncrr8PHQRriKVmpslcd5F8/GhsbFz5RJC4MXHCTP67sCI/or7jAsG5s/9vRDmbnvM69xf+DOIZ1sZVudI7wLr1O8pljNTuAJhQ+MCLNRFOvCKJpgOHjEDHNGJg0+J8bACnnrsB6fi9/MyUHoSQKBgCzX+x2O4kyHwS0W0qB5+d2PqIOexWfLRZZ0FAozq/HV6NK7WrFcKAmd0QwE8lBzuJEq1dPBUWR6BEyDTI3M2RDPvEeH0e/A75WEDBarca2hVoe6452GiRcrgqia3GNpS17epDSdcK+P3OMYqm8sBYbUINeURTTXbWdI82hLi1vLAoGBAOX1RCqZpbLF/SNCT3BBcib/DmfZ7R1Rym5ZfYCfd3Wv478Inuhp4UraCTtwMnGrgJh5uQIGe7GCN7w1zA8w59/EtMknYWdMhBV3MRTr/b72uFf0ZQeeJYD/VX6LMRsxX/bWCrxB0McLcLHOevRXFAgOk4ymc1YuWhNZT2x9qBEC";
            var publicKey1 = @"MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAoiuclnTJCSvh/78lk9FvKTVGA1RbgJQ1jliwW1wNWSPkat/ENJup45wJxydlyksLo5V8LcieLw2XZPSxfR+TSLdZ40vTRtCSofrHazCH8lD2oCnVnE/oLJecw94n51WLCiVOWcgy6PV+M5l0EuuyQKfmkClNWCsaB6oH4PpRF5to3STa8p1hOrwzFnrosV2shpTiuH/NrUd80uFina95axuT6lFowGARafsLABPq4RQr7JN4jsKlEi9h9F5vGgCnsjKkOWgulwtrTxVfdaHh79IyGEBjd1nE/5aBTW4JiHbjYF8zBxgsAOj2yDL5kLDzInrni8iP4KbXCdSgEOw5lwIDAQAB";

            var signed1 = Netstandard2RSAHelper.SignDataAsBase64(str, privateKey1, RSASignaturePadding.Pkcs1);

            Assert.True(Netstandard2RSAHelper.VerifyBase64Data(str, signed1, publicKey1));
        }

        [Fact]
        public void GetKeyFormat_Test()
        {
            var privateKey1 = @"MIIEvgIBADANBgkqhkiG9w0BAQEFAASCBKgwggSkAgEAAoIBAQCiK5yWdMkJK+H/vyWT0W8pNUYDVFuAlDWOWLBbXA1ZI+Rq38Q0m6njnAnHJ2XKSwujlXwtyJ4vDZdk9LF9H5NIt1njS9NG0JKh+sdrMIfyUPagKdWcT+gsl5zD3ifnVYsKJU5ZyDLo9X4zmXQS67JAp+aQKU1YKxoHqgfg+lEXm2jdJNrynWE6vDMWeuixXayGlOK4f82tR3zS4WKdr3lrG5PqUWjAYBFp+wsAE+rhFCvsk3iOwqUSL2H0Xm8aAKeyMqQ5aC6XC2tPFV91oeHv0jIYQGN3WcT/loFNbgmIduNgXzMHGCwA6PbIMvmQsPMieueLyI/gptcJ1KAQ7DmXAgMBAAECggEAdM2anBeDkNKFlbG0PBJ10SVoxw8kVKjrJhqk8ziK+LBMXdnLtOSFacXCmSm8PAbvXCRX65X6sQNGDtXbXLa4W4CMApVjmDaz1m0M4xxyXkqIR/W5wDshi7SrmbvR2+IWIfmaxDnYyUfAXSPioZoXqlAEVcMZzS5GMkz6HcXNUIVcLTPSI8g9DgjKN11jhhBFEi4qMN7IdwmVrGLHpAzuzXN2EXBjLadt3Dg0RqT43a/+2OGN6YY+UcfZ/VSA3Y85PD0vRVhp4corZPp4hRinGdNzjImAMrx6RWq/Uz9SmijJEI3yhRd+zoqFipDESVJqutfvMNwIY6PLw6JDntL2mQKBgQDmqnQWND8dPu1ox141P5gRQX5pm/n057MhS3NddyyDOjoGZyibIGujzSIGCCTYBInIU1oGGkyOxU6h999U2g7l0H4ATnAuZCO9LxyAZonLO9HKK09U43os/fYL/vWEkCXsVX1is6P2VqYBiYbj2acY36CO7JgZX43q7M5ZfNxclQKBgQCz+0vLSrOgBbvAuZ5hTghtZBpTvRqdIb6Gol2Beh2HZ8fSSWzMWwPOXMJeYRTDcBhoh3Be7ilCoucYt6ITroVxUJWdo96xrC1Qv+tZHrfKPd3O43jdhvp2D9zyVlYrI83lmVwYK1p1v2JESOTD8jbynx9t1tfF4JX7vcHXkQ9GewKBgQDA95QKB0dNhhlCcJcFYOAsf2frncrr8PHQRriKVmpslcd5F8/GhsbFz5RJC4MXHCTP67sCI/or7jAsG5s/9vRDmbnvM69xf+DOIZ1sZVudI7wLr1O8pljNTuAJhQ+MCLNRFOvCKJpgOHjEDHNGJg0+J8bACnnrsB6fi9/MyUHoSQKBgCzX+x2O4kyHwS0W0qB5+d2PqIOexWfLRZZ0FAozq/HV6NK7WrFcKAmd0QwE8lBzuJEq1dPBUWR6BEyDTI3M2RDPvEeH0e/A75WEDBarca2hVoe6452GiRcrgqia3GNpS17epDSdcK+P3OMYqm8sBYbUINeURTTXbWdI82hLi1vLAoGBAOX1RCqZpbLF/SNCT3BBcib/DmfZ7R1Rym5ZfYCfd3Wv478Inuhp4UraCTtwMnGrgJh5uQIGe7GCN7w1zA8w59/EtMknYWdMhBV3MRTr/b72uFf0ZQeeJYD/VX6LMRsxX/bWCrxB0McLcLHOevRXFAgOk4ymc1YuWhNZT2x9qBEC";

            var privateKey2 = @"MIIEpAIBAAKCAQEAoiuclnTJCSvh/78lk9FvKTVGA1RbgJQ1jliwW1wNWSPkat/ENJup45wJxydlyksLo5V8LcieLw2XZPSxfR+TSLdZ40vTRtCSofrHazCH8lD2oCnVnE/oLJecw94n51WLCiVOWcgy6PV+M5l0EuuyQKfmkClNWCsaB6oH4PpRF5to3STa8p1hOrwzFnrosV2shpTiuH/NrUd80uFina95axuT6lFowGARafsLABPq4RQr7JN4jsKlEi9h9F5vGgCnsjKkOWgulwtrTxVfdaHh79IyGEBjd1nE/5aBTW4JiHbjYF8zBxgsAOj2yDL5kLDzInrni8iP4KbXCdSgEOw5lwIDAQABAoIBAHTNmpwXg5DShZWxtDwSddElaMcPJFSo6yYapPM4iviwTF3Zy7TkhWnFwpkpvDwG71wkV+uV+rEDRg7V21y2uFuAjAKVY5g2s9ZtDOMccl5KiEf1ucA7IYu0q5m70dviFiH5msQ52MlHwF0j4qGaF6pQBFXDGc0uRjJM+h3FzVCFXC0z0iPIPQ4IyjddY4YQRRIuKjDeyHcJlaxix6QM7s1zdhFwYy2nbdw4NEak+N2v/tjhjemGPlHH2f1UgN2POTw9L0VYaeHKK2T6eIUYpxnTc4yJgDK8ekVqv1M/UpooyRCN8oUXfs6KhYqQxElSarrX7zDcCGOjy8OiQ57S9pkCgYEA5qp0FjQ/HT7taMdeNT+YEUF+aZv59OezIUtzXXcsgzo6BmcomyBro80iBggk2ASJyFNaBhpMjsVOofffVNoO5dB+AE5wLmQjvS8cgGaJyzvRyitPVON6LP32C/71hJAl7FV9YrOj9lamAYmG49mnGN+gjuyYGV+N6uzOWXzcXJUCgYEAs/tLy0qzoAW7wLmeYU4IbWQaU70anSG+hqJdgXodh2fH0klszFsDzlzCXmEUw3AYaIdwXu4pQqLnGLeiE66FcVCVnaPesawtUL/rWR63yj3dzuN43Yb6dg/c8lZWKyPN5ZlcGCtadb9iREjkw/I28p8fbdbXxeCV+73B15EPRnsCgYEAwPeUCgdHTYYZQnCXBWDgLH9n653K6/Dx0Ea4ilZqbJXHeRfPxobGxc+USQuDFxwkz+u7AiP6K+4wLBubP/b0Q5m57zOvcX/gziGdbGVbnSO8C69TvKZYzU7gCYUPjAizURTrwiiaYDh4xAxzRiYNPifGwAp567Aen4vfzMlB6EkCgYAs1/sdjuJMh8EtFtKgefndj6iDnsVny0WWdBQKM6vx1ejSu1qxXCgJndEMBPJQc7iRKtXTwVFkegRMg0yNzNkQz7xHh9HvwO+VhAwWq3GtoVaHuuOdhokXK4KomtxjaUte3qQ0nXCvj9zjGKpvLAWG1CDXlEU0121nSPNoS4tbywKBgQDl9UQqmaWyxf0jQk9wQXIm/w5n2e0dUcpuWX2An3d1r+O/CJ7oaeFK2gk7cDJxq4CYebkCBnuxgje8NcwPMOffxLTJJ2FnTIQVdzEU6/2+9rhX9GUHniWA/1V+izEbMV/21gq8QdDHC3Cxznr0VxQIDpOMpnNWLloTWU9sfagRAg==";

            var format1 = Netstandard2RSAHelper.GetKeyFormat(privateKey1);
            Assert.Equal(RSAKeyFormat.PKCS8, format1);

            var format2 = Netstandard2RSAHelper.GetKeyFormat(privateKey2);
            Assert.Equal(RSAKeyFormat.PKCS1, format2);

            //var format3 = RSAHelper.GetKeyFormat("xxxqqqq");
            //Assert.Equal(RSAKeyFormat.Unknow, format3);


        }
    }
}
