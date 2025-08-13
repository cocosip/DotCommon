using System;
using System.IO;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;

namespace DotCommon.Crypto.RSA
{
    public interface IRSAEncryptionService
    {
        /// <summary>
        /// Generates an RSA key pair.
        /// </summary>
        /// <param name="keySize">The key size in bits (e.g., 2048, 4096). Default is 2048.</param>
        /// <param name="rd">Optional: A secure random number generator. If null, a new one will be created.</param>
        /// <returns>An <see cref="AsymmetricCipherKeyPair"/> containing the public and private keys.</returns>
        AsymmetricCipherKeyPair GenerateRSAKeyPair(int keySize = 2048, SecureRandom? rd = null);

        /// <summary>
        /// Imports an RSA public key from a PEM formatted string.
        /// </summary>
        /// <param name="publicKeyPem">The PEM formatted string containing the public key.</param>
        /// <returns>An <see cref="AsymmetricKeyParameter"/> representing the public key.</returns>
        AsymmetricKeyParameter ImportPublicKeyPem(string publicKeyPem);

        /// <summary>
        /// Imports an RSA private key from a PEM formatted string.
        /// </summary>
        /// <param name="privateKeyPem">The PEM formatted string containing the private key.</param>
        /// <returns>An <see cref="AsymmetricKeyParameter"/> representing the private key.</returns>
        AsymmetricKeyParameter ImportPrivateKeyPem(string privateKeyPem);

        /// <summary>
        /// Imports an RSA private key from a PEM formatted string (PKCS#8 format).
        /// </summary>
        /// <param name="privateKeyPem">The PEM formatted string containing the PKCS#8 private key.</param>
        /// <returns>An <see cref="AsymmetricKeyParameter"/> representing the private key.</returns>
        AsymmetricKeyParameter ImportPrivateKeyPkcs8Pem(string privateKeyPem);

        /// <summary>
        /// Import public key from Base64 encoded string (SubjectPublicKeyInfo format)
        /// </summary>
        /// <param name="publicKeyBase64">Base64 encoded public key string</param>
        /// <returns>RSA public key parameter</returns>
        AsymmetricKeyParameter ImportPublicKey(string publicKeyBase64);

        /// <summary>
        /// Import private key from Base64 encoded string (PKCS#1 raw format)
        /// </summary>
        /// <param name="privateKeyBase64">Base64 encoded private key string</param>
        /// <returns>RSA private key parameter</returns>
        AsymmetricKeyParameter ImportPrivateKey(string privateKeyBase64);

        /// <summary>
        ///  RSA encrypts data.
        /// </summary>
        /// <param name="publicKeyParam"></param>
        /// <param name="plainText"></param>
        /// <param name="padding"></param>
        /// <returns></returns>
        byte[] Encrypt(AsymmetricKeyParameter publicKeyParam, byte[] plainText, string padding);

        /// <summary>
        /// RSA decrypts data.
        /// </summary>
        /// <param name="privateKeyParam"></param>
        /// <param name="cipherText"></param>
        /// <param name="padding"></param>
        /// <returns></returns>
        byte[] Decrypt(AsymmetricKeyParameter privateKeyParam, byte[] cipherText, string padding);

        /// <summary>
        /// RSA signs data.
        /// </summary>
        /// <param name="privateKeyParam"></param>
        /// <param name="data"></param>
        /// <param name="algorithm">SHA1WITHRSA,SHA256WITHRSA,SHA384WITHRSA,SHA512WITHRSA</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        byte[] Sign(AsymmetricKeyParameter privateKeyParam, byte[] data, string algorithm = "SHA256WITHRSA");

        /// <summary>
        /// RSA verifies a signature.
        /// </summary>
        /// <param name="publicKeyParam"></param>
        /// <param name="data"></param>
        /// <param name="signature"></param>
        /// <param name="algorithm"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        bool VerifySign(AsymmetricKeyParameter publicKeyParam, byte[] data, byte[] signature, string algorithm = "SHA256WITHRSA");
    }
}