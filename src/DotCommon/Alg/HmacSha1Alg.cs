using DotCommon.Utility;
using System;
using System.Security.Cryptography;
using System.Text;

namespace DotCommon.Alg
{
    /// <summary>HMAC-SHA1加密
    /// </summary>
    public class HmacSha1Alg
    {

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
    }
}
