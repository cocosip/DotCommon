namespace DotCommon.Crypto.SM4
{
    /// <summary>
    /// Configuration options for SM4 encryption services.
    /// </summary>
    public class DotCommonSm4EncryptionOptions
    {
        /// <summary>
        /// Gets or sets the default Initialization Vector (IV) for SM4 encryption.
        /// </summary>
        public byte[]? DefaultIv { get; set; }

        /// <summary>
        /// Gets or sets the default encryption mode (e.g., ECB, CBC).
        /// </summary>
        public string? DefaultMode { get; set; }

        /// <summary>
        /// Gets or sets the default padding scheme (e.g., PKCS7Padding, NoPadding).
        /// </summary>
        public string? DefaultPadding { get; set; }
    }
}