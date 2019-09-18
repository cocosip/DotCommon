using System;
using System.Security.Cryptography;
using System.Text;

namespace DotCommon.Encrypt
{
    /// <summary>AES加密解密,16位密钥对应128位加密,24位密钥对应192位加密,32位密钥对应256位加密
    /// </summary>
    public class AesEncryptor
    {

        private readonly byte[] _key = { 0x0F, 0x0E, 0x0D, 0x0C, 0x0B, 0x0A, 0x09, 0x08, 0x07, 0x06, 0x05, 0x04, 0x03, 0x02, 0x01, 0x00 };
        private readonly byte[] _iv = { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F };

        /// <summary>密码模式
        /// </summary>
        public CipherMode Mode { get; set; } = CipherMode.CBC;

        /// <summary>填充模式
        /// </summary>
        public PaddingMode Padding { get; set; } = PaddingMode.PKCS7;

        /// <summary>密钥长度默认128
        /// </summary>
        public int KeySize { get; set; } = 128;

        /// <summary>Ctor
        /// </summary>
        public AesEncryptor()
        {

        }

        /// <summary>Ctor
        /// </summary>
        /// <param name="key">Aes密钥</param>
        public AesEncryptor(string key) : this()
        {
            _key = Convert.FromBase64String(key);
        }

        /// <summary>Ctor
        /// </summary>
        /// <param name="key">Aes密钥</param>
        /// <param name="iv">加密向量</param>
        public AesEncryptor(string key, string iv) : this(Convert.FromBase64String(key), Convert.FromBase64String(iv))
        {

        }

        /// <summary>Ctor
        /// </summary>
        /// <param name="key">Aes密钥</param>
        /// <param name="iv">加密向量</param>
        public AesEncryptor(byte[] key, byte[] iv)
        {
            _key = key;
            _iv = iv;
            if (_iv.Length != 16)
            {
                throw new ArgumentException("iv向量长度不正确");
            }
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
            using (var aes = new AesCryptoServiceProvider())
            {
                //Mode
                aes.Mode = Mode;
                aes.KeySize = KeySize;
                //Padding
                aes.Padding = Padding;
                var transform = aes.CreateEncryptor(_key, _iv);
                var bytes = transform.TransformFinalBlock(dataBytes, 0, dataBytes.Length);
                return bytes;
            }
        }

        /// <summary>解密核心
        /// </summary>
        private byte[] DecryptBytes(byte[] dataBytes)
        {
            using (var aes = new AesCryptoServiceProvider())
            {
                //Mode
                aes.Mode = Mode;
                aes.KeySize = KeySize;
                //Padding
                aes.Padding = Padding;
                var transform = aes.CreateDecryptor(_key, _iv);
                var bytes = transform.TransformFinalBlock(dataBytes, 0, dataBytes.Length);
                return bytes;
            }
        }

    }
}
