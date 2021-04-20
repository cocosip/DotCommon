using System;

namespace DotCommon.Encrypt
{

    /// <summary>RSA私钥编码格式
    /// </summary>
    public enum RSAKeyFormat
    {
        /// <summary>
        /// PKCS1
        /// </summary>
        PKCS1 = 1,

        /// <summary>
        /// PKCS8
        /// </summary>
        PKCS8 = 2,

        /// <summary>
        /// 未知
        /// </summary>
        Unknow = 4
    }

    /// <summary>
    /// RSA密钥对
    /// </summary>
    public class RSAKeyPair : IEquatable<RSAKeyPair>
    {
        /// <summary>
        /// 公钥
        /// </summary>
        public string PublicKey { get; set; }

        /// <summary>
        /// 私钥
        /// </summary>
        public string PrivateKey { get; set; }

        /// <summary>
        /// ctor
        /// </summary>
        public RSAKeyPair()
        {

        }

        /// <summary>
        /// ctor
        /// </summary>
        public RSAKeyPair(string publicKey, string privateKey)
        {
            PublicKey = publicKey;
            PrivateKey = privateKey;
        }

        /// <summary>
        /// 是否相同
        /// </summary>
        public bool Equals(RSAKeyPair other)
        {
            return other.PrivateKey == PrivateKey && other.PublicKey == PublicKey;
        }
    }
}
