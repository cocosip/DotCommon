using System;
using System.Security.Cryptography;
using System.Text;

namespace DotCommon.Encrypt
{
    public class TripleDesEncryptor
    {
        private readonly byte[] _key = { 0x0F, 0x0E, 0x0D, 0x0C, 0x0B, 0x0A, 0x09, 0x08, 0x07, 0x06, 0x05, 0x04, 0x03, 0x02, 0x01, 0x00 };
        private readonly byte[] _iv = { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F };

        public TripleDesEncryptor(string key, string iv)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                _key = Encoding.UTF8.GetBytes(key);
            }
            if (!string.IsNullOrWhiteSpace(iv))
            {
                _iv = Encoding.UTF8.GetBytes(iv);
            }
        }

        public TripleDesEncryptor(string key) : this(key, "")
        {

        }

        /// <summary>加密
        /// </summary>
        public byte[] Encrypt(byte[] dataBytes)
        {
            return EncryptBytes(dataBytes);
        }
        /// <summary>加密
        /// </summary>
        public string Encrypt(string data, string encode = "utf-8")
        {
            var dataBytes = Encoding.GetEncoding(encode).GetBytes(data);
            var encryptedData = EncryptBytes(dataBytes);
            return Convert.ToBase64String(encryptedData);
        }

        /// <summary>解密
        /// </summary>
        public byte[] Decrypt(byte[] dataBytes)
        {
            return DecryptBytes(dataBytes);
        }

        /// <summary>解密
        /// </summary>
        public string Decrypt(string data, string encode = "utf-8")
        {
            var dataBytes = Convert.FromBase64String(data);
            var dataSource = DecryptBytes(dataBytes);
            return Encoding.GetEncoding(encode).GetString(dataSource);
        }


        /// <summary>加密核心
        /// </summary>
        private byte[] EncryptBytes(byte[] dataBytes)
        {
            var aes = TripleDES.Create();
            var transform = aes.CreateEncryptor(_key, _iv);
            var bytes = transform.TransformFinalBlock(dataBytes, 0, dataBytes.Length);
            aes.Dispose();
            return bytes;
        }

        /// <summary>解密核心
        /// </summary>
        private byte[] DecryptBytes(byte[] dataBytes)
        {
            var aes = TripleDES.Create();
            var transform = aes.CreateDecryptor(_key, _iv);
            var bytes = transform.TransformFinalBlock(dataBytes, 0, dataBytes.Length);
            aes.Dispose();
            return bytes;
        }
    }
}
