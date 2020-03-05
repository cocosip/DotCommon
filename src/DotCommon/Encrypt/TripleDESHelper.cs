using System.Security.Cryptography;

namespace DotCommon.Encrypt
{
    /// <summary>DES加密相关操作
    /// </summary>
    public static class TripleDESHelper
    {
        private static readonly byte[] _key = { 0x0F, 0x0E, 0x0D, 0x0C, 0x0B, 0x0A, 0x09, 0x08, 0x07, 0x06, 0x05, 0x04, 0x03, 0x02, 0x01, 0x00 };
        private static readonly byte[] _iv = { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F };

        /// <summary>加密
        /// </summary>
        /// <param name="data">待加密的数据</param>
        /// <param name="key">密钥Key</param>
        /// <param name="iv">Iv向量</param>
        /// <param name="keySize">Key的位数</param>
        /// <param name="mode">CipherMode</param>
        /// <param name="padding">PaddingMode</param>
        /// <returns></returns>
        public static byte[] Encrypt(byte[] data, byte[] key = null, byte[] iv = null, int keySize = 128, CipherMode mode = CipherMode.CBC, PaddingMode padding = PaddingMode.PKCS7)
        {
            using (var aes = new TripleDESCryptoServiceProvider())
            {
                key ??= _key;
                iv ??= _iv;
                //Mode
                aes.Mode = mode;
                aes.KeySize = keySize;
                //Padding
                aes.Padding = padding;
                var transform = aes.CreateEncryptor(key, iv);
                var bytes = transform.TransformFinalBlock(data, 0, data.Length);
                return bytes;
            }
        }


        /// <summary>解密
        /// </summary>
        /// <param name="data">加密后的数据</param>
        /// <param name="key">密钥Key</param>
        /// <param name="iv">Iv向量</param>
        /// <param name="keySize">Key的位数</param>
        /// <param name="mode">CipherMode</param>
        /// <param name="padding">PaddingMode</param>
        /// <returns></returns>
        public static byte[] Decrypt(byte[] data, byte[] key = null, byte[] iv = null, int keySize = 128, CipherMode mode = CipherMode.CBC, PaddingMode padding = PaddingMode.PKCS7)
        {
            using (var aes = new TripleDESCryptoServiceProvider())
            {
                key ??= _key;
                iv ??= _iv;
                //Mode
                aes.Mode = mode;
                aes.KeySize = keySize;
                //Padding
                aes.Padding = padding;
                var transform = aes.CreateDecryptor(key, iv);
                var bytes = transform.TransformFinalBlock(data, 0, data.Length);
                return bytes;
            }
        }

    }
}
