using DotCommon.Encrypt;
using System.Security.Cryptography;
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
            var keyPair2 = new RSAKeyPair(keyPair.PublicKey, keyPair.PrivateKey);

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
            var decrypted1 = RSAHelper.DecryptFromBase64(encrypted1, privateKey1, RSAKeyFormat.PKCS1, RSAEncryptionPadding.Pkcs1);
            Assert.Equal(str, decrypted1);
        }

        [Fact]
        public void Sign_VerifySign_Test()
        {
            var str = "你好,中国!";
            var privateKey1 = @"MIIEpAIBAAKCAQEAoiuclnTJCSvh/78lk9FvKTVGA1RbgJQ1jliwW1wNWSPkat/ENJup45wJxydlyksLo5V8LcieLw2XZPSxfR+TSLdZ40vTRtCSofrHazCH8lD2oCnVnE/oLJecw94n51WLCiVOWcgy6PV+M5l0EuuyQKfmkClNWCsaB6oH4PpRF5to3STa8p1hOrwzFnrosV2shpTiuH/NrUd80uFina95axuT6lFowGARafsLABPq4RQr7JN4jsKlEi9h9F5vGgCnsjKkOWgulwtrTxVfdaHh79IyGEBjd1nE/5aBTW4JiHbjYF8zBxgsAOj2yDL5kLDzInrni8iP4KbXCdSgEOw5lwIDAQABAoIBAHTNmpwXg5DShZWxtDwSddElaMcPJFSo6yYapPM4iviwTF3Zy7TkhWnFwpkpvDwG71wkV+uV+rEDRg7V21y2uFuAjAKVY5g2s9ZtDOMccl5KiEf1ucA7IYu0q5m70dviFiH5msQ52MlHwF0j4qGaF6pQBFXDGc0uRjJM+h3FzVCFXC0z0iPIPQ4IyjddY4YQRRIuKjDeyHcJlaxix6QM7s1zdhFwYy2nbdw4NEak+N2v/tjhjemGPlHH2f1UgN2POTw9L0VYaeHKK2T6eIUYpxnTc4yJgDK8ekVqv1M/UpooyRCN8oUXfs6KhYqQxElSarrX7zDcCGOjy8OiQ57S9pkCgYEA5qp0FjQ/HT7taMdeNT+YEUF+aZv59OezIUtzXXcsgzo6BmcomyBro80iBggk2ASJyFNaBhpMjsVOofffVNoO5dB+AE5wLmQjvS8cgGaJyzvRyitPVON6LP32C/71hJAl7FV9YrOj9lamAYmG49mnGN+gjuyYGV+N6uzOWXzcXJUCgYEAs/tLy0qzoAW7wLmeYU4IbWQaU70anSG+hqJdgXodh2fH0klszFsDzlzCXmEUw3AYaIdwXu4pQqLnGLeiE66FcVCVnaPesawtUL/rWR63yj3dzuN43Yb6dg/c8lZWKyPN5ZlcGCtadb9iREjkw/I28p8fbdbXxeCV+73B15EPRnsCgYEAwPeUCgdHTYYZQnCXBWDgLH9n653K6/Dx0Ea4ilZqbJXHeRfPxobGxc+USQuDFxwkz+u7AiP6K+4wLBubP/b0Q5m57zOvcX/gziGdbGVbnSO8C69TvKZYzU7gCYUPjAizURTrwiiaYDh4xAxzRiYNPifGwAp567Aen4vfzMlB6EkCgYAs1/sdjuJMh8EtFtKgefndj6iDnsVny0WWdBQKM6vx1ejSu1qxXCgJndEMBPJQc7iRKtXTwVFkegRMg0yNzNkQz7xHh9HvwO+VhAwWq3GtoVaHuuOdhokXK4KomtxjaUte3qQ0nXCvj9zjGKpvLAWG1CDXlEU0121nSPNoS4tbywKBgQDl9UQqmaWyxf0jQk9wQXIm/w5n2e0dUcpuWX2An3d1r+O/CJ7oaeFK2gk7cDJxq4CYebkCBnuxgje8NcwPMOffxLTJJ2FnTIQVdzEU6/2+9rhX9GUHniWA/1V+izEbMV/21gq8QdDHC3Cxznr0VxQIDpOMpnNWLloTWU9sfagRAg==";
            var publicKey1 = @"MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAoiuclnTJCSvh/78lk9FvKTVGA1RbgJQ1jliwW1wNWSPkat/ENJup45wJxydlyksLo5V8LcieLw2XZPSxfR+TSLdZ40vTRtCSofrHazCH8lD2oCnVnE/oLJecw94n51WLCiVOWcgy6PV+M5l0EuuyQKfmkClNWCsaB6oH4PpRF5to3STa8p1hOrwzFnrosV2shpTiuH/NrUd80uFina95axuT6lFowGARafsLABPq4RQr7JN4jsKlEi9h9F5vGgCnsjKkOWgulwtrTxVfdaHh79IyGEBjd1nE/5aBTW4JiHbjYF8zBxgsAOj2yDL5kLDzInrni8iP4KbXCdSgEOw5lwIDAQAB";

            var signed1 = RSAHelper.SignDataAsBase64(str, privateKey1, RSAKeyFormat.PKCS1, RSASignaturePadding.Pkcs1);

            Assert.True(RSAHelper.VerifyBase64Data(str, signed1, publicKey1));
        }
    }
}