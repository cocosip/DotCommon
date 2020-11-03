#if NETSTANDARD2_1
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace DotCommon.Encrypt
{
    /// <summary>
    /// 基于Netstandard2.1平台下的RSA操作
    /// </summary>
    public class Netstandard21RSAHelper
    {

        private static readonly Dictionary<string, HashAlgorithmName> HashAlgorithmNameDict = new Dictionary<string, HashAlgorithmName>()
        {
            {"MD5",HashAlgorithmName.MD5 },
            {"SHA1",HashAlgorithmName.SHA1 },
            {"SHA256",HashAlgorithmName.SHA256 },
            {"SHA384",HashAlgorithmName.SHA384 },
            {"SHA512",HashAlgorithmName.SHA512 },
        };

        /// <summary>生成RSA密钥对(Pem密钥格式)
        /// </summary>
        /// <param name="format">格式,PKCS1或者PKCS8</param>
        /// <param name="keySize">512,1024,1536,2048</param>
        /// <returns>公钥,私钥</returns>
        public static RSAKeyPair GenerateKeyPair(RSAKeyFormat format = RSAKeyFormat.PKCS1, int keySize = 1024)
        {
            using (var rsa = RSA.Create())
            {
                rsa.KeySize = keySize;

                var privateKeyBuffer = format == RSAKeyFormat.PKCS1 ? rsa.ExportRSAPrivateKey() : rsa.ExportPkcs8PrivateKey();
                var privateKey = Convert.ToBase64String(privateKeyBuffer);
                //该版本下不正确
                //var publicKeyBuffer = rsa.ExportRSAPublicKey();
                var publicKeyBuffer = rsa.ExportSubjectPublicKeyInfo();
                var publicKey = Convert.ToBase64String(publicKeyBuffer);
                return new RSAKeyPair(publicKey, privateKey);
            }
        }


        /// <summary>
        /// 将PKCS8编码格式的私钥转换成PKCS1编码格式的私钥
        /// </summary>
        /// <param name="pkcs8Key">PKCS8格式密钥</param>
        /// <returns></returns>
        public static string PKCS8ToPKCS1(string pkcs8Key)
        {
            using (var rsa = RSA.Create())
            {
                var pkcs8KeyBuffer = Convert.FromBase64String(pkcs8Key);
                rsa.ImportPkcs8PrivateKey(pkcs8KeyBuffer, out int bytesRead);
                var pkcs1KeyBuffer = rsa.ExportRSAPrivateKey();
                return Convert.ToBase64String(pkcs1KeyBuffer);
            }
        }

        /// <summary>
        /// 将PKCS1编码格式的私钥转换成PKCS8编码格式的私钥
        /// </summary>
        /// <param name="pkcs1Key">PKCS1格式密钥</param>
        /// <returns></returns>
        public static string PKCS1ToPKCS8(string pkcs1Key)
        {
            using (var rsa = RSA.Create())
            {
                var pkcs1KeyBuffer = Convert.FromBase64String(pkcs1Key);
                rsa.ImportRSAPrivateKey(pkcs1KeyBuffer, out int bytesRead);
                var pkcs8KeyBuffer = rsa.ExportPkcs8PrivateKey();
                return Convert.ToBase64String(pkcs8KeyBuffer);
            }
        }

        #region RSA加密解密,签名解签

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="data">原数据</param>
        /// <param name="publicKey">公钥</param>
        /// <param name="encryptionPadding">算法</param>
        /// <returns></returns>
        public static byte[] Encrypt(byte[] data, string publicKey, RSAEncryptionPadding encryptionPadding = null)
        {
            using (var rsa = RSA.Create())
            {
                var publicKeyBuffer = Convert.FromBase64String(publicKey);
                //该版本下不正确
                //rsa.ImportRSAPublicKey(publicKeyBuffer, out int bytesRead);
                rsa.ImportSubjectPublicKeyInfo(publicKeyBuffer, out int bytesRead);

                encryptionPadding ??= RSAEncryptionPadding.Pkcs1;
                var encryptedData = rsa.Encrypt(data, encryptionPadding);
                return encryptedData;
            }
        }

        /// <summary>加密
        /// </summary>
        /// <param name="data">原数据</param>
        /// <param name="publicKey">公钥</param>
        /// <param name="encryptionPadding">算法</param>
        /// <param name="encode">编码</param>
        /// <returns></returns>
        public static string EncryptAsBase64(string data, string publicKey, RSAEncryptionPadding encryptionPadding = null, string encode = "utf-8")
        {
            var encryptedData = Encrypt(Encoding.GetEncoding(encode).GetBytes(data), publicKey, encryptionPadding);
            return Convert.ToBase64String(encryptedData);
        }


        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="data">待解密数据</param>
        /// <param name="privateKey"></param>
        /// <param name="encryptionPadding">算法</param>
        /// <returns></returns>
        public static byte[] Decrypt(byte[] data, string privateKey, RSAEncryptionPadding encryptionPadding = null)
        {
            using (var rsa = RSA.Create())
            {
                var privateKeyBuffer = Convert.FromBase64String(privateKey);
                rsa.ImportRSAPrivateKey(privateKeyBuffer, out int bytesRead);
               
                encryptionPadding ??= RSAEncryptionPadding.Pkcs1;
                var decryptedData = rsa.Decrypt(data, encryptionPadding);
                return decryptedData;
            }
        }


        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="data">原数据</param>
        /// <param name="privateKey">私钥</param>
        /// <param name="encryptionPadding">算法</param>
        /// <param name="encode">编码</param>
        /// <returns></returns>
        public static string DecryptFromBase64(string data, string privateKey, RSAEncryptionPadding encryptionPadding = null, string encode = "utf-8")
        {
            var decryptedData = Decrypt(Convert.FromBase64String(data), privateKey, encryptionPadding);
            return Encoding.GetEncoding(encode).GetString(decryptedData);
        }


        /// <summary>
        /// 数据签名
        /// </summary>
        /// <param name="data">待签名数据</param>
        /// <param name="privateKey">私钥</param>
        /// <param name="signaturePadding">签名类型</param>
        /// <param name="hashAlgorithmName">哈希算法</param>
        /// <returns></returns>

        public static byte[] SignData(byte[] data, string privateKey, RSASignaturePadding signaturePadding = null, string hashAlgorithmName = "SHA256")
        {
            using (var rsa = RSA.Create())
            {
                var privateKeyBuffer = Convert.FromBase64String(privateKey);
                rsa.ImportRSAPrivateKey(privateKeyBuffer, out int bytesRead);

                signaturePadding ??= RSASignaturePadding.Pkcs1;
                var signedData = rsa.SignData(data, HashAlgorithmNameDict[hashAlgorithmName], signaturePadding);
                return signedData;
            }
        }
        /// <summary>
        /// 数据签名
        /// </summary>
        /// <param name="data">待签名数据</param>
        /// <param name="privateKey">私钥</param>
        /// <param name="signaturePadding">签名类型</param>
        /// <param name="hashAlgorithmName">哈希算法</param>
        /// <param name="encode">编码</param>
        /// <returns></returns>
        public static string SignDataAsBase64(string data, string privateKey, RSASignaturePadding signaturePadding = null, string hashAlgorithmName = "SHA256", string encode = "utf-8")
        {
            var signedData = SignData(Encoding.GetEncoding(encode).GetBytes(data), privateKey, signaturePadding, hashAlgorithmName);
            return Convert.ToBase64String(signedData);
        }

        /// <summary>
        /// 签名校验
        /// </summary>
        /// <param name="data">原数据</param>
        /// <param name="signature">签名后数据</param>
        /// <param name="publicKey">公寓奥</param>
        /// <param name="signaturePadding">签名算法</param>
        /// <param name="hashAlgorithmName">哈希算法</param>
        /// <returns></returns>
        public static bool VerifyData(byte[] data, byte[] signature, string publicKey, RSASignaturePadding signaturePadding = null, string hashAlgorithmName = "SHA256")
        {
            using (var rsa = RSA.Create())
            {
                var publicKeyBuffer = Convert.FromBase64String(publicKey);
                //该版本下不正确
                //rsa.ImportRSAPublicKey(publicKeyBuffer, out int bytesRead);
                rsa.ImportSubjectPublicKeyInfo(publicKeyBuffer, out int bytesRead);

                signaturePadding ??= RSASignaturePadding.Pkcs1;
                return rsa.VerifyData(data, signature, HashAlgorithmNameDict[hashAlgorithmName], signaturePadding);
            }
        }

        /// <summary>
        /// 签名校验
        /// </summary>
        /// <param name="data">源数据</param>
        /// <param name="base64Signature">签名后数据Base64编码</param>
        /// <param name="publicKey">公寓奥</param>
        /// <param name="signaturePadding">签名算法</param>
        /// <param name="hashAlgorithmName">哈希算法</param>
        /// <param name="encode">编码</param>
        /// <returns></returns>
        public static bool VerifyBase64Data(string data, string base64Signature, string publicKey, RSASignaturePadding signaturePadding = null, string hashAlgorithmName = "SHA256", string encode = "utf-8")
        {
            var dataBytes = Encoding.GetEncoding(encode).GetBytes(data);
            var signature = Convert.FromBase64String(base64Signature);
            return VerifyData(dataBytes, signature, publicKey, signaturePadding, hashAlgorithmName);
        }
        #endregion

    }
}
#endif