using System;
using System.Text;

namespace DotCommon.Encrypt
{
    /// <summary>
    /// Provides Base64 encoding and decoding functionality.
    /// </summary>
    public static class Base64Helper
    {
        /// <summary>
        /// Encodes a string to Base64 format.
        /// </summary>
        /// <param name="data">The string to encode.</param>
        /// <param name="encoding">The text encoding to use. Default is UTF-8.</param>
        /// <returns>The Base64 encoded string.</returns>
        /// <exception cref="ArgumentNullException">Thrown when data is null.</exception>
        /// <exception cref="ArgumentException">Thrown when encoding is null or empty.</exception>
        public static string Base64Encode(string data, string encoding = "utf-8")
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            if (string.IsNullOrEmpty(encoding))
                throw new ArgumentException("Encoding cannot be null or empty.", nameof(encoding));

            var dataBytes = Encoding.GetEncoding(encoding).GetBytes(data);
            return Convert.ToBase64String(dataBytes);
        }

        /// <summary>
        /// Encodes byte array to Base64 format.
        /// </summary>
        /// <param name="data">The byte array to encode.</param>
        /// <returns>The Base64 encoded string.</returns>
        /// <exception cref="ArgumentNullException">Thrown when data is null.</exception>
        public static string Base64Encode(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            return Convert.ToBase64String(data);
        }

        /// <summary>
        /// Decodes a Base64 string to its original string representation.
        /// </summary>
        /// <param name="data">The Base64 encoded string.</param>
        /// <param name="encoding">The text encoding to use. Default is UTF-8.</param>
        /// <returns>The decoded string.</returns>
        /// <exception cref="ArgumentNullException">Thrown when data is null.</exception>
        /// <exception cref="ArgumentException">Thrown when encoding is null or empty.</exception>
        /// <exception cref="FormatException">Thrown when the input is not a valid Base64 string.</exception>
        public static string Base64Decode(string data, string encoding = "utf-8")
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            if (string.IsNullOrEmpty(encoding))
                throw new ArgumentException("Encoding cannot be null or empty.", nameof(encoding));

            try
            {
                var bytes = Convert.FromBase64String(data);
                return Encoding.GetEncoding(encoding).GetString(bytes);
            }
            catch (FormatException)
            {
                throw new FormatException("Input is not a valid Base64 string.");
            }
        }

        /// <summary>
        /// Decodes a Base64 string to byte array.
        /// </summary>
        /// <param name="data">The Base64 encoded string.</param>
        /// <returns>The decoded byte array.</returns>
        /// <exception cref="ArgumentNullException">Thrown when data is null.</exception>
        /// <exception cref="FormatException">Thrown when the input is not a valid Base64 string.</exception>
        public static byte[] Base64DecodeToBytes(string data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            try
            {
                return Convert.FromBase64String(data);
            }
            catch (FormatException)
            {
                throw new FormatException("Input is not a valid Base64 string.");
            }
        }

        /// <summary>
        /// Formats a Base64 string as a data URI for use in HTML/CSS/JavaScript.
        /// </summary>
        /// <param name="base64">The Base64 encoded string.</param>
        /// <param name="mimeType">The MIME type of the data. Default is "image/jpg".</param>
        /// <returns>A data URI string in the format "data:{mimeType};base64,{base64}".</returns>
        /// <exception cref="ArgumentNullException">Thrown when base64 is null.</exception>
        /// <exception cref="ArgumentException">Thrown when mimeType is null or empty.</exception>
        public static string FormatAsDataUri(string base64, string mimeType = "image/jpg")
        {
            if (base64 == null)
                throw new ArgumentNullException(nameof(base64));

            if (string.IsNullOrEmpty(mimeType))
                throw new ArgumentException("MIME type cannot be null or empty.", nameof(mimeType));

            return $"data:{mimeType};base64,{base64}";
        }
    }
}