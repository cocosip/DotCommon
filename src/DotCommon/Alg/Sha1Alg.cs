using DotCommon.Utility;
using System;
using System.Security.Cryptography;
using System.Text;

namespace DotCommon.Alg
{
    /// <summary>SHA1算法
    /// </summary>
    public class Sha1Alg
    {
        protected Sha1Alg()
        {

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
            using(var sha1 = SHA1.Create())
            {
                var hashBytes = sha1.ComputeHash(sourceBytes);
                return hashBytes;
            }
        }

    }
}
