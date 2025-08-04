using System;

namespace DotCommon.Encrypt
{

    /// <summary>
    /// RSA private key encoding format
    /// </summary>
    public enum RSAKeyFormat
    {
        /// <summary>
        /// PKCS1 format
        /// </summary>
        PKCS1 = 1,

        /// <summary>
        /// PKCS8 format
        /// </summary>
        PKCS8 = 2,

        /// <summary>
        /// Unknown format
        /// </summary>
        Unknow = 4
    }

    /// <summary>
    /// RSA key pair
    /// </summary>
    public class RSAKeyPair : IEquatable<RSAKeyPair>
    {
        /// <summary>
        /// Public key
        /// </summary>
        public string PublicKey { get; set; }

        /// <summary>
        /// Private key
        /// </summary>
        public string PrivateKey { get; set; }


        /// <summary>
        /// Initializes a new instance of the RSAKeyPair class
        /// </summary>
        /// <param name="publicKey">Public key</param>
        /// <param name="privateKey">Private key</param>
        public RSAKeyPair(string publicKey, string privateKey)
        {
            PublicKey = publicKey;
            PrivateKey = privateKey;
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type
        /// </summary>
        /// <param name="other">An object to compare with this object</param>
        /// <returns>true if the current object is equal to the other parameter; otherwise, false</returns>
        public bool Equals(RSAKeyPair other)
        {
            return other.PrivateKey == PrivateKey && other.PublicKey == PublicKey;
        }

        /// <summary>
        /// Returns a hash code for the current RSA key pair
        /// </summary>
        /// <returns>A hash code for the current object</returns>
        public override int GetHashCode()
        {
            return StringComparer.InvariantCulture.GetHashCode(PublicKey) ^ StringComparer.InvariantCulture.GetHashCode(PrivateKey);
        }
    }
}
