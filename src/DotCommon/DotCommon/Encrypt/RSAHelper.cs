using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace DotCommon.Encrypt
{
    /// <summary>
    /// RSA encryption and key operations
    /// </summary>
    public static class RSAHelper
    {
#if NETSTANDARD2_0
        /// <summary>Fixed content encoded OID sequence for PKCS #1 rsaEncryption szOID_RSA_RSA ="1.2.840.113549.1.1.1"
        /// </summary>
        private static readonly byte[] SeqOID = { 0x30, 0x0D, 0x06, 0x09, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x01, 0x01, 0x05, 0x00 };

        /// <summary>Fixed version number
        /// </summary>
        private static readonly byte[] Version = { 0x02, 0x01, 0x00 };

        private static readonly Dictionary<string, HashAlgorithmName> HashAlgorithmNameDict = new Dictionary<string, HashAlgorithmName>()
        {
            {"MD5",HashAlgorithmName.MD5 },
            {"SHA1",HashAlgorithmName.SHA1 },
            {"SHA256",HashAlgorithmName.SHA256 },
            {"SHA384",HashAlgorithmName.SHA384 },
            {"SHA512",HashAlgorithmName.SHA512 },
        };
#endif

        /// <summary>
        /// Generate RSA key pair (PEM key format)
        /// </summary>
        /// <param name="format">Format, PKCS1 or PKCS8</param>
        /// <param name="keySize">512, 1024, 1536, 2048</param>
        /// <param name="subjectPublicKey">Whether to use SubjectPublicKey format (only valid in .NET Standard 2.1 and above)</param>
        /// <returns>Public key, private key</returns>
        public static RSAKeyPair GenerateKeyPair(RSAKeyFormat format = RSAKeyFormat.PKCS1, int keySize = 2048, bool subjectPublicKey = true)
        {
#if NETSTANDARD2_0
            using var rsa = RSA.Create();
            rsa.KeySize = keySize;

            var publicParameters = rsa.ExportParameters(false);
            var privateParameters = rsa.ExportParameters(true);
            string publicKey = ExportPublicKey(publicParameters);
            string privateKey = format == RSAKeyFormat.PKCS1 ? ExportPrivateKeyPKCS1(privateParameters) : ExportPrivateKeyPKCS8(privateParameters);
            return new RSAKeyPair(publicKey, privateKey);
#else
            using var rsa = RSA.Create();
            rsa.KeySize = keySize;

            var privateKeyBuffer = format == RSAKeyFormat.PKCS1 ? rsa.ExportRSAPrivateKey() : rsa.ExportPkcs8PrivateKey();
            var privateKey = Convert.ToBase64String(privateKeyBuffer);

            var publicKeyBuffer = subjectPublicKey ? rsa.ExportSubjectPublicKeyInfo() : rsa.ExportRSAPublicKey();
            var publicKey = Convert.ToBase64String(publicKeyBuffer);
            return new RSAKeyPair(publicKey, privateKey);
#endif
        }

#if NETSTANDARD2_0
        /// <summary>
        /// Generate public key from RSAParameters
        /// </summary>
        /// <param name="rsaParameters">RSA parameters</param>
        /// <returns></returns>
        public static string ExportPublicKey(RSAParameters rsaParameters)
        {
            //Exponent
            var exponentBytes = TLVFormat(0x02, rsaParameters.Exponent);
            //Modulus
            var modulusBytes = TLVFormat(0x02, rsaParameters.Modulus);
            var contentBytes = new List<byte>();
            contentBytes.AddRange(modulusBytes);
            contentBytes.AddRange(exponentBytes);
            contentBytes = TLVFormat(0x30, contentBytes.ToArray());
            contentBytes.Insert(0, 0x00);

            //Content, Modulus+Exponent
            var bodyBytes = TLVFormat(0x03, contentBytes.ToArray());
            bodyBytes.InsertRange(0, SeqOID);

            //Final key binary
            var keyBytes = TLVFormat(0x30, bodyBytes.ToArray());
            return Convert.ToBase64String(keyBytes.ToArray());
        }

        /// <summary>
        /// Generate PKCS1 private key from RSAParameters
        /// </summary>
        /// <param name="rsaParameters">RSA parameters</param>
        /// <returns></returns>
        public static string ExportPrivateKeyPKCS1(RSAParameters rsaParameters)
        {
            //Key data excluding header length
            var bodyBytes = new List<byte>();
            //Version number
            bodyBytes.AddRange(Version);
            //Modulus
            bodyBytes.AddRange(TLVFormat(0x02, rsaParameters.Modulus));
            //Exponent
            bodyBytes.AddRange(TLVFormat(0x02, rsaParameters.Exponent));
            //D
            bodyBytes.AddRange(TLVFormat(0x02, rsaParameters.D));
            //P
            bodyBytes.AddRange(TLVFormat(0x02, rsaParameters.P));
            //Q
            bodyBytes.AddRange(TLVFormat(0x02, rsaParameters.Q));
            //DP
            bodyBytes.AddRange(TLVFormat(0x02, rsaParameters.DP));
            //DQ
            bodyBytes.AddRange(TLVFormat(0x02, rsaParameters.DQ));
            //InverseQ
            bodyBytes.AddRange(TLVFormat(0x02, rsaParameters.InverseQ));
            //Key binary
            var keyBytes = TLVFormat(0x30, bodyBytes.ToArray());
            return Convert.ToBase64String(keyBytes.ToArray());
        }

        /// <summary>
        /// Generate PKCS8 private key from RSAParameters
        /// </summary>
        public static string ExportPrivateKeyPKCS8(RSAParameters rsaParameters)
        {
            //Data after the second version
            var contentBytes = new List<byte>();
            //Version number
            contentBytes.AddRange(Version);

            //Modulus
            contentBytes.AddRange(TLVFormat(0x02, rsaParameters.Modulus));
            //Exponent
            contentBytes.AddRange(TLVFormat(0x02, rsaParameters.Exponent));
            //D
            contentBytes.AddRange(TLVFormat(0x02, rsaParameters.D));
            //P
            contentBytes.AddRange(TLVFormat(0x02, rsaParameters.P));
            //Q
            contentBytes.AddRange(TLVFormat(0x02, rsaParameters.Q));
            //DP
            contentBytes.AddRange(TLVFormat(0x02, rsaParameters.DP));
            //DQ
            contentBytes.AddRange(TLVFormat(0x02, rsaParameters.DQ));
            //InverseQ
            contentBytes.AddRange(TLVFormat(0x02, rsaParameters.InverseQ));

            //TLV without the first version
            var bodyTlvBytes = TLVFormat(0x30, contentBytes.ToArray());
            //TLV after SeqOid
            var bodyBytes = TLVFormat(0x04, bodyTlvBytes.ToArray());
            bodyBytes.InsertRange(0, SeqOID);
            bodyBytes.InsertRange(0, Version);

            //Key binary
            var keyBytes = TLVFormat(0x30, bodyBytes.ToArray());
            return Convert.ToBase64String(keyBytes.ToArray());
        }
#endif

        /// <summary>
        /// Convert PKCS8 encoded private key to PKCS1 encoded private key
        /// </summary>
        /// <param name="pkcs8Key">PKCS8 format key</param>
        /// <returns></returns>
        public static string PKCS8ToPKCS1(string pkcs8Key)
        {
#if NETSTANDARD2_0
            var rsaParams = ReadPrivateKeyInfo(pkcs8Key);
            return ExportPrivateKeyPKCS1(rsaParams);
#else
            using var rsa = RSA.Create();
            var pkcs8KeyBuffer = Convert.FromBase64String(pkcs8Key);
            rsa.ImportPkcs8PrivateKey(pkcs8KeyBuffer, out int bytesRead);
            var pkcs1KeyBuffer = rsa.ExportRSAPrivateKey();
            return Convert.ToBase64String(pkcs1KeyBuffer);
#endif
        }

        /// <summary>
        /// Convert PKCS1 encoded private key to PKCS8 encoded private key
        /// </summary>
        /// <param name="pkcs1Key">PKCS1 format key</param>
        /// <returns></returns>
        public static string PKCS1ToPKCS8(string pkcs1Key)
        {
#if NETSTANDARD2_0
            var rsaParams = ReadPrivateKeyInfo(pkcs1Key);
            return ExportPrivateKeyPKCS8(rsaParams);
#else
            using var rsa = RSA.Create();
            var pkcs1KeyBuffer = Convert.FromBase64String(pkcs1Key);
            rsa.ImportRSAPrivateKey(pkcs1KeyBuffer, out int bytesRead);
            var pkcs8KeyBuffer = rsa.ExportPkcs8PrivateKey();
            return Convert.ToBase64String(pkcs8KeyBuffer);
#endif
        }

#if NETSTANDARD2_0
        /// <summary>
        /// Read RSA parameters from public key
        /// </summary>
        public static RSAParameters ReadPublicKeyInfo(string publicKey)
        {
            var rsaKeyInfo = new RSAParameters();
            //Public key binary data
            var keyBytes = Convert.FromBase64String(publicKey);
            var keySpan = new Span<byte>(keyBytes);
            if (keySpan[0] != 0x30)
            {
                throw new ArgumentException("RSA public key header flag is incorrect, should be 0x30.");
            }
            var bodySpan = ReadContent(ReadTLV(keySpan));
            if (!SeqOID.SequenceEqual(bodySpan.Slice(0, 15).ToArray()))
            {
                throw new ArgumentException("RSA public key OID sequence is incorrect.");
            }
            //Content
            var contentSpan = bodySpan.Slice(15);
            if (contentSpan[0] != 0x03)
            {
                throw new ArgumentException("RSA public key content flag is incorrect, should be 0x03.");
            }
            //Fixed 0x00
            var fixedSpan = ReadTLV(contentSpan);
            if (fixedSpan[0] != 0x00)
            {
                throw new ArgumentException("RSA public key fixed content is incorrect, should be 0x00");
            }
            //Content
            var keyContentSpan = ReadTLV(fixedSpan.Slice(1));
            if (keyContentSpan[0] != 0x02)
            {
                throw new ArgumentException("RSA public key Modulus flag is incorrect, should be 0x02.");
            }

            var tlvs = SplitTLVs(keyContentSpan);
            //Modulus
            rsaKeyInfo.Modulus = ReadContent(tlvs[0]).ToArray();
            //Exponent
            rsaKeyInfo.Exponent = ReadContent(tlvs[1]).ToArray();
            return rsaKeyInfo;
        }

        /// <summary>
        /// Read RSA parameters from private key
        /// </summary>
        public static RSAParameters ReadPrivateKeyInfo(string privateKey)
        {
            var rsaKeyInfo = new RSAParameters();
            var keyBytes = Convert.FromBase64String(privateKey);
            var keySpan = new Span<byte>(keyBytes);
            if (keySpan[0] != 0x30)
            {
                throw new ArgumentException("RSA private key header flag is incorrect, should be 0x30.");
            }
            //Body
            var bodySpan = ReadTLV(keySpan);
            if (!Version.SequenceEqual(bodySpan.Slice(0, 3).ToArray()))
            {
                throw new ArgumentException("RSA private key first fixed version is incorrect.");
            }
            //Content
            var contentSpan = bodySpan.Slice(3);

            if (contentSpan[0] == 0x30)
            {
                //PKCS8
                if (!SeqOID.SequenceEqual(contentSpan.Slice(0, 15).ToArray()))
                {
                    throw new ArgumentException("RSA private key is in PKCS8 format, OID sequence is incorrect");
                }
                //Data after removing OID
                var itemSpan1 = contentSpan.Slice(15);
                var itemSpan2 = ReadTLV(itemSpan1);
                var secondVersionSpan = ReadTLV(itemSpan2);
                //Read second version
                if (!Version.SequenceEqual(secondVersionSpan.Slice(0, 3).ToArray()))
                {
                    throw new ArgumentException("RSA private key is in PKCS8 format, second fixed version is incorrect.");
                }
                //Content of both formats
                contentSpan = secondVersionSpan.Slice(3);
            }
            //Multiple parallel TLVs
            var tlvs = SplitTLVs(contentSpan);
            //Modulus
            rsaKeyInfo.Modulus = ReadContent(tlvs[0]).ToArray();
            //Exponent
            rsaKeyInfo.Exponent = ReadContent(tlvs[1]).ToArray();
            //D
            rsaKeyInfo.D = ReadContent(tlvs[2]).ToArray();
            //P
            rsaKeyInfo.P = ReadContent(tlvs[3]).ToArray();
            //Q
            rsaKeyInfo.Q = ReadContent(tlvs[4]).ToArray();
            //DP
            rsaKeyInfo.DP = ReadContent(tlvs[5]).ToArray();
            //DQ
            rsaKeyInfo.DQ = ReadContent(tlvs[6]).ToArray();
            //InverseQ
            rsaKeyInfo.InverseQ = ReadContent(tlvs[7]).ToArray();
            return rsaKeyInfo;
        }

        /// <summary>
        /// Get RSA private key format
        /// </summary>
        public static RSAKeyFormat GetKeyFormat(string privateKey)
        {
            var keyFormat = RSAKeyFormat.PKCS1;
            var keyBytes = Convert.FromBase64String(privateKey);
            var keySpan = new Span<byte>(keyBytes);
            if (keySpan[0] != 0x30)
            {
                return RSAKeyFormat.Unknow;
            }
            //Body
            var bodySpan = ReadTLV(keySpan);
            if (!Version.SequenceEqual(bodySpan.Slice(0, 3).ToArray()))
            {
                return RSAKeyFormat.Unknow;
            }
            //Content
            var contentSpan = bodySpan.Slice(3);
            if (contentSpan[0] == 0x30)
            {
                keyFormat = RSAKeyFormat.PKCS8;
            }
            return keyFormat;
        }
#endif

        #region RSA encryption/decryption, signing/verification

        /// <summary>
        /// Encrypt
        /// </summary>
        /// <param name="data">Original data</param>
        /// <param name="publicKey">Public key</param>
        /// <param name="encryptionPadding">Algorithm</param>
        /// <param name="subjectPublicKey">Whether to use SubjectPublicKey format (only valid in .NET Standard 2.1 and above)</param>
        /// <returns></returns>
        public static byte[] Encrypt(byte[] data, string publicKey, RSAEncryptionPadding? encryptionPadding = null, bool subjectPublicKey = true)
        {
#if NETSTANDARD2_0
            using var rsa = CreateRsaFromPublicKey(publicKey);
            encryptionPadding ??= RSAEncryptionPadding.Pkcs1;
            var encryptedData = rsa.Encrypt(data, encryptionPadding);
            return encryptedData;
#else
            using var rsa = RSA.Create();
            var publicKeyBuffer = Convert.FromBase64String(publicKey);

            if (subjectPublicKey)
            {
                rsa.ImportSubjectPublicKeyInfo(publicKeyBuffer, out int _);
            }
            else
            {
                rsa.ImportRSAPublicKey(publicKeyBuffer, out int _);
            }

            encryptionPadding ??= RSAEncryptionPadding.Pkcs1;
            var encryptedData = rsa.Encrypt(data, encryptionPadding);
            return encryptedData;
#endif
        }

        /// <summary>
        /// Encrypt
        /// </summary>
        /// <param name="data">Original data</param>
        /// <param name="publicKey">Public key</param>
        /// <param name="encryptionPadding">Algorithm</param>
        /// <param name="encode">Encoding</param>
        /// <param name="subjectPublicKey">Whether to use SubjectPublicKey format (only valid in .NET Standard 2.1 and above)</param>
        /// <returns></returns>
        public static string EncryptAsBase64(string data, string publicKey, RSAEncryptionPadding? encryptionPadding = null, string encode = "utf-8", bool subjectPublicKey = true)
        {
            var encryptedData = Encrypt(Encoding.GetEncoding(encode).GetBytes(data), publicKey, encryptionPadding, subjectPublicKey);
            return Convert.ToBase64String(encryptedData);
        }

        /// <summary>
        /// Decrypt
        /// </summary>
        /// <param name="data">Data to decrypt</param>
        /// <param name="privateKey">Private key</param>
        /// <param name="keyFormat">Key format</param>
        /// <param name="encryptionPadding">Algorithm</param>
        /// <returns></returns>
        public static byte[] Decrypt(byte[] data, string privateKey, RSAKeyFormat keyFormat = RSAKeyFormat.PKCS1, RSAEncryptionPadding? encryptionPadding = null)
        {
#if NETSTANDARD2_0
            using var rsa = CreateRsaFromPrivateKey(privateKey);
            encryptionPadding ??= RSAEncryptionPadding.Pkcs1;
            var decryptedData = rsa.Decrypt(data, encryptionPadding);
            return decryptedData;
#else
            using var rsa = RSA.Create();
            var privateKeyBuffer = Convert.FromBase64String(privateKey);
            if (keyFormat == RSAKeyFormat.PKCS1)
            {
                rsa.ImportRSAPrivateKey(privateKeyBuffer, out int _);
            }
            else
            {
                rsa.ImportPkcs8PrivateKey(privateKeyBuffer, out int _);
            }
            encryptionPadding ??= RSAEncryptionPadding.Pkcs1;
            var decryptedData = rsa.Decrypt(data, encryptionPadding);
            return decryptedData;
#endif
        }

        /// <summary>
        /// Decrypt
        /// </summary>
        /// <param name="data">Original data</param>
        /// <param name="privateKey">Private key</param>
        /// <param name="keyFormat">Key format</param>
        /// <param name="encryptionPadding">Algorithm</param>
        /// <param name="encode">Encoding</param>
        /// <returns></returns>
        public static string DecryptFromBase64(string data, string privateKey, RSAKeyFormat keyFormat = RSAKeyFormat.PKCS1, RSAEncryptionPadding? encryptionPadding = null, string encode = "utf-8")
        {
            var decryptedData = Decrypt(Convert.FromBase64String(data), privateKey, keyFormat, encryptionPadding);
            return Encoding.GetEncoding(encode).GetString(decryptedData);
        }

        /// <summary>
        /// Data signing
        /// </summary>
        /// <param name="data">Data to sign</param>
        /// <param name="privateKey">Private key</param>
        /// <param name="keyFormat">Format, default PKCS1</param>
        /// <param name="signaturePadding">Signature type</param>
        /// <param name="hashAlgorithmName">Hash algorithm</param>
        /// <returns></returns>
        public static byte[] SignData(byte[] data, string privateKey, RSAKeyFormat keyFormat = RSAKeyFormat.PKCS1, RSASignaturePadding? signaturePadding = null, HashAlgorithmName? hashAlgorithmName = null)
        {
#if NETSTANDARD2_0
            using var rsa = CreateRsaFromPrivateKey(privateKey);
            signaturePadding ??= RSASignaturePadding.Pkcs1;
            var algorithmName = hashAlgorithmName ?? HashAlgorithmName.SHA256;
            var signedData = rsa.SignData(data, algorithmName, signaturePadding);
            return signedData;
#else
            using var rsa = RSA.Create();
            var privateKeyBuffer = Convert.FromBase64String(privateKey);
            if (keyFormat == RSAKeyFormat.PKCS1)
            {
                rsa.ImportRSAPrivateKey(privateKeyBuffer, out int _);
            }
            else
            {
                rsa.ImportPkcs8PrivateKey(privateKeyBuffer, out int _);
            }

            signaturePadding ??= RSASignaturePadding.Pkcs1;
            var algorithmName = hashAlgorithmName ?? HashAlgorithmName.SHA256;
            var signedData = rsa.SignData(data, algorithmName, signaturePadding);
            return signedData;
#endif
        }

        /// <summary>
        /// Data signing
        /// </summary>
        /// <param name="data">Data to sign</param>
        /// <param name="privateKey">Private key</param>
        /// <param name="keyFormat">Format, default PKCS1</param>
        /// <param name="signaturePadding">Signature type</param>
        /// <param name="hashAlgorithmName">Hash algorithm</param>
        /// <param name="encode">Encoding</param>
        /// <returns></returns>
        public static string SignDataAsBase64(string data, string privateKey, RSAKeyFormat keyFormat = RSAKeyFormat.PKCS1, RSASignaturePadding? signaturePadding = null, HashAlgorithmName? hashAlgorithmName = null, string encode = "utf-8")
        {
            var signedData = SignData(Encoding.GetEncoding(encode).GetBytes(data), privateKey, keyFormat, signaturePadding, hashAlgorithmName);
            return Convert.ToBase64String(signedData);
        }

        /// <summary>
        /// Signature verification
        /// </summary>
        /// <param name="data">Original data</param>
        /// <param name="signature">Signed data</param>
        /// <param name="publicKey">Public key</param>
        /// <param name="subjectPublicKey">Whether to use SubjectPublicKey format (only valid in .NET Standard 2.1 and above)</param>
        /// <param name="signaturePadding">Signature algorithm</param>
        /// <param name="hashAlgorithmName">Hash algorithm</param>
        /// <returns></returns>
        public static bool VerifyData(byte[] data, byte[] signature, string publicKey, bool subjectPublicKey = true, RSASignaturePadding? signaturePadding = null, HashAlgorithmName? hashAlgorithmName = null)
        {
#if NETSTANDARD2_0
            using var rsa = CreateRsaFromPublicKey(publicKey);
            signaturePadding ??= RSASignaturePadding.Pkcs1;
            var algorithmName = hashAlgorithmName ?? HashAlgorithmName.SHA256;
            return rsa.VerifyData(data, signature, algorithmName, signaturePadding);
#else
            using var rsa = RSA.Create();
            var publicKeyBuffer = Convert.FromBase64String(publicKey);

            if (subjectPublicKey)
            {
                rsa.ImportSubjectPublicKeyInfo(publicKeyBuffer, out int _);
            }
            else
            {
                rsa.ImportRSAPublicKey(publicKeyBuffer, out int _);
            }

            var algorithmName = hashAlgorithmName ?? HashAlgorithmName.SHA256;
            signaturePadding ??= RSASignaturePadding.Pkcs1;
            return rsa.VerifyData(data, signature, algorithmName, signaturePadding);
#endif
        }

        /// <summary>
        /// Signature verification
        /// </summary>
        /// <param name="data">Source data</param>
        /// <param name="base64Signature">Signed data Base64 encoded</param>
        /// <param name="publicKey">Public key</param>
        /// <param name="subjectPublicKey">Whether to use SubjectPublicKey format (only valid in .NET Standard 2.1 and above)</param>
        /// <param name="signaturePadding">Signature algorithm</param>
        /// <param name="hashAlgorithmName">Hash algorithm</param>
        /// <param name="encode">Encoding</param>
        /// <returns></returns>
        public static bool VerifyBase64Data(string data, string base64Signature, string publicKey, bool subjectPublicKey = true, RSASignaturePadding? signaturePadding = null, HashAlgorithmName? hashAlgorithmName = null, string encode = "utf-8")
        {
            var dataBytes = Encoding.GetEncoding(encode).GetBytes(data);
            var signature = Convert.FromBase64String(base64Signature);
            return VerifyData(dataBytes, signature, publicKey, subjectPublicKey, signaturePadding, hashAlgorithmName);
        }

#if NETSTANDARD2_0
        /// <summary>
        /// Create RSA object from RSA public key
        /// </summary>
        private static RSA CreateRsaFromPublicKey(string publicKey)
        {
            var rsaParams = ReadPublicKeyInfo(publicKey);
            var rsa = RSA.Create();
            rsa.ImportParameters(rsaParams);
            return rsa;
        }

        /// <summary>
        /// Create RSA object from private key
        /// </summary>
        private static RSA CreateRsaFromPrivateKey(string privateKey)
        {
            RSAParameters rsaParams = ReadPrivateKeyInfo(privateKey);
            var rsa = RSA.Create();
            rsa.ImportParameters(rsaParams);
            return rsa;
        }
#endif

        #endregion

#if NETSTANDARD2_0
        /// <summary>
        /// TLV format (flag + length data occupancy bits + length value + data)
        /// </summary>
        /// <param name="flag">Flag</param>
        /// <param name="content"></param>
        /// <returns></returns>
        private static List<byte> TLVFormat(byte flag, byte[] content)
        {
            var tlvBytes = new List<byte>
            {
                flag
            };
            var contentLength = content.Length;
            //Check if 0x00 needs to be added
            if (content[0] >= 0x80)
            {
                contentLength++;
            }
            //If length is less than 0x80 (128, because byte max is 0-127), there's no length occupancy bit
            if (contentLength >= 0x80)
            {
                //Less than 256, 0xFF=255
                if (contentLength <= 0xFF)
                {
                    tlvBytes.Add(0x81);
                    tlvBytes.Add((byte)contentLength);
                }
                else
                {
                    tlvBytes.Add(0x82);
                    var lengthBytes = BitConverter.GetBytes((ushort)contentLength);
                    lengthBytes.Reverse();
                    tlvBytes.AddRange(lengthBytes);
                }
            }
            else
            {
                //Length less than 128, directly add 1 bit for length
                tlvBytes.Add((byte)contentLength);
            }
            //Check if 0x00 needs to be added
            if (content[0] >= 0x80)
            {
                tlvBytes.Add(0x00);
            }
            //Add content
            tlvBytes.AddRange(content);
            return tlvBytes;
        }

        /// <summary>
        /// Read data from TLV structure
        /// </summary>
        private static Span<byte> ReadTLV(Span<byte> tlvSpan)
        {
            //Length
            int length;
            Span<byte> contentSpan;
            //If this value is less than 0x80, there's no length occupancy bit
            if (tlvSpan[1] < 0x80)
            {
                length = tlvSpan[1];
                contentSpan = tlvSpan.Slice(2, length);
            }
            else if (tlvSpan[1] == 0x81)
            {
                length = tlvSpan[2];
                contentSpan = tlvSpan.Slice(3, length);
            }
            else if (tlvSpan[1] == 0x82)
            {
                var lengthSpan = tlvSpan.Slice(2, 2);
                lengthSpan.Reverse();
                length = BitConverter.ToUInt16(lengthSpan.ToArray(), 0);
                contentSpan = tlvSpan.Slice(4, length);
            }
            else
            {
                throw new ArgumentException("TLV length occupancy bit is incorrect, should be 0x81 or 0x82");
            }
            return contentSpan;
        }

        /// <summary>
        /// Split multiple TLV format data, return data list
        /// </summary>
        private static List<byte[]> SplitTLVs(Span<byte> tlvs)
        {
            var tlvList = new List<byte[]>();
            var tlvSpan = tlvs.Slice(0);
            var readLength = 0;
            while (readLength < tlvs.Length)
            {
                //Length
                int length;
                //Current item length
                int itemLength = 1;
                Span<byte> contentSpan;
                //If this value is less than 0x80, there's no length occupancy bit
                if (tlvSpan[1] < 0x80)
                {
                    length = tlvSpan[1];
                    contentSpan = tlvSpan.Slice(2, length);
                    itemLength += 1;
                }
                else if (tlvSpan[1] == 0x81)
                {
                    length = tlvSpan[2];
                    contentSpan = tlvSpan.Slice(3, length);
                    itemLength += 2;
                }
                else if (tlvSpan[1] == 0x82)
                {
                    //Length occupies 2 bytes
                    var lengthSpan = tlvSpan.Slice(2, 2);
                    lengthSpan.Reverse();
                    length = BitConverter.ToUInt16(lengthSpan.ToArray(), 0);
                    contentSpan = tlvSpan.Slice(4, length);
                    itemLength += 3;
                }
                else
                {
                    throw new ArgumentException("TLV length occupancy bit is incorrect, should be 0x81 or 0x82");
                }
                tlvList.Add(contentSpan.ToArray());
                itemLength += length;
                tlvSpan = tlvSpan.Slice(itemLength);
                readLength += itemLength;
            }
            return tlvList;
        }

        /// <summary>
        /// Read content data
        /// </summary>
        private static Span<byte> ReadContent(Span<byte> contentSpan)
        {
            if (contentSpan[0] == 0x00)
            {
                if (contentSpan[1] < 0x80)
                {
                    throw new ArgumentException("RSA content data first bit less than 0x80 does not need 0x00 padding");
                }
                return contentSpan.Slice(1);
            }
            else
            {
                return contentSpan.Slice(0);
            }
        }
#endif
    }
}