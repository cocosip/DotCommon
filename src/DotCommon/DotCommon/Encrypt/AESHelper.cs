using System;
using System.Security.Cryptography;
using System.Text;

namespace DotCommon.Encrypt
{
    /// <summary>
    /// Provides AES encryption and decryption functionality.
    /// </summary>
    public static class AESHelper
    {
        /// <summary>
        /// Default key size in bits.
        /// </summary>
        public const int DefaultKeySize = 256;

        /// <summary>
        /// Default block size in bits.
        /// </summary>
        public const int DefaultBlockSize = 128;

        /// <summary>
        /// Default cipher mode.
        /// </summary>
        public const CipherMode DefaultCipherMode = CipherMode.CBC;

        /// <summary>
        /// Default padding mode.
        /// </summary>
        public const PaddingMode DefaultPaddingMode = PaddingMode.PKCS7;

        /// <summary>
        /// Generates a random key for AES encryption.
        /// </summary>
        /// <param name="keySize">The key size in bits (128, 192, or 256).</param>
        /// <returns>A randomly generated key.</returns>
        public static byte[] GenerateKey(int keySize = DefaultKeySize)
        {
            if (keySize != 128 && keySize != 192 && keySize != 256)
            {
                throw new ArgumentException("Key size must be 128, 192, or 256 bits.", nameof(keySize));
            }

            using var aes = Aes.Create();
            aes.KeySize = keySize;
            aes.GenerateKey();
            return aes.Key;
        }

        /// <summary>
        /// Generates a random initialization vector (IV) for AES encryption.
        /// </summary>
        /// <returns>A randomly generated IV.</returns>
        public static byte[] GenerateIV()
        {
            using var aes = Aes.Create();
            aes.GenerateIV();
            return aes.IV;
        }

        /// <summary>
        /// Encrypts data using AES encryption.
        /// </summary>
        /// <param name="data">The plaintext data to encrypt.</param>
        /// <param name="key">The encryption key. If null, a default key will be used.</param>
        /// <param name="iv">The initialization vector. If null, a default IV will be used.</param>
        /// <param name="keySize">The key size in bits (128, 192, or 256). Default is 256.</param>
        /// <param name="mode">The cipher mode. Default is CBC.</param>
        /// <param name="padding">The padding mode. Default is PKCS7.</param>
        /// <returns>The encrypted data.</returns>
        /// <exception cref="ArgumentNullException">Thrown when data is null.</exception>
        /// <exception cref="CryptographicException">Thrown when encryption fails.</exception>
        public static byte[] Encrypt(
            byte[] data,
            byte[]? key = null,
            byte[]? iv = null,
            int keySize = DefaultKeySize,
            CipherMode mode = DefaultCipherMode,
            PaddingMode padding = DefaultPaddingMode)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            if (keySize != 128 && keySize != 192 && keySize != 256)
                throw new ArgumentException("Key size must be 128, 192, or 256 bits.", nameof(keySize));

            try
            {
                using var aes = Aes.Create();
                aes.KeySize = keySize;
                aes.Mode = mode;
                aes.Padding = padding;
                aes.Key = key ?? GenerateKey(keySize);
                aes.IV = iv ?? GenerateIV();

                using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                return encryptor.TransformFinalBlock(data, 0, data.Length);
            }
            catch (Exception ex)
            {
                throw new CryptographicException("Encryption failed.", ex);
            }
        }

        /// <summary>
        /// Encrypts a string using AES encryption.
        /// </summary>
        /// <param name="plainText">The plaintext string to encrypt.</param>
        /// <param name="key">The encryption key. If null, a default key will be used.</param>
        /// <param name="iv">The initialization vector. If null, a default IV will be used.</param>
        /// <param name="keySize">The key size in bits (128, 192, or 256). Default is 256.</param>
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
            int keySize = DefaultKeySize,
            CipherMode mode = DefaultCipherMode,
            PaddingMode padding = DefaultPaddingMode,
            Encoding? encoding = null)
        {
            if (plainText == null)
                throw new ArgumentNullException(nameof(plainText));

            encoding ??= Encoding.UTF8;
            var data = encoding.GetBytes(plainText);
            return Encrypt(data, key, iv, keySize, mode, padding);
        }

        /// <summary>
        /// Decrypts data using AES decryption.
        /// </summary>
        /// <param name="data">The encrypted data to decrypt.</param>
        /// <param name="key">The decryption key. If null, a default key will be used.</param>
        /// <param name="iv">The initialization vector. If null, a default IV will be used.</param>
        /// <param name="keySize">The key size in bits (128, 192, or 256). Default is 256.</param>
        /// <param name="mode">The cipher mode. Default is CBC.</param>
        /// <param name="padding">The padding mode. Default is PKCS7.</param>
        /// <returns>The decrypted data.</returns>
        /// <exception cref="ArgumentNullException">Thrown when data is null.</exception>
        /// <exception cref="CryptographicException">Thrown when decryption fails.</exception>
        public static byte[] Decrypt(
            byte[] data,
            byte[]? key = null,
            byte[]? iv = null,
            int keySize = DefaultKeySize,
            CipherMode mode = DefaultCipherMode,
            PaddingMode padding = DefaultPaddingMode)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            if (keySize != 128 && keySize != 192 && keySize != 256)
                throw new ArgumentException("Key size must be 128, 192, or 256 bits.", nameof(keySize));

            try
            {
                using var aes = Aes.Create();
                aes.KeySize = keySize;
                aes.Mode = mode;
                aes.Padding = padding;
                aes.Key = key ?? GenerateKey(keySize);
                aes.IV = iv ?? GenerateIV();

                using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                return decryptor.TransformFinalBlock(data, 0, data.Length);
            }
            catch (Exception ex)
            {
                throw new CryptographicException("Decryption failed.", ex);
            }
        }

        /// <summary>
        /// Decrypts data to a string using AES decryption.
        /// </summary>
        /// <param name="data">The encrypted data to decrypt.</param>
        /// <param name="key">The decryption key. If null, a default key will be used.</param>
        /// <param name="iv">The initialization vector. If null, a default IV will be used.</param>
        /// <param name="keySize">The key size in bits (128, 192, or 256). Default is 256.</param>
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
            int keySize = DefaultKeySize,
            CipherMode mode = DefaultCipherMode,
            PaddingMode padding = DefaultPaddingMode,
            Encoding? encoding = null)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            encoding ??= Encoding.UTF8;
            var decryptedBytes = Decrypt(data, key, iv, keySize, mode, padding);
            return encoding.GetString(decryptedBytes);
        }
    }
}