using System;
using System.Security.Cryptography;
using System.Text;

namespace DotCommon.Encrypt
{
    /// <summary>
    /// MD5工具类，提供MD5哈希计算功能
    /// </summary>
    /// <remarks>
    /// 注意：MD5算法已被认为不安全，不应用于密码存储或安全敏感场景。
    /// 建议在安全敏感场景中使用更安全的哈希算法，如SHA-256或更高版本。
    /// </remarks>
    public static class MD5Helper
    {
        /// <summary>
        /// 计算字节数组的MD5哈希值
        /// </summary>
        /// <param name="data">要计算哈希值的字节数组</param>
        /// <returns>MD5哈希值的字节数组</returns>
        /// <exception cref="ArgumentNullException">当data为null时抛出</exception>
        public static byte[] GetMD5Internal(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            using (var md5 = MD5.Create())
            {
                return md5.ComputeHash(data);
            }
        }

        /// <summary>
        /// 计算字节数组的MD5哈希值并返回十六进制字符串
        /// </summary>
        /// <param name="data">要计算哈希值的字节数组</param>
        /// <returns>MD5哈希值的十六进制字符串（大写，无分隔符）</returns>
        /// <exception cref="ArgumentNullException">当data为null时抛出</exception>
        public static string GetMD5(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            var md5Bytes = GetMD5Internal(data);
            return BitConverter.ToString(md5Bytes).Replace("-", "");
        }

        /// <summary>
        /// 计算字符串的MD5哈希值并返回十六进制字符串
        /// </summary>
        /// <param name="data">要计算哈希值的字符串</param>
        /// <param name="encodingName">字符串编码方式，默认为UTF-8</param>
        /// <returns>MD5哈希值的十六进制字符串（大写，无分隔符）</returns>
        /// <exception cref="ArgumentNullException">当data为null时抛出</exception>
        /// <exception cref="ArgumentException">当encodingName为空或无效时抛出</exception>
        public static string GetMD5(string data, string encodingName = "UTF-8")
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            if (string.IsNullOrWhiteSpace(encodingName))
                throw new ArgumentException("Encoding name cannot be null or empty", nameof(encodingName));

            try
            {
                return GetMD5(Encoding.GetEncoding(encodingName).GetBytes(data));
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException($"Invalid encoding name: {encodingName}", nameof(encodingName), ex);
            }
        }
    }
}