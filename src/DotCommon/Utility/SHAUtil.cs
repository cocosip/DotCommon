using System;
using System.Security.Cryptography;
using System.Text;

namespace DotCommon.Utility
{
    /// <summary>SHA1算法工具类
    /// </summary>
    public static class SHAUtil
    {
        /// <summary>获取十六进制字符串的SHA1-Hash
        /// </summary>
        /// <param name="source">字符串</param>
        /// <param name="encode">编码</param>
        /// <returns></returns>
        public static string GetHex16StringSHA1Hash(string source, string encode = "utf-8")
        {
            var sourceBytes = Encoding.GetEncoding(encode).GetBytes(source);
            var hashBytes = GetSHA1Hash(sourceBytes);
            return ByteBufferUtil.ByteBufferToHex16(hashBytes);
        }

        /// <summary>获取字符串的SHA1-Hash Base64值
        /// </summary>
        /// <param name="source">字符串</param>
        /// <param name="encode">编码</param>
        /// <returns></returns>
        public static string GetBase64StringSHA1Hash(string source, string encode = "utf-8")
        {
            var sourceBytes = Encoding.GetEncoding(encode).GetBytes(source);
            var hashBytes = GetSHA1Hash(sourceBytes);
            return Convert.ToBase64String(hashBytes);
        }


        /// <summary>获取二进制的SHA1值
        /// </summary>
        /// <param name="sourceBuffer">二进制数据</param>
        /// <returns></returns>
        public static byte[] GetSHA1Hash(byte[] sourceBuffer)
        {
            using (var sha1 = SHA1.Create())
            {
                var hashBytes = sha1.ComputeHash(sourceBuffer);
                return hashBytes;
            }
        }

        /// <summary>获取十六进制字符串的SHA256-Hash
        /// </summary>
        /// <param name="source">字符串</param>
        /// <param name="encode">编码</param>
        /// <returns></returns>
        public static string GetHex16StringSHA256Hash(string source, string encode = "utf-8")
        {
            var sourceBytes = Encoding.GetEncoding(encode).GetBytes(source);
            var hashBytes = GetSHA256Hash(sourceBytes);
            return ByteBufferUtil.ByteBufferToHex16(hashBytes);
        }

        /// <summary>获取字符串的SHA256-Hash Base64值
        /// </summary>
        /// <param name="source">字符串</param>
        /// <param name="encode">编码</param>
        /// <returns></returns>
        public static string GetBase64StringSHA256Hash(string source, string encode = "utf-8")
        {
            var sourceBytes = Encoding.GetEncoding(encode).GetBytes(source);
            var hashBytes = GetSHA256Hash(sourceBytes);
            return Convert.ToBase64String(hashBytes);
        }


        /// <summary>获取二进制的SHA256值
        /// </summary>
        /// <param name="sourceBuffer">二进制数据</param>
        /// <returns></returns>
        public static byte[] GetSHA256Hash(byte[] sourceBuffer)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashBytes = sha256.ComputeHash(sourceBuffer);
                return hashBytes;
            }
        }


        /// <summary>获取十六进制字符串的SHA512-Hash
        /// </summary>
        /// <param name="source">字符串</param>
        /// <param name="encode">编码</param>
        /// <returns></returns>
        public static string GetHex16StringSHA512Hash(string source, string encode = "utf-8")
        {
            var sourceBytes = Encoding.GetEncoding(encode).GetBytes(source);
            var hashBytes = GetSHA512Hash(sourceBytes);
            return ByteBufferUtil.ByteBufferToHex16(hashBytes);
        }

        /// <summary>获取字符串的SHA512-Hash Base64值
        /// </summary>
        /// <param name="source">字符串</param>
        /// <param name="encode">编码</param>
        /// <returns></returns>
        public static string GetBase64StringSHA512Hash(string source, string encode = "utf-8")
        {
            var sourceBytes = Encoding.GetEncoding(encode).GetBytes(source);
            var hashBytes = GetSHA512Hash(sourceBytes);
            return Convert.ToBase64String(hashBytes);
        }

        /// <summary>获取二进制的SHA512值
        /// </summary>
        /// <param name="sourceBuffer">二进制数据</param>
        /// <returns></returns>
        public static byte[] GetSHA512Hash(byte[] sourceBuffer)
        {
            using (var sha512 = SHA512.Create())
            {
                var hashBytes = sha512.ComputeHash(sourceBuffer);
                return hashBytes;
            }
        }
    }
}
