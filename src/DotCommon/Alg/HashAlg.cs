using DotCommon.Utility;
using System;
using System.Security.Cryptography;
using System.Text;

namespace DotCommon.Alg
{
    /// <summary>哈希值计算
    /// </summary>
    public class HashAlg
    {
        /// <summary>获取字符串的MD5 Hash
        /// </summary>
        public static string GetStringMd5Hash(string sourceString, string encode = "utf-8")
        {
            var sourceBytes = Encoding.GetEncoding(encode).GetBytes(sourceString);
            var hashBytes = GetMd5Hash(sourceBytes);
            return ByteBufferUtil.ByteArrayToString(hashBytes);
        }

        /// <summary>获取字符串的MD5-Hash Base64值 
        /// </summary>
        public static string GetBase64StringMd5Hash(string sourceString, string encode = "utf-8")
        {
            var sourceBytes = Encoding.GetEncoding(encode).GetBytes(sourceString);
            var hashBytes = GetMd5Hash(sourceBytes);
            return Convert.ToBase64String(hashBytes);
        }


        /// <summary>文件bytes
        /// </summary>
        public static byte[] GetMd5Hash(byte[] sourceBytes)
        {
            using (var md5 = MD5.Create())
            {
                var hashBytes = md5.ComputeHash(sourceBytes);
                return hashBytes;
            }
        }


        /// <summary>获取字符串的Sha1 Hash
        /// </summary>
        public static string GetStringSha1Hash(string sourceString, string encode = "utf-8")
        {
            var sourceBytes = Encoding.GetEncoding(encode).GetBytes(sourceString);
            var hashBytes = GetSha1Hash(sourceBytes);
            return ByteBufferUtil.ByteArrayToString(hashBytes);
        }

        /// <summary>获取字符串的Sha1-Hash Base64值 
        /// </summary>
        public static string GetBase64StringSha1Hash(string sourceString, string encode = "utf-8")
        {
            var sourceBytes = Encoding.GetEncoding(encode).GetBytes(sourceString);
            var hashBytes = GetSha1Hash(sourceBytes);
            return Convert.ToBase64String(hashBytes);
        }


        /// <summary>文件bytes
        /// </summary>
        public static byte[] GetSha1Hash(byte[] sourceBytes)
        {
            using (var sha1 = SHA1.Create())
            {
                var hashBytes = sha1.ComputeHash(sourceBytes);
                return hashBytes;
            }
        }

    }
}
