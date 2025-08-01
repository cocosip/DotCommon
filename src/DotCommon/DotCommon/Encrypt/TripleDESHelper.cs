using System;
using System.Security.Cryptography;
using System.Text;

namespace DotCommon.Encrypt
{
    /// <summary>
    /// Provides TripleDES encryption and decryption functionality.
    /// </summary>
    public static class TripleDESHelper
    {
        /// <summary>
        /// Default key for TripleDES encryption (24 bytes).
        /// </summary>
        private static readonly byte[] DefaultKey = {
            0x0F, 0x0E, 0x0D, 0x0C, 0x0B, 0x0A, 0x09, 0x08,
            0x07, 0x06, 0x05, 0x04, 0x03, 0x02, 0x01, 0x00,
            0x0F, 0x0E, 0x0D, 0x0C, 0x0B, 0x0A, 0x09, 0x08
        };

        /// <summary>
        /// Default initialization vector for TripleDES encryption (8 bytes).
        /// </summary>
        private static readonly byte[] DefaultIV = {
            0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07
        };

        /// <summary>
        /// Default key size in bits for TripleDES.
        /// </summary>
        public const int DefaultKeySize = 192;

        /// <summary>
        /// Default block size in bits for TripleDES.
        /// </summary>
        public const int DefaultBlockSize = 64;

        /// <summary>
        /// Default cipher mode for TripleDES.
        /// </summary>
        public const CipherMode DefaultCipherMode = CipherMode.CBC;

        /// <summary>
        /// Default padding mode for TripleDES.
        /// </summary>
        public const PaddingMode DefaultPaddingMode = PaddingMode.PKCS7;

        /// <summary>
        /// Generates a random key for TripleDES encryption.
        /// </summary>
        /// <returns>A randomly generated key.</returns>
        public static byte[] GenerateKey()
        {
            using var tripleDes = TripleDES.Create();
            tripleDes.GenerateKey();
            return tripleDes.Key;
        }

        /// <summary>
        /// Generates a random initialization vector (IV) for TripleDES encryption.
        /// </summary>
        /// <returns>A randomly generated IV.</returns>
        public static byte[] GenerateIV()
        {
            using var tripleDes = TripleDES.Create();
            tripleDes.GenerateIV();
            return tripleDes.IV;
        }

        /// <summary>
        /// Ensures the IV is the correct size for TripleDES (8 bytes).
        /// If the provided IV is larger, it will be truncated.
        /// If smaller, it will be padded with zeros.
        /// </summary>
        /// <param name="iv">The IV to adjust.</param>
        /// <returns>An IV that is exactly 8 bytes long.</returns>
        private static byte[] AdjustIV(byte[]? iv)
        {
            if (iv == null)
                return (byte[])DefaultIV.Clone();

            if (iv.Length == 8)
                return (byte[])iv.Clone();

            var adjustedIV = new byte[8];
            if (iv.Length > 8)
            {
                // Truncate to 8 bytes
                Array.Copy(iv, adjustedIV, 8);
            }
            else
            {
                // Pad with zeros
                Array.Copy(iv, adjustedIV, iv.Length);
            }

            return adjustedIV;
        }

        /// <summary>
        /// Ensures the key is the correct size for TripleDES (24 bytes).
        /// If the provided key is larger, it will be truncated.
        /// If smaller, it will be padded with zeros.
        /// </summary>
        /// <param name="key">The key to adjust.</param>
        /// <returns>A key that is exactly 24 bytes long.</returns>
        private static byte[] AdjustKey(byte[]? key)
        {
            if (key == null)
                return (byte[])DefaultKey.Clone();

            if (key.Length == 24)
                return (byte[])key.Clone();

            var adjustedKey = new byte[24];
            if (key.Length > 24)
            {
                // Truncate to 24 bytes
                Array.Copy(key, adjustedKey, 24);
            }
            else
            {
                // Pad with zeros
                Array.Copy(key, adjustedKey, key.Length);
            }

            return adjustedKey;
        }

        /// <summary>
        /// Encrypts data using TripleDES encryption.
        /// </summary>
        /// <param name="data">The plaintext data to encrypt.</param>
        /// <param name="key">The encryption key. If null, a default key will be used.</param>
        /// <param name="iv">The initialization vector. If null, a default IV will be used.</param>
        /// <param name="mode">The cipher mode. Default is CBC.</param>
        /// <param name="padding">The padding mode. Default is PKCS7.</param>
        /// <returns>The encrypted data.</returns>
        /// <exception cref="ArgumentNullException">Thrown when data is null.</exception>
        /// <exception cref="CryptographicException">Thrown when encryption fails.</exception>
        public static byte[] Encrypt(
            byte[] data,
            byte[]? key = null,
            byte[]? iv = null,
            CipherMode mode = DefaultCipherMode,
            PaddingMode padding = DefaultPaddingMode)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            try
            {
                using var tripleDes = TripleDES.Create();
                tripleDes.Mode = mode;
                tripleDes.Padding = padding;

                // Adjust key and IV to correct sizes
                tripleDes.Key = AdjustKey(key);
                tripleDes.IV = AdjustIV(iv);

                using var encryptor = tripleDes.CreateEncryptor(tripleDes.Key, tripleDes.IV);
                return encryptor.TransformFinalBlock(data, 0, data.Length);
            }
            catch (Exception ex)
            {
                throw new CryptographicException("Encryption failed.", ex);
            }
        }

        /// <summary>
        /// Encrypts a string using TripleDES encryption.
        /// </summary>
        /// <param name="plainText">The plaintext string to encrypt.</param>
        /// <param name="key">The encryption key. If null, a default key will be used.</param>
        /// <param name="iv">The initialization vector. If null, a default IV will be used.</param>
        /// <param name="mode">The cipher mode. Default is CBC.</param>
        /// <param name="padding">The padding mode. Default is PKCS7.</param>
        /// <param name="encoding">The text encoding. Default is UTF-8.</param>
        /// <returns>The encrypted data.</returns>
        /// <exception cref="ArgumentNullException">Thrown when plainText is null.</exception>
        /// <exception cref="CryptographicException">Thrown when encryption fails.</exception>
        public static byte[] Encrypt(
            string plainText,
            byte[]? key = null,
            byte[]? iv = null,
            CipherMode mode = DefaultCipherMode,
            PaddingMode padding = DefaultPaddingMode,
            Encoding? encoding = null)
        {
            if (plainText == null)
                throw new ArgumentNullException(nameof(plainText));

            encoding ??= Encoding.UTF8;
            var data = encoding.GetBytes(plainText);
            return Encrypt(data, key, iv, mode, padding);
        }

        /// <summary>
        /// Decrypts data using TripleDES decryption.
        /// </summary>
        /// <param name="data">The encrypted data to decrypt.</param>
        /// <param name="key">The decryption key. If null, a default key will be used.</param>
        /// <param name="iv">The initialization vector. If null, a default IV will be used.</param>
        /// <param name="mode">The cipher mode. Default is CBC.</param>
        /// <param name="padding">The padding mode. Default is PKCS7.</param>
        /// <returns>The decrypted data.</returns>
        /// <exception cref="ArgumentNullException">Thrown when data is null.</exception>
        /// <exception cref="CryptographicException">Thrown when decryption fails.</exception>
        public static byte[] Decrypt(
            byte[] data,
            byte[]? key = null,
            byte[]? iv = null,
            CipherMode mode = DefaultCipherMode,
            PaddingMode padding = DefaultPaddingMode)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            try
            {
                using var tripleDes = TripleDES.Create();
                tripleDes.Mode = mode;
                tripleDes.Padding = padding;

                // Adjust key and IV to correct sizes
                tripleDes.Key = AdjustKey(key);
                tripleDes.IV = AdjustIV(iv);

                using var decryptor = tripleDes.CreateDecryptor(tripleDes.Key, tripleDes.IV);
                return decryptor.TransformFinalBlock(data, 0, data.Length);
            }
            catch (Exception ex)
            {
                throw new CryptographicException("Decryption failed.", ex);
            }
        }

        /// <summary>
        /// Decrypts data to a string using TripleDES decryption.
        /// </summary>
        /// <param name="data">The encrypted data to decrypt.</param>
        /// <param name="key">The decryption key. If null, a default key will be used.</param>
        /// <param name="iv">The initialization vector. If null, a default IV will be used.</param>
        /// <param name="mode">The cipher mode. Default is CBC.</param>
        /// <param name="padding">The padding mode. Default is PKCS7.</param>
        /// <param name="encoding">The text encoding. Default is UTF-8.</param>
        /// <returns>The decrypted string.</returns>
        /// <exception cref="ArgumentNullException">Thrown when data is null.</exception>
        /// <exception cref="CryptographicException">Thrown when decryption fails.</exception>
        public static string DecryptToString(
            byte[] data,
            byte[]? key = null,
            byte[]? iv = null,
            CipherMode mode = DefaultCipherMode,
            PaddingMode padding = DefaultPaddingMode,
            Encoding? encoding = null)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            encoding ??= Encoding.UTF8;
            var decryptedBytes = Decrypt(data, key, iv, mode, padding);
            return encoding.GetString(decryptedBytes);
        }
    }
}