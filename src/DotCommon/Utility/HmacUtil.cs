using System;
using System.Security.Cryptography;
using System.Text;

namespace DotCommon.Utility
{
    /// <summary>HMAC算法工具类
    /// </summary>
    public static class HMACUtil
    {

        #region HMAC-Sha1算法

        /// <summary>获取HMAC-SHA1加密后的Base64值
        /// </summary>
        /// <param name="source">字符串</param>
        /// <param name="key">密钥字符串</param>
        /// <param name="sourceEncode">编码</param>
        /// <param name="keyEncode">密钥编码</param>
        /// <returns>Base64编码后的字符串</returns>
        public static string GetBase64StringHMACSHA1(string source, string key = "12345678", string sourceEncode = "utf-8", string keyEncode = "utf-8")
        {
            var hashBytes = GetHMACSHA1(Encoding.GetEncoding(sourceEncode).GetBytes(source), Encoding.GetEncoding(keyEncode).GetBytes(key));
            return Convert.ToBase64String(hashBytes);
        }


        /// <summary>获取HMAC-SHA1加密后的十六进制值
        /// </summary>
        /// <param name="source">字符串</param>
        /// <param name="key">密钥字符串</param>
        /// <param name="sourceEncode">编码</param>
        /// <param name="keyEncode">密钥编码</param>
        /// <returns>转换成16进制后的字符串</returns>
        public static string GetHex16StringHMACSHA1(string source, string key = "12345678", string sourceEncode = "utf-8", string keyEncode = "utf-8")
        {
            var hashBytes = GetHMACSHA1(Encoding.GetEncoding(sourceEncode).GetBytes(source), Encoding.GetEncoding(keyEncode).GetBytes(key));
            return ByteBufferUtil.ByteBufferToHex16(hashBytes);
        }


        /// <summary>HMAC-SHA1加密
        /// </summary>
        /// <param name="sourceBytes">数据二进制</param>
        /// <param name="keyBytes">密钥二进制</param>
        /// <returns></returns>
        public static byte[] GetHMACSHA1(byte[] sourceBytes, byte[] keyBytes)
        {
            using (var hmacSha1 = new HMACSHA1(keyBytes))
            {
                var hashBytes = hmacSha1.ComputeHash(sourceBytes);
                return hashBytes;
            }
        }

        #endregion


        #region  HMAC-SHA256算法

        /// <summary>获取HMAC-SHA256加密后的Base64值
        /// </summary>
        /// <param name="source">字符串</param>
        /// <param name="key">密钥字符串</param>
        /// <param name="sourceEncode">编码</param>
        /// <param name="keyEncode">密钥编码</param>
        /// <returns>Base64编码后的字符串</returns>
        public static string GetBase64StringHMACSHA256(string source, string key = "12345678", string sourceEncode = "utf-8", string keyEncode = "utf-8")
        {
            var hashBytes = GetHMACSHA256(Encoding.GetEncoding(sourceEncode).GetBytes(source), Encoding.GetEncoding(keyEncode).GetBytes(key));
            return Convert.ToBase64String(hashBytes);
        }


        /// <summary>获取HMAC-SHA256加密后的十六进制值
        /// </summary>
        /// <param name="source">字符串</param>
        /// <param name="key">密钥字符串</param>
        /// <param name="sourceEncode">编码</param>
        /// <param name="keyEncode">密钥编码</param>
        /// <returns>转换成16进制后的字符串</returns>
        public static string GetHex16StringHMACSHA256(string source, string key = "12345678", string sourceEncode = "utf-8", string keyEncode = "utf-8")
        {
            var hashBytes = GetHMACSHA256(Encoding.GetEncoding(sourceEncode).GetBytes(source), Encoding.GetEncoding(keyEncode).GetBytes(key));
            return ByteBufferUtil.ByteBufferToHex16(hashBytes);
        }

        /// <summary>HMAC-SHA256加密
        /// </summary>
        /// <param name="sourceBytes">数据二进制</param>
        /// <param name="keyBytes">密钥二进制</param>
        /// <returns></returns>
        public static byte[] GetHMACSHA256(byte[] sourceBytes, byte[] keyBytes)
        {
            using (var hmacSha256 = new HMACSHA256(keyBytes))
            {
                var hashBytes = hmacSha256.ComputeHash(sourceBytes);
                return hashBytes;
            }
        }
        #endregion


        #region  HMAC-MD5算法

        /// <summary>获取HMAC-MD5加密后的Base64值
        /// </summary>
        /// <param name="source">字符串</param>
        /// <param name="key">密钥字符串</param>
        /// <param name="sourceEncode">编码</param>
        /// <param name="keyEncode">密钥编码</param>
        /// <returns>Base64编码后的字符串</returns>
        public static string GetBase64StringHMACMD5(string source, string key = "12345678", string sourceEncode = "utf-8", string keyEncode = "utf-8")
        {
            var hashBytes = GetHMACMD5(Encoding.GetEncoding(sourceEncode).GetBytes(source), Encoding.GetEncoding(keyEncode).GetBytes(key));
            return Convert.ToBase64String(hashBytes);
        }


        /// <summary>获取HMAC-MD5加密后的十六进制值
        /// </summary>
        /// <param name="source">字符串</param>
        /// <param name="key">密钥字符串</param>
        /// <param name="sourceEncode">编码</param>
        /// <param name="keyEncode">密钥编码</param>
        /// <returns>转换成16进制后的字符串</returns>
        public static string GetHex16StringHMACMD5(string source, string key = "12345678", string sourceEncode = "utf-8", string keyEncode = "utf-8")
        {
            var hashBytes = GetHMACMD5(Encoding.GetEncoding(sourceEncode).GetBytes(source), Encoding.GetEncoding(keyEncode).GetBytes(key));
            return ByteBufferUtil.ByteBufferToHex16(hashBytes);
        }

        /// <summary>HMAC-MD5加密
        /// </summary>
        /// <param name="sourceBytes">数据二进制</param>
        /// <param name="keyBytes">密钥二进制</param>
        /// <returns></returns>
        public static byte[] GetHMACMD5(byte[] sourceBytes, byte[] keyBytes)
        {
            using (var hmacMd5 = new HMACMD5(keyBytes))
            {
                var hashBytes = hmacMd5.ComputeHash(sourceBytes);
                return hashBytes;
            }
        }
        #endregion
    }
}
