using System;
using System.Security.Cryptography;
using System.Text;

namespace DotCommon.Utility
{
    /// <summary>
    /// Provides utility methods for HMAC (Hash-based Message Authentication Code) operations.
    /// </summary>
    public static class HMACUtil
    {
        /// <summary>
        /// Computes the HMAC hash for the given data and key using the specified HMAC algorithm.
        /// </summary>
        /// <param name="hmacAlgorithm">The HMAC algorithm instance (e.g., HMACSHA1, HMACSHA256, HMACMD5).</param>
        /// <param name="data">The input data to hash.</param>
        /// <param name="key">The secret key for the HMAC computation.</param>
        /// <returns>The computed HMAC hash as a byte array.</returns>
        private static byte[] ComputeHash(HMAC hmacAlgorithm, byte[] data, byte[] key)
        {
            hmacAlgorithm.Key = key;
            return hmacAlgorithm.ComputeHash(data);
        }

        /// <summary>
        /// Computes the HMAC-SHA1 hash for the given data and key.
        /// </summary>
        /// <param name="data">The input data as a byte array.</param>
        /// <param name="key">The secret key as a byte array.</param>
        /// <returns>The computed HMAC-SHA1 hash as a byte array.</returns>
        private static byte[] ComputeHmacSha1(byte[] data, byte[] key)
        {
            using var hmac = new HMACSHA1();
            return ComputeHash(hmac, data, key);
        }

        /// <summary>
        /// Computes the HMAC-SHA256 hash for the given data and key.
        /// </summary>
        /// <param name="data">The input data as a byte array.</param>
        /// <param name="key">The secret key as a byte array.</param>
        /// <returns>The computed HMAC-SHA256 hash as a byte array.</returns>
        private static byte[] ComputeHmacSha256(byte[] data, byte[] key)
        {
            using var hmac = new HMACSHA256();
            return ComputeHash(hmac, data, key);
        }

        /// <summary>
        /// Computes the HMAC-MD5 hash for the given data and key.
        /// </summary>
        /// <param name="data">The input data as a byte array.</param>
        /// <param name="key">The secret key as a byte array.</param>
        /// <returns>The computed HMAC-MD5 hash as a byte array.</returns>
        private static byte[] ComputeHmacMd5(byte[] data, byte[] key)
        {
            using var hmac = new HMACMD5();
            return ComputeHash(hmac, data, key);
        }

        /// <summary>
        /// Computes the HMAC hash for the given string data and key, and returns the result as a Base64 string.
        /// </summary>
        /// <param name="data">The input string data.</param>
        /// <param name="key">The secret key string.</param>
        /// <param name="hmacAlgorithm">A function that computes the HMAC hash (e.g., ComputeHmacSha1, ComputeHmacSha256).</param>
        /// <param name="dataEncoding">The encoding for the input data string. Defaults to UTF8.</param>
        /// <param name="keyEncoding">The encoding for the secret key string. Defaults to UTF8.</param>
        /// <returns>The Base64 encoded HMAC hash string.</returns>
        private static string ComputeHmacToBase64(string data, string key, Func<byte[], byte[], byte[]> hmacAlgorithm, Encoding? dataEncoding = null, Encoding? keyEncoding = null)
        {
            dataEncoding ??= Encoding.UTF8;
            keyEncoding ??= Encoding.UTF8;

            byte[] dataBytes = dataEncoding.GetBytes(data);
            byte[] keyBytes = keyEncoding.GetBytes(key);
            byte[] hashBytes = hmacAlgorithm(dataBytes, keyBytes);
            return Convert.ToBase64String(hashBytes);
        }

        /// <summary>
        /// Computes the HMAC hash for the given string data and key, and returns the result as a hexadecimal string.
        /// </summary>
        /// <param name="data">The input string data.</param>
        /// <param name="key">The secret key string.</param>
        /// <param name="hmacAlgorithm">A function that computes the HMAC hash (e.g., ComputeHmacSha1, ComputeHmacSha256).</param>
        /// <param name="dataEncoding">The encoding for the input data string. Defaults to UTF8.</param>
        /// <param name="keyEncoding">The encoding for the secret key string. Defaults to UTF8.</param>
        /// <returns>The hexadecimal encoded HMAC hash string.</returns>
        private static string ComputeHmacToHex(string data, string key, Func<byte[], byte[], byte[]> hmacAlgorithm, Encoding? dataEncoding = null, Encoding? keyEncoding = null)
        {
            dataEncoding ??= Encoding.UTF8;
            keyEncoding ??= Encoding.UTF8;

            byte[] dataBytes = dataEncoding.GetBytes(data);
            byte[] keyBytes = keyEncoding.GetBytes(key);
            byte[] hashBytes = hmacAlgorithm(dataBytes, keyBytes);
            return ByteBufferUtil.ByteBufferToHex16(hashBytes);
        }

        /// <summary>
        /// Computes the HMAC-SHA1 hash for the given string data and key, and returns the result as a Base64 string.
        /// </summary>
        /// <param name="data">The input string data.</param>
        /// <param name="key">The secret key string. Defaults to "12345678".</param>
        /// <param name="dataEncoding">The encoding for the input data string. Defaults to UTF8.</param>
        /// <param name="keyEncoding">The encoding for the secret key string. Defaults to UTF8.</param>
        /// <returns>The Base64 encoded HMAC-SHA1 hash string.</returns>
        public static string ComputeHmacSha1ToBase64(string data, string key = "12345678", Encoding? dataEncoding = null, Encoding? keyEncoding = null)
        {
            return ComputeHmacToBase64(data, key, ComputeHmacSha1, (Encoding?)dataEncoding, (Encoding?)keyEncoding);
        }

        /// <summary>
        /// Computes the HMAC-SHA1 hash for the given string data and key, and returns the result as a hexadecimal string.
        /// </summary>
        /// <param name="data">The input string data.</param>
        /// <param name="key">The secret key string. Defaults to "12345678".</param>
        /// <param name="dataEncoding">The encoding for the input data string. Defaults to UTF8.</param>
        /// <param name="keyEncoding">The encoding for the secret key string. Defaults to UTF8.</param>
        /// <returns>The hexadecimal encoded HMAC-SHA1 hash string.</returns>
        public static string ComputeHmacSha1ToHex(string data, string key = "12345678", Encoding? dataEncoding = null, Encoding? keyEncoding = null)
        {
            return ComputeHmacToHex(data, key, ComputeHmacSha1, (Encoding?)dataEncoding, (Encoding?)keyEncoding);
        }

        /// <summary>
        /// Computes the HMAC-SHA256 hash for the given string data and key, and returns the result as a Base64 string.
        /// </summary>
        /// <param name="data">The input string data.</param>
        /// <param name="key">The secret key string. Defaults to "12345678".</param>
        /// <param name="dataEncoding">The encoding for the input data string. Defaults to UTF8.</param>
        /// <param name="keyEncoding">The encoding for the secret key string. Defaults to UTF8.</param>
        /// <returns>The Base64 encoded HMAC-SHA256 hash string.</returns>
        public static string ComputeHmacSha256ToBase64(string data, string key = "12345678", Encoding? dataEncoding = null, Encoding? keyEncoding = null)
        {
            return ComputeHmacToBase64(data, key, ComputeHmacSha256, (Encoding?)dataEncoding, (Encoding?)keyEncoding);
        }

        /// <summary>
        /// Computes the HMAC-SHA256 hash for the given string data and key, and returns the result as a hexadecimal string.
        /// </summary>
        /// <param name="data">The input string data.</param>
        /// <param name="key">The secret key string. Defaults to "12345678".</param>
        /// <param name="dataEncoding">The encoding for the input data string. Defaults to UTF8.</param>
        /// <param name="keyEncoding">The encoding for the secret key string. Defaults to UTF8.</param>
        /// <returns>The hexadecimal encoded HMAC-SHA256 hash string.</returns>
        public static string ComputeHmacSha256ToHex(string data, string key = "12345678", Encoding? dataEncoding = null, Encoding? keyEncoding = null)
        {
            return ComputeHmacToHex(data, key, ComputeHmacSha256, (Encoding?)dataEncoding, (Encoding?)keyEncoding);
        }

        /// <summary>
        /// Computes the HMAC-MD5 hash for the given string data and key, and returns the result as a Base64 string.
        /// </summary>
        /// <param name="data">The input string data.</param>
        /// <param name="key">The secret key string. Defaults to "12345678".</param>
        /// <param name="dataEncoding">The encoding for the input data string. Defaults to UTF8.</param>
        /// <param name="keyEncoding">The encoding for the secret key string. Defaults to UTF8.</param>
        /// <returns>The Base64 encoded HMAC-MD5 hash string.</returns>
        public static string ComputeHmacMd5ToBase64(string data, string key = "12345678", Encoding? dataEncoding = null, Encoding? keyEncoding = null)
        {
            return ComputeHmacToBase64(data, key, ComputeHmacMd5, (Encoding?)dataEncoding, (Encoding?)keyEncoding);
        }

        /// <summary>
        /// Computes the HMAC-MD5 hash for the given string data and key, and returns the result as a hexadecimal string.
        /// </summary>
        /// <param name="data">The input string data.</param>
        /// <param name="key">The secret key string. Defaults to "12345678".</param>
        /// <param name="dataEncoding">The encoding for the input data string. Defaults to UTF8.</param>
        /// <param name="keyEncoding">The encoding for the secret key string. Defaults to UTF8.</param>
        /// <returns>The hexadecimal encoded HMAC-MD5 hash string.</returns>
        public static string ComputeHmacMd5ToHex(string data, string key = "12345678", Encoding? dataEncoding = null, Encoding? keyEncoding = null)
        {
            return ComputeHmacToHex(data, key, ComputeHmacMd5, (Encoding?)dataEncoding, (Encoding?)keyEncoding);
        }
    }
}