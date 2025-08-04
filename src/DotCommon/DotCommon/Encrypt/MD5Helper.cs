using System;
using System.Security.Cryptography;
using System.Text;

namespace DotCommon.Encrypt
{
    /// <summary>
    /// MD5 utility class that provides MD5 hash computation functionality.
    /// </summary>
    /// <remarks>
    /// Note: The MD5 algorithm is considered insecure and should not be used for password storage or security-sensitive scenarios.
    /// It is recommended to use more secure hash algorithms such as SHA-256 or higher versions in security-sensitive scenarios.
    /// </remarks>
    public static class MD5Helper
    {
        /// <summary>
        /// Computes the MD5 hash value of a byte array.
        /// </summary>
        /// <param name="data">The byte array to compute the hash value for.</param>
        /// <returns>The MD5 hash value as a byte array.</returns>
        /// <exception cref="ArgumentNullException">Thrown when data is null.</exception>
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
        /// Computes the MD5 hash value of a byte array and returns it as a hexadecimal string.
        /// </summary>
        /// <param name="data">The byte array to compute the hash value for.</param>
        /// <returns>The MD5 hash value as a hexadecimal string (uppercase, no separators).</returns>
        /// <exception cref="ArgumentNullException">Thrown when data is null.</exception>
        public static string GetMD5(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            var md5Bytes = GetMD5Internal(data);
            return BitConverter.ToString(md5Bytes).Replace("-", "");
        }

        /// <summary>
        /// Computes the MD5 hash value of a string and returns it as a hexadecimal string.
        /// </summary>
        /// <param name="data">The string to compute the hash value for.</param>
        /// <param name="encodingName">The string encoding method, defaults to UTF-8.</param>
        /// <returns>The MD5 hash value as a hexadecimal string (uppercase, no separators).</returns>
        /// <exception cref="ArgumentNullException">Thrown when data is null.</exception>
        /// <exception cref="ArgumentException">Thrown when encodingName is null or invalid.</exception>
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