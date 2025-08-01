namespace DotCommon.Crypto.SM4
{
    /// <summary>
    /// Defines constants for SM4 encryption modes and padding schemes.
    /// </summary>
    public class Sm4EncryptionNames
    {
        /// <summary>
        /// Represents Electronic Codebook (ECB) mode.
        /// </summary>
        public const string ModeECB = "ECB";

        /// <summary>
        /// Represents Cipher Block Chaining (CBC) mode.
        /// </summary>
        public const string ModeCBC = "CBC";

        /// <summary>
        /// Represents PKCS7 padding scheme.
        /// </summary>
        public const string PKCS7Padding = "PKCS7Padding";

        /// <summary>
        /// Represents no padding scheme.
        /// </summary>
        public const string NoPadding = "NoPadding";
    }
}