#if NETSTANDARD2_1

using System;
using System.Security.Cryptography;
using System.Text;

namespace DotCommon.Encrypt
{
    /// <summary>
    /// RSA工具类
    /// </summary>
    public static class RSAHelper
    {
        /// <summary>
        /// 生成密钥对
        /// </summary>
        /// <param name="format"></param>
        /// <param name="keySize"></param>
        /// <param name="subjectPublicKey"></param>
        /// <returns></returns>
        public static RSAKeyPair GenerateKeyPair(RSAKeyFormat format = RSAKeyFormat.PKCS1, int keySize = 2048, bool subjectPublicKey = true)
        {
            using var rsa = RSA.Create();
            rsa.KeySize = keySize;

            var privateKeyBuffer = format == RSAKeyFormat.PKCS1 ? rsa.ExportRSAPrivateKey() : rsa.ExportPkcs8PrivateKey();
            var privateKey = Convert.ToBase64String(privateKeyBuffer);

            var publicKeyBuffer = subjectPublicKey ? rsa.ExportSubjectPublicKeyInfo() : rsa.ExportRSAPublicKey();
            var publicKey = Convert.ToBase64String(publicKeyBuffer);
            return new RSAKeyPair(publicKey, privateKey);
        }


        /// <summary>
        /// 将PKCS8编码格式的私钥转换成PKCS1编码格式的私钥
        /// </summary>
        /// <param name="pkcs8Key">PKCS8格式密钥</param>
        /// <returns></returns>
        public static string PKCS8ToPKCS1(string pkcs8Key)
        {
            using var rsa = RSA.Create();
            var pkcs8KeyBuffer = Convert.FromBase64String(pkcs8Key);
            rsa.ImportPkcs8PrivateKey(pkcs8KeyBuffer, out int bytesRead);
            var pkcs1KeyBuffer = rsa.ExportRSAPrivateKey();
            return Convert.ToBase64String(pkcs1KeyBuffer);
        }

        /// <summary>
        /// 将PKCS1编码格式的私钥转换成PKCS8编码格式的私钥
        /// </summary>
        /// <param name="pkcs1Key">PKCS1格式密钥</param>
        /// <returns></returns>
        public static string PKCS1ToPKCS8(string pkcs1Key)
        {
            using var rsa = RSA.Create();
            var pkcs1KeyBuffer = Convert.FromBase64String(pkcs1Key);
            rsa.ImportRSAPrivateKey(pkcs1KeyBuffer, out int bytesRead);
            var pkcs8KeyBuffer = rsa.ExportPkcs8PrivateKey();
            return Convert.ToBase64String(pkcs8KeyBuffer);
        }


        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="data">原数据</param>
        /// <param name="publicKey">公钥</param>
        /// <param name="encryptionPadding">算法</param>
        /// <param name="subjectPublicKey">subjectPublicKey</param>
        /// <returns></returns>
        public static byte[] Encrypt(byte[] data, string publicKey, RSAEncryptionPadding? encryptionPadding = null, bool subjectPublicKey = true)
        {
            using var rsa = RSA.Create();
            var publicKeyBuffer = Convert.FromBase64String(publicKey);

            if (subjectPublicKey)
            {
                rsa.ImportSubjectPublicKeyInfo(publicKeyBuffer, out int _);
            }
            else
            {
                rsa.ImportRSAPublicKey(publicKeyBuffer, out int _);
            }

            encryptionPadding ??= RSAEncryptionPadding.Pkcs1;
            var encryptedData = rsa.Encrypt(data, encryptionPadding);
            return encryptedData;
        }

        /// <summary>加密
        /// </summary>
        /// <param name="data">原数据</param>
        /// <param name="publicKey">公钥</param>
        /// <param name="encryptionPadding">算法</param>
        /// <param name="encode">编码</param>
        /// <param name="subjectPublicKey">subjectPublicKey</param>
        /// <returns></returns>
        public static string EncryptAsBase64(string data, string publicKey, RSAEncryptionPadding? encryptionPadding = null, string encode = "utf-8", bool subjectPublicKey = true)
        {
            var buffer = Encoding.GetEncoding(encode).GetBytes(data);
            var encryptedData = Encrypt(buffer, publicKey, encryptionPadding, subjectPublicKey);
            return Convert.ToBase64String(encryptedData);
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="data">待解密数据</param>
        /// <param name="privateKey">私钥</param>
        /// <param name="keyFormat">密钥格式</param>
        /// <param name="encryptionPadding">算法</param>
        /// <returns></returns>
        public static byte[] Decrypt(byte[] data, string privateKey, RSAKeyFormat keyFormat = RSAKeyFormat.PKCS1, RSAEncryptionPadding? encryptionPadding = null)
        {
            using var rsa = RSA.Create();
            var privateKeyBuffer = Convert.FromBase64String(privateKey);
            if (keyFormat == RSAKeyFormat.PKCS1)
            {
                rsa.ImportRSAPrivateKey(privateKeyBuffer, out int _);
            }
            else
            {
                rsa.ImportPkcs8PrivateKey(privateKeyBuffer, out int _);
            }
            encryptionPadding ??= RSAEncryptionPadding.Pkcs1;
            var decryptedData = rsa.Decrypt(data, encryptionPadding);
            return decryptedData;
        }


        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="data">原数据</param>
        /// <param name="privateKey">私钥</param>
        /// <param name="keyFormat">密钥格式</param>
        /// <param name="encryptionPadding">算法</param>
        /// <param name="encode">编码</param>
        /// <returns></returns>
        public static string DecryptFromBase64(string data, string privateKey, RSAKeyFormat keyFormat = RSAKeyFormat.PKCS1, RSAEncryptionPadding? encryptionPadding = null, string encode = "utf-8")
        {
            var decryptedData = Decrypt(Convert.FromBase64String(data), privateKey, keyFormat, encryptionPadding);
            return Encoding.GetEncoding(encode).GetString(decryptedData);
        }


        /// <summary>
        /// 数据签名
        /// </summary>
        /// <param name="data">待签名数据</param>
        /// <param name="privateKey">私钥</param>
        /// <param name="keyFormat">格式,默认PKCS1</param>
        /// <param name="signaturePadding">签名类型</param>
        /// <param name="hashAlgorithmName">哈希算法</param>
        /// <returns></returns>
        public static byte[] SignData(byte[] data, string privateKey, RSAKeyFormat keyFormat = RSAKeyFormat.PKCS1, RSASignaturePadding? signaturePadding = null, HashAlgorithmName hashAlgorithmName = default)
        {
            using var rsa = RSA.Create();
            var privateKeyBuffer = Convert.FromBase64String(privateKey);
            if (keyFormat == RSAKeyFormat.PKCS1)
            {
                rsa.ImportRSAPrivateKey(privateKeyBuffer, out int _);
            }
            else
            {
                rsa.ImportPkcs8PrivateKey(privateKeyBuffer, out int _);
            }

            signaturePadding ??= RSASignaturePadding.Pkcs1;
            if (hashAlgorithmName == null || hashAlgorithmName == default)
            {
                hashAlgorithmName = HashAlgorithmName.SHA256;
            }
            var signedData = rsa.SignData(data, hashAlgorithmName, signaturePadding);
            return signedData;
        }

        /// <summary>
        /// 数据签名
        /// </summary>
        /// <param name="data">待签名数据</param>
        /// <param name="privateKey">私钥</param>
        /// <param name="keyFormat">格式,默认PKCS1</param>
        /// <param name="signaturePadding">签名类型</param>
        /// <param name="hashAlgorithmName">哈希算法</param>
        /// <param name="encode">编码</param>
        /// <returns></returns>
        public static string SignDataAsBase64(string data, string privateKey, RSAKeyFormat keyFormat = RSAKeyFormat.PKCS1, RSASignaturePadding? signaturePadding = null, HashAlgorithmName hashAlgorithmName = default, string encode = "utf-8")
        {
            var buffer = Encoding.GetEncoding(encode).GetBytes(data);
            var signedData = SignData(buffer, privateKey, keyFormat, signaturePadding, hashAlgorithmName);
            return Convert.ToBase64String(signedData);
        }

        /// <summary>
        /// 签名校验
        /// </summary>
        /// <param name="data">原数据</param>
        /// <param name="signature">签名后数据</param>
        /// <param name="publicKey">公钥</param>
        /// <param name="subjectPublicKey">subjectPublicKey</param>
        /// <param name="signaturePadding">签名算法</param>
        /// <param name="hashAlgorithmName">哈希算法</param>
        /// <returns></returns>
        public static bool VerifyData(byte[] data, byte[] signature, string publicKey, bool subjectPublicKey = true, RSASignaturePadding? signaturePadding = null, HashAlgorithmName hashAlgorithmName = default)
        {
            using var rsa = RSA.Create();
            var publicKeyBuffer = Convert.FromBase64String(publicKey);

            if (subjectPublicKey)
            {
                rsa.ImportSubjectPublicKeyInfo(publicKeyBuffer, out int _);
            }
            else
            {
                rsa.ImportRSAPublicKey(publicKeyBuffer, out int _);
            }

            if (hashAlgorithmName == null || hashAlgorithmName == default)
            {
                hashAlgorithmName = HashAlgorithmName.SHA256;
            }

            signaturePadding ??= RSASignaturePadding.Pkcs1;
            return rsa.VerifyData(data, signature, hashAlgorithmName, signaturePadding);
        }

        /// <summary>
        /// 签名校验
        /// </summary>
        /// <param name="data">源数据</param>
        /// <param name="base64Signature">签名后数据Base64编码</param>
        /// <param name="publicKey">公钥</param>
        /// <param name="subjectPublicKey">subjectPublicKey</param>
        /// <param name="signaturePadding">签名算法</param>
        /// <param name="hashAlgorithmName">哈希算法</param>
        /// <param name="encode">编码</param>
        /// <returns></returns>
        public static bool VerifyBase64Data(string data, string base64Signature, string publicKey, bool subjectPublicKey = true, RSASignaturePadding? signaturePadding = null, HashAlgorithmName hashAlgorithmName = default, string encode = "utf-8")
        {
            var buffer = Encoding.GetEncoding(encode).GetBytes(data);
            var signature = Convert.FromBase64String(base64Signature);
            return VerifyData(buffer, signature, publicKey, subjectPublicKey, signaturePadding, hashAlgorithmName);
        }
    }
}

#endif