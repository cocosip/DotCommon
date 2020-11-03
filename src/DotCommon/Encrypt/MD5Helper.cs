using System;
using System.Security.Cryptography;
using System.Text;

namespace DotCommon.Encrypt
{
    public static class MD5Helper
    {

        /// <summary>
        /// MD5加密
        /// </summary>
        public static byte[] GetMD5Internal(byte[] data)
        {
            using (var md5 = MD5.Create())
            {
                return md5.ComputeHash(data);
            }
        }

        /// <summary>
        /// Md5加密
        /// </summary>
        public static string GetMD5(byte[] data)
        {
            var md5Bytes = GetMD5Internal(data);
            //其他的代码写法: md5.ComputeHash(data).Aggregate("", (current, b) => current + b.ToString("X2"))
            return BitConverter.ToString(md5Bytes).Replace("-", "");
        }

        /// <summary>
        /// Md5加密
        /// </summary>
        public static string GetMD5(string data, string encode = "utf-8")
        {
            return GetMD5(Encoding.GetEncoding(encode).GetBytes(data));
        }
    }
}
