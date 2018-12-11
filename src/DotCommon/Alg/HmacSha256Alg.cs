using DotCommon.Utility;
using System;
using System.Security.Cryptography;
using System.Text;

namespace DotCommon.Alg
{
    /// <summary>HMAC-SHA256加密
    /// </summary>
    public class HmacSha256Alg
    {
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

    }
}
