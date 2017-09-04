using DotCommon.Utility;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace DotCommon.Alg
{
    /// <summary>SHA1算法
    /// </summary>
    public class Sha1Alg
    {

        /// <summary> 获取字符的Hash值
        /// </summary>
        public static string GetStringSha1(string sourceString, string encode = "utf-8")
        {
            var sourceBytes = Encoding.GetEncoding(encode).GetBytes(sourceString);
            var hashBytes = GetSha1Hash(sourceBytes);
            return ByteBufferUtil.ByteArrayToString(hashBytes);
        }

        /// <summary> 获取字符的Hash值转为Base64
        /// </summary>
        public static string GetBase64StringSha1(string sourceString, string encode = "utf-8")
        {
            var sourceBytes = Encoding.GetEncoding(encode).GetBytes(sourceString);
            var hashBytes = GetSha1Hash(sourceBytes);
            return Convert.ToBase64String(hashBytes);
        }



        /// <summary>获取SHA1-哈希
        /// </summary>
        public static byte[] GetSha1Hash(byte[] sourceBytes)
        {
            var hashBytes = SHA1.Create().ComputeHash(sourceBytes);
            return hashBytes;
        }

    }
}
