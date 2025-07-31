using System;
using System.Security.Cryptography;
using System.Text;

namespace DotCommon.Utility
{
    /// <summary>
    /// Provides utility methods for SHA (Secure Hash Algorithm) operations.
    /// </summary>
    public static class SHAUtil
    {
        /// <summary>
        /// Computes the hash for the given data using the specified hash algorithm.
        /// </summary>
        /// <param name="hashAlgorithm">The hash algorithm instance (e.g., SHA1, SHA256, SHA512).</param>
        /// <param name="data">The input data to hash.</param>
        /// <returns>The computed hash as a byte array.</returns>
        private static byte[] ComputeHash(HashAlgorithm hashAlgorithm, byte[] data)
        {
            return hashAlgorithm.ComputeHash(data);
        }

        /// <summary>
        /// Computes the SHA1 hash for the given data.
        /// </summary>
        /// <param name="data">The input data as a byte array.</param>
        /// <returns>The computed SHA1 hash as a byte array.</returns>
        private static byte[] ComputeSha1(byte[] data)
        {
            using var sha1 = SHA1.Create();
            return ComputeHash(sha1, data);
        }

        /// <summary>
        /// Computes the SHA256 hash for the given data.
        /// </summary>
        /// <param name="data">The input data as a byte array.</param>
        /// <returns>The computed SHA256 hash as a byte array.</returns>
        private static byte[] ComputeSha256(byte[] data)
        {
            using var sha256 = SHA256.Create();
            return ComputeHash(sha256, data);
        }

        /// <summary>
        /// Computes the SHA512 hash for the given data.
        /// </summary>
        /// <param name="data">The input data as a byte array.</param>
        /// <returns>The computed SHA512 hash as a byte array.</returns>
        private static byte[] ComputeSha512(byte[] data)
        {
            using var sha512 = SHA512.Create();
            return ComputeHash(sha512, data);
        }

        /// <summary>
        /// Computes the SHA hash for the given string data and returns the result as a Base64 string.
        /// </summary>
        /// <param name="data">The input string data.</param>
        /// <param name="hashAlgorithm">A function that computes the SHA hash (e.g., ComputeSha1, ComputeSha256).</param>
        /// <param name="encoding">The encoding for the input data string. Defaults to UTF8.</param>
        /// <returns>The Base64 encoded SHA hash string.</returns>
        private static string ComputeShaToBase64(string data, Func<byte[], byte[]> hashAlgorithm, Encoding? encoding = null)
        {
            encoding ??= Encoding.UTF8;
            byte[] dataBytes = encoding.GetBytes(data);
            byte[] hashBytes = hashAlgorithm(dataBytes);
            return Convert.ToBase64String(hashBytes);
        }

        /// <summary>
        /// Computes the SHA hash for the given string data and returns the result as a hexadecimal string.
        /// </summary>
        /// <param name="data">The input string data.</param>
        /// <param name="hashAlgorithm">A function that computes the SHA hash (e.g., ComputeSha1, ComputeSha256).</param>
        /// <param name="encoding">The encoding for the input data string. Defaults to UTF8.</param>
        /// <returns>The hexadecimal encoded SHA hash string.</returns>
        private static string ComputeShaToHex(string data, Func<byte[], byte[]> hashAlgorithm, Encoding? encoding = null)
        {
            encoding ??= Encoding.UTF8;
            byte[] dataBytes = encoding.GetBytes(data);
            byte[] hashBytes = hashAlgorithm(dataBytes);
            return ByteBufferUtil.ByteBufferToHex16(hashBytes);
        }

        /// <summary>
        /// Computes the SHA1 hash for the given string data and returns the result as a hexadecimal string.
        /// </summary>
        /// <param name="data">The input string data.</param>
        /// <param name="encoding">The encoding for the input data string. Defaults to UTF8.</param>
        /// <returns>The hexadecimal encoded SHA1 hash string.</returns>
        public static string ComputeSha1ToHex(string data, Encoding? encoding = null)
        {
            return ComputeShaToHex(data, ComputeSha1, encoding);
        }

        /// <summary>
        /// Computes the SHA1 hash for the given string data and returns the result as a Base64 string.
        /// </summary>
        /// <param name="data">The input string data.</param>
        /// <param name="encoding">The encoding for the input data string. Defaults to UTF8.</param>
        /// <returns>The Base64 encoded SHA1 hash string.</returns>
        public static string ComputeSha1ToBase64(string data, Encoding? encoding = null)
        {
            return ComputeShaToBase64(data, ComputeSha1, encoding);
        }

        /// <summary>
        /// Computes the SHA256 hash for the given string data and returns the result as a hexadecimal string.
        /// </summary>
        /// <param name="data">The input string data.</param>
        /// <param name="encoding">The encoding for the input data string. Defaults to UTF8.</param>
        /// <returns>The hexadecimal encoded SHA256 hash string.</returns>
        public static string ComputeSha256ToHex(string data, Encoding? encoding = null)
        {
            return ComputeShaToHex(data, ComputeSha256, encoding);
        }

        /// <summary>
        /// Computes the SHA256 hash for the given string data and returns the result as a Base64 string.
        /// </summary>
        /// <param name="data">The input string data.</param>
        /// <param name="encoding">The encoding for the input data string. Defaults to UTF8.</param>
        /// <returns>The Base64 encoded SHA256 hash string.</returns>
        public static string ComputeSha256ToBase64(string data, Encoding? encoding = null)
        {
            return ComputeShaToBase64(data, ComputeSha256, encoding);
        }

        /// <summary>
        /// Computes the SHA512 hash for the given string data and returns the result as a hexadecimal string.
        /// </summary>
        /// <param name="data">The input string data.</param>
        /// <param name="encoding">The encoding for the input data string. Defaults to UTF8.</param>
        /// <returns>The hexadecimal encoded SHA512 hash string.</returns>
        public static string ComputeSha512ToHex(string data, Encoding? encoding = null)
        {
            return ComputeShaToHex(data, ComputeSha512, encoding);
        }

        /// <summary>
        /// Computes the SHA512 hash for the given string data and returns the result as a Base64 string.
        /// </summary>
        /// <param name="data">The input string data.</param>
        /// <param name="encoding">The encoding for the input data string. Defaults to UTF8.</param>
        /// <returns>The Base64 encoded SHA512 hash string.</returns>
        public static string ComputeSha512ToBase64(string data, Encoding? encoding = null)
        {
            return ComputeShaToBase64(data, ComputeSha512, encoding);
        }
    }
}