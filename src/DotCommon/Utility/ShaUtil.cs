using System;
using System.Security.Cryptography;
using System.Text;

namespace DotCommon.Utility
{
    /// <summary>SHA1算法
    /// </summary>
    public static class ShaUtil
    {
        /// <summary>获取十六进制字符串的Sha1 Hash
        /// </summary>
        /// <param name="source">字符串</param>
        /// <param name="encode">编码</param>
        /// <returns></returns>
        public static string GetHex16StringSha1Hash(string source, string encode = "utf-8")
        {
            var sourceBytes = Encoding.GetEncoding(encode).GetBytes(source);
            var hashBytes = GetSha1Hash(sourceBytes);
            return ByteBufferUtil.ByteBufferToHex16(hashBytes);
        }

        /// <summary>获取字符串的Sha1-Hash Base64值
        /// </summary>
        /// <param name="source">字符串</param>
        /// <param name="encode">编码</param>
        /// <returns></returns>
        public static string GetBase64StringSha1Hash(string source, string encode = "utf-8")
        {
            var sourceBytes = Encoding.GetEncoding(encode).GetBytes(source);
            var hashBytes = GetSha1Hash(sourceBytes);
            return Convert.ToBase64String(hashBytes);
        }


        /// <summary>获取二进制的Sha1值
        /// </summary>
        /// <param name="sourceBuffer">二进制数据</param>
        /// <returns></returns>
        public static byte[] GetSha1Hash(byte[] sourceBuffer)
        {
            using (var sha1 = SHA1.Create())
            {
                var hashBytes = sha1.ComputeHash(sourceBuffer);
                return hashBytes;
            }
        }

        /// <summary>获取十六进制字符串的Sha256 Hash
        /// </summary>
        /// <param name="source">字符串</param>
        /// <param name="encode">编码</param>
        /// <returns></returns>
        public static string GetHex16StringSha256Hash(string source, string encode = "utf-8")
        {
            var sourceBytes = Encoding.GetEncoding(encode).GetBytes(source);
            var hashBytes = GetSha256Hash(sourceBytes);
            return ByteBufferUtil.ByteBufferToHex16(hashBytes);
        }

        /// <summary>获取字符串的Sha256-Hash Base64值
        /// </summary>
        /// <param name="source">字符串</param>
        /// <param name="encode">编码</param>
        /// <returns></returns>
        public static string GetBase64StringSha256Hash(string source, string encode = "utf-8")
        {
            var sourceBytes = Encoding.GetEncoding(encode).GetBytes(source);
            var hashBytes = GetSha256Hash(sourceBytes);
            return Convert.ToBase64String(hashBytes);
        }


        /// <summary>获取二进制的Sha256值
        /// </summary>
        /// <param name="sourceBuffer">二进制数据</param>
        /// <returns></returns>
        public static byte[] GetSha256Hash(byte[] sourceBuffer)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashBytes = sha256.ComputeHash(sourceBuffer);
                return hashBytes;
            }
        }
    }
}
