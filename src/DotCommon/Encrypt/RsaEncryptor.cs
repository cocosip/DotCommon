using System;
using System.Security.Cryptography;
using System.Text;

namespace DotCommon.Encrypt
{
    /// <summary>RSA加密解密,签名验签
    /// </summary>
    public class RsaEncryptor
    {
        private string PrivateKeyPem { get; set; }
        private string PublicKeyPem { get; set; }
        private string HashAlg { get; set; } = "RSA2";
        private RSASignaturePadding SignaturePadding { get; set; } = RSASignaturePadding.Pkcs1;
        private RSAEncryptionPadding EncryptionPadding { get; set; } = RSAEncryptionPadding.Pkcs1;


        public RsaEncryptor()
        {

        }
        public RsaEncryptor(string publicKeyPem, string privateKeyPem) : this(publicKeyPem, privateKeyPem, "RSA2")
        {
        }

        public RsaEncryptor(string publicKeyPem, string privateKeyPem, string hashAlg)
        {
            PublicKeyPem = publicKeyPem;
            PrivateKeyPem = privateKeyPem;
            HashAlg = hashAlg;
        }

        /// <summary>设置算法
        /// </summary>
        public RsaEncryptor SetHashAlg(string hashAlg)
        {
            HashAlg = hashAlg;
            return this;
        }

        /// <summary>设置RSAEncryptionPadding
        /// </summary>
        public RsaEncryptor SetSignaturePadding(RSASignaturePadding signaturePadding)
        {
            SignaturePadding = signaturePadding;
            return this;
        }


        #region 加载公钥和私钥
        public RsaEncryptor LoadPublicKey(string publicKeyPem)
        {
            PublicKeyPem = publicKeyPem;
            return this;
        }

        public RsaEncryptor LoadPrivateKey(string privateKeyPem)
        {
            PrivateKeyPem = privateKeyPem;
            return this;
        }
        #endregion

        #region 签名与验证签名

        /// <summary>对数据签名
        /// </summary>
        public string SignData(string data, string code = "utf-8")
        {
            var signedData = SignData(Encoding.GetEncoding(code).GetBytes(data));
            return Convert.ToBase64String(signedData);
        }

        /// <summary>对数据签名
        /// </summary>
        public byte[] SignData(byte[] data)
        {
            using (var rsa = CreateRsaFromPrivateKey(PrivateKeyPem))
            {
                var signedData = rsa.SignData(data, GetHashAlgorithmName(HashAlg), SignaturePadding);
                return signedData;
            }
        }

        /// <summary>验证签名
        /// </summary>
        public bool VerifyData(byte[] data, byte[] signature)
        {
            var rsa = CreateRsaFromPublicKey(PublicKeyPem);
            return rsa.VerifyData(data, signature, GetHashAlgorithmName(HashAlg), SignaturePadding);
        }

        /// <summary>验证签名
        /// </summary>
        public bool VerifyData(string data, string signature, string code = "utf-8")
        {
            return VerifyData(Encoding.GetEncoding(code).GetBytes(data), (Convert.FromBase64String(signature)));
        }
        #endregion

        #region 加密与解密

        /// <summary>加密
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns></returns>
        public string Encrypt(byte[] data)
        {
            using (var rsa = CreateRsaFromPublicKey(PublicKeyPem))
            {
                var encryptedData = rsa.Encrypt(data, EncryptionPadding);
                return Convert.ToBase64String(encryptedData);
            }
        }

        /// <summary>加密
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="code">编码</param>
        /// <returns></returns>
        public string Encrypt(string data, string code = "utf-8")
        {
            return Encrypt(Encoding.GetEncoding(code).GetBytes(data));
        }

        /// <summary>解密
        /// </summary>
        /// <param name="data">数据源</param>
        /// <param name="code">编码</param>
        /// <returns></returns>
        public string Decrypt(string data, string code = "utf-8")
        {
            return Decrypt(Convert.FromBase64String(data), code);
        }

        /// <summary>解密
        /// </summary>
        /// <param name="data">数据源</param>
        /// <param name="code">编码</param>
        /// <returns></returns>
        public string Decrypt(byte[] data, string code = "utf-8")
        {
            using (var rsa = CreateRsaFromPrivateKey(PrivateKeyPem))
            {
                return Encoding.GetEncoding(code).GetString(rsa.Decrypt(data, EncryptionPadding));
            }
        }

        #endregion


        private HashAlgorithmName GetHashAlgorithmName(string hashAlg)
        {
            if ("RSA2".Equals(hashAlg))
            {
                return HashAlgorithmName.SHA256;
            }
            return HashAlgorithmName.SHA1;
        }

        /// <summary>根据私钥生成RSA
        /// </summary>
        private static RSA CreateRsaFromPrivateKey(string privateKey)
        {
            RSAParameters rsaParams = RsaUtil.ReadPrivateKeyInfo(privateKey);
            var rsa = RSA.Create();
            rsa.ImportParameters(rsaParams);
            return rsa;

            #region 原方法
            //var privateKeyBits = Convert.FromBase64String(privateKey);
            //var rsa = RSA.Create();
            //var rsaParams = new RSAParameters();

            //using (var binr = new BinaryReader(new MemoryStream(privateKeyBits)))
            //{
            //    byte bt = 0;
            //    ushort twobytes = 0;
            //    twobytes = binr.ReadUInt16();
            //    if (twobytes == 0x8130)
            //    {
            //        binr.ReadByte();
            //    }
            //    else if (twobytes == 0x8230)
            //    {
            //        binr.ReadInt16();
            //    }
            //    else
            //    {
            //        throw new Exception("Unexpected value read binr.ReadUInt16()");
            //    }
            //    twobytes = binr.ReadUInt16();
            //    if (twobytes != 0x0102)
            //    {
            //        throw new Exception("Unexpected version");
            //    }
            //    bt = binr.ReadByte();
            //    if (bt != 0x00)
            //    {
            //        throw new Exception("Unexpected value read binr.ReadByte()");
            //    }
            //    rsaParams.Modulus = binr.ReadBytes(GetIntegerSize(binr));
            //    rsaParams.Exponent = binr.ReadBytes(GetIntegerSize(binr));
            //    rsaParams.D = binr.ReadBytes(GetIntegerSize(binr));
            //    rsaParams.P = binr.ReadBytes(GetIntegerSize(binr));
            //    rsaParams.Q = binr.ReadBytes(GetIntegerSize(binr));
            //    rsaParams.DP = binr.ReadBytes(GetIntegerSize(binr));
            //    rsaParams.DQ = binr.ReadBytes(GetIntegerSize(binr));
            //    rsaParams.InverseQ = binr.ReadBytes(GetIntegerSize(binr));
            //}
            //rsa.ImportParameters(rsaParams);
            //return rsa; 
            #endregion
        }

        /// <summary>根据公钥生成RSA
        /// </summary>
        private static RSA CreateRsaFromPublicKey(string publicKey)
        {
            var rsaParams = RsaUtil.ReadPublicKeyInfo(publicKey);
            var rsa = RSA.Create();
            rsa.ImportParameters(rsaParams);
            return rsa;

            #region 原方法
            ////1.2.840.113549.1.1.1
            //byte[] seqOid = { 0x30, 0x0D, 0x06, 0x09, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x01, 0x01, 0x05, 0x00 };
            //var x509Key = Convert.FromBase64String(publicKey);
            //using (var mem = new MemoryStream(x509Key))
            //{
            //    using (var binr = new BinaryReader(mem))
            //    {
            //        byte bt = 0;
            //        ushort twobytes = 0;

            //        twobytes = binr.ReadUInt16();
            //        if (twobytes == 0x8130)
            //        {
            //            binr.ReadByte();
            //        }
            //        else if (twobytes == 0x8230)
            //        {
            //            binr.ReadInt16();
            //        }
            //        else
            //        {
            //            return null;
            //        }
            //        var seq = binr.ReadBytes(15);
            //        if (!CompareByteArray(seq, seqOid))
            //        {
            //            return null;
            //        }

            //        twobytes = binr.ReadUInt16();
            //        if (twobytes == 0x8103)
            //        {
            //            binr.ReadByte();
            //        }
            //        else if (twobytes == 0x8203)
            //        {
            //            binr.ReadInt16();
            //        }
            //        else
            //        {
            //            return null;
            //        }
            //        bt = binr.ReadByte();
            //        if (bt != 0x00)
            //        {
            //            return null;
            //        }
            //        twobytes = binr.ReadUInt16();
            //        if (twobytes == 0x8130)
            //        {
            //            binr.ReadByte();
            //        }
            //        else if (twobytes == 0x8230)
            //        {
            //            binr.ReadInt16();
            //        }
            //        else
            //        {
            //            return null;
            //        }
            //        twobytes = binr.ReadUInt16();
            //        byte lowbyte = 0x00;
            //        byte highbyte = 0x00;

            //        if (twobytes == 0x8102)
            //        {
            //            lowbyte = binr.ReadByte();
            //        }
            //        else if (twobytes == 0x8202)
            //        {
            //            highbyte = binr.ReadByte();
            //            lowbyte = binr.ReadByte();
            //        }
            //        else
            //        {
            //            return null;
            //        }
            //        byte[] modint = { lowbyte, highbyte, 0x00, 0x00 };
            //        int modsize = BitConverter.ToInt32(modint, 0);

            //        int firstbyte = binr.PeekChar();
            //        if (firstbyte == 0x00)
            //        {
            //            binr.ReadByte();
            //            modsize -= 1;
            //        }
            //        byte[] modulus = binr.ReadBytes(modsize);
            //        if (binr.ReadByte() != 0x02)
            //        {
            //            return null;
            //        }
            //        int expbytes = (int)binr.ReadByte();
            //        byte[] exponent = binr.ReadBytes(expbytes);
            //        var rsa = RSA.Create();
            //        var rsaKeyInfo = new RSAParameters
            //        {
            //            Modulus = modulus,
            //            Exponent = exponent
            //        };
            //        rsa.ImportParameters(rsaKeyInfo);
            //        return rsa;
            //    }

            //} 
            #endregion
        }

        #region 原方法

        //private static int GetIntegerSize(BinaryReader binr)
        //{
        //    byte bt = 0;
        //    int count = 0;
        //    bt = binr.ReadByte();
        //    if (bt != 0x02)
        //    {
        //        return 0;
        //    }
        //    bt = binr.ReadByte();
        //    if (bt == 0x81)
        //    {
        //        count = binr.ReadByte();
        //    }
        //    else
        //    {
        //        if (bt == 0x82)
        //        {
        //            var highbyte = binr.ReadByte();
        //            var lowbyte = binr.ReadByte();
        //            byte[] modint = { lowbyte, highbyte, 0x00, 0x00 };
        //            count = BitConverter.ToInt32(modint, 0);
        //        }
        //        else
        //        {
        //            count = bt;
        //        }
        //    }

        //    while (binr.ReadByte() == 0x00)
        //    {
        //        count -= 1;
        //    }
        //    binr.BaseStream.Seek(-1, SeekOrigin.Current);
        //    return count;
        //}
        //private static bool CompareByteArray(byte[] a, byte[] b)
        //{
        //    if (a.Length != b.Length)
        //    {
        //        return false;
        //    }
        //    int i = 0;
        //    foreach (byte c in a)
        //    {
        //        if (c != b[i])
        //        {
        //            return false;
        //        }
        //        i++;
        //    }
        //    return true;
        //} 
        #endregion


    }
}
