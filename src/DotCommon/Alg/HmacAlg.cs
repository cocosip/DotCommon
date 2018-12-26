using DotCommon.Utility;
using System;
using System.Security.Cryptography;
using System.Text;

namespace DotCommon.Alg
{
    /// <summary>HMAC算法
    /// </summary>
    public class HmacAlg
    {

        #region HMAC-Sha1算法

        /// <summary>获取HMAC-SHA1加密后的Base64值
        /// </summary>
        public static string GetStringBase64HmacSha1(string source, string key = "12345678", string sourceEncode = "utf-8", string keyEncode = "utf-8")
        {
            var hashBytes = GetHmacSha1(Encoding.GetEncoding(sourceEncode).GetBytes(source), Encoding.GetEncoding(keyEncode).GetBytes(key));
            return Convert.ToBase64String(hashBytes);
        }


        /// <summary>获取HMAC-SHA1加密后的十六进制值
        /// </summary>
        public static string GetStringHmacSha1(string source, string key = "12345678", string sourceEncode = "utf-8", string keyEncode = "utf-8")
        {
            var hashBytes = GetHmacSha1(Encoding.GetEncoding(sourceEncode).GetBytes(source), Encoding.GetEncoding(keyEncode).GetBytes(key));
            return ByteBufferUtil.ByteArrayToString(hashBytes);
        }


        /// <summary>获取HMAC-SHA1加密后的
        /// </summary>
        public static byte[] GetHmacSha1(byte[] sourceBytes, byte[] keyBytes)
        {
            using(var hmacSha1 = new HMACSHA1(keyBytes))
            {
                var hashBytes = hmacSha1.ComputeHash(sourceBytes);
                return hashBytes;
            }
        }

        #endregion


        #region  HMAC-SHA256算法

        /// <summary>获取HMAC-SHA256加密后的Base64值
        /// </summary>
        public static string GetStringBase64HmacSha256(string source, string key = "12345678", string sourceEncode = "utf-8", string keyEncode = "utf-8")
        {
            var hashBytes = GetHmacSha256(Encoding.GetEncoding(sourceEncode).GetBytes(source), Encoding.GetEncoding(keyEncode).GetBytes(key));
            return Convert.ToBase64String(hashBytes);
        }


        /// <summary>获取HMAC-SHA256加密后的十六进制值
        /// </summary>
        public static string GetStringHmacSha256(string source, string key = "12345678", string sourceEncode = "utf-8", string keyEncode = "utf-8")
        {
            var hashBytes = GetHmacSha256(Encoding.GetEncoding(sourceEncode).GetBytes(source), Encoding.GetEncoding(keyEncode).GetBytes(key));
            return ByteBufferUtil.ByteArrayToString(hashBytes);
        }

        /// <summary>获取HMAC-SHA256加密后的
        /// </summary>
        public static byte[] GetHmacSha256(byte[] sourceBytes, byte[] keyBytes)
        {
            using(var hmacSha256 = new HMACSHA256(keyBytes))
            {
                var hashBytes = hmacSha256.ComputeHash(sourceBytes);
                return hashBytes;
            }
        }
        #endregion


        #region  HMAC-MD5算法

        /// <summary>获取HMAC-MD5加密后的Base64值
        /// </summary>
        public static string GetStringBase64HmacMd5(string source, string key = "12345678", string sourceEncode = "utf-8", string keyEncode = "utf-8")
        {
            var hashBytes = GetHmacMd5(Encoding.GetEncoding(sourceEncode).GetBytes(source), Encoding.GetEncoding(keyEncode).GetBytes(key));
            return Convert.ToBase64String(hashBytes);
        }


        /// <summary>获取HMAC-MD5加密后的十六进制值
        /// </summary>
        public static string GetStringHmacMd5(string source, string key = "12345678", string sourceEncode = "utf-8", string keyEncode = "utf-8")
        {
            var hashBytes = GetHmacMd5(Encoding.GetEncoding(sourceEncode).GetBytes(source), Encoding.GetEncoding(keyEncode).GetBytes(key));
            return ByteBufferUtil.ByteArrayToString(hashBytes);
        }

        /// <summary>获取HMAC-MD5加密
        /// </summary>
        public static byte[] GetHmacMd5(byte[] sourceBytes, byte[] keyBytes)
        {
            using(var hmacMd5 = new HMACMD5(keyBytes))
            {
                var hashBytes = hmacMd5.ComputeHash(sourceBytes);
                return hashBytes;
            }
        }
        #endregion
    }
}
