using DotCommon.Utility;
using System;
using System.Security.Cryptography;
using System.Text;

namespace DotCommon.Algorithm
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
            var hashBytes = MD5.Create().ComputeHash(sourceBytes);
            return hashBytes;
        }

    }
}
