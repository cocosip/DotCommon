using System;
using System.Text;

namespace DotCommon.Encrypt
{
    /// <summary>Base64加密解密
    /// </summary>
    public class Base64Encryptor
    {
        #region Base64进行加密

        /// <summary>Base64进行加密
        /// </summary>
        public string Base64Encrypt(string encryptString, string encoding = "utf-8")
        {
            var bytes = Encoding.GetEncoding(encoding).GetBytes(encryptString);
            var base64 = Convert.ToBase64String(bytes);
            return base64;
        }

        #endregion

        #region Base64进行解密

        /// <summary>Base64进行解密
        /// </summary>
        public string Base64Decrypt(string deEncryptString, string encoding = "utf-8")
        {
            var bytes = Convert.FromBase64String(deEncryptString);
            var sourceStr = Encoding.GetEncoding(encoding).GetString(bytes);
            return sourceStr;
        }

        #endregion
    }
}
