#if NET45
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace DotCommon.Encrypt
{
    public class RsaEncryptor
    {
        private string PrivateKeyPem { get; set; }
        private string PublicKeyPem { get; set; }
        private string HashAlg { get; set; } = "RSA2";
        public RsaEncryptor()
        {

        }
        public RsaEncryptor(string privateKeyPem, string publicKeyPem)
        {
            PrivateKeyPem = privateKeyPem;
            PublicKeyPem = publicKeyPem;
        }

        public RsaEncryptor(string privateKeyPem, string publicKeyPem, string hashAlg) : this(privateKeyPem, publicKeyPem)
        {
            HashAlg = hashAlg;
        }

        /// <summary>设置算法
        /// </summary>
        public RsaEncryptor SetHashAlg(string hashAlg)
        {
            HashAlg = hashAlg;
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
            return SignData(Encoding.GetEncoding(code).GetBytes(data));
        }

        /// <summary>对数据签名
        /// </summary>
        public string SignData(byte[] data)
        {
            RSACryptoServiceProvider rsaCsp = LoadCertificateString(PrivateKeyPem, HashAlg);
            var signatureBytes = rsaCsp.SignData(data, GetHashAlgorithmName(HashAlg));
            return Convert.ToBase64String(signatureBytes);
        }

        /// <summary>验证签名
        /// </summary>
        public bool VerifyData(byte[] data, byte[] signature)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.PersistKeyInCsp = false;
            RSACryptoServiceProviderExtension.LoadPublicKeyPEM(rsa, PublicKeyPem);
            if ("RSA2".Equals(HashAlg))
            {
                return rsa.VerifyData(data, GetHashAlgorithmName(HashAlg), signature);
            }
            else
            {
                SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();
                return rsa.VerifyData(data, sha1, signature);
            }
        }

        /// <summary>验证签名
        /// </summary>
        public bool VerifyData(string data, string signature, string code = "utf-8")
        {
            return VerifyData(Encoding.GetEncoding(code).GetBytes(data), (Convert.FromBase64String(signature)));
        }
        #endregion



        #region Utilities

        private string GetHashAlgorithmName(string hashAlg)
        {
            if ("RSA2".Equals(hashAlg))
            {
                return "SHA256";
            }
            return "SHA1";
        }
        private static RSACryptoServiceProvider LoadCertificateString(string strKey, string signType)
        {
            byte[] data = null;
            //读取带
            //ata = Encoding.Default.GetBytes(strKey);
            data = Convert.FromBase64String(strKey);
            //data = GetPem("RSA PRIVATE KEY", data);
            RSACryptoServiceProvider rsa = DecodeRSAPrivateKey(data, signType);
            return rsa;
        }

        private static RSACryptoServiceProvider DecodeRSAPrivateKey(byte[] privkey, string signType)
        {
            byte[] MODULUS, E, D, P, Q, DP, DQ, IQ;

            // --------- Set up stream to decode the asn.1 encoded RSA private key ------
            MemoryStream mem = new MemoryStream(privkey);
            BinaryReader binr = new BinaryReader(mem);  //wrap Memory Stream with BinaryReader for easy reading
            byte bt = 0;
            ushort twobytes = 0;
            int elems = 0;
            try
            {
                twobytes = binr.ReadUInt16();
                if (twobytes == 0x8130) //data read as little endian order (actual data order for Sequence is 30 81)
                    binr.ReadByte();    //advance 1 byte
                else if (twobytes == 0x8230)
                    binr.ReadInt16();    //advance 2 bytes
                else
                    return null;

                twobytes = binr.ReadUInt16();
                if (twobytes != 0x0102) //version number
                    return null;
                bt = binr.ReadByte();
                if (bt != 0x00)
                    return null;


                //------ all private key components are Integer sequences ----
                elems = GetIntegerSize(binr);
                MODULUS = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                E = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                D = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                P = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                Q = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                DP = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                DQ = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                IQ = binr.ReadBytes(elems);


                // ------- create RSACryptoServiceProvider instance and initialize with public key -----
                CspParameters CspParameters = new CspParameters();
                CspParameters.Flags = CspProviderFlags.UseMachineKeyStore;

                int bitLen = 1024;
                if ("RSA2".Equals(signType))
                {
                    bitLen = 2048;
                }

                RSACryptoServiceProvider RSA = new RSACryptoServiceProvider(bitLen, CspParameters);
                RSAParameters RSAparams = new RSAParameters();
                RSAparams.Modulus = MODULUS;
                RSAparams.Exponent = E;
                RSAparams.D = D;
                RSAparams.P = P;
                RSAparams.Q = Q;
                RSAparams.DP = DP;
                RSAparams.DQ = DQ;
                RSAparams.InverseQ = IQ;
                RSA.ImportParameters(RSAparams);
                return RSA;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                binr.Close();
            }
        }

        private static int GetIntegerSize(BinaryReader binr)
        {
            byte bt = 0;
            byte lowbyte = 0x00;
            byte highbyte = 0x00;
            int count = 0;
            bt = binr.ReadByte();
            if (bt != 0x02)		//expect integer
                return 0;
            bt = binr.ReadByte();

            if (bt == 0x81)
                count = binr.ReadByte();	// data size in next byte
            else
                if (bt == 0x82)
            {
                highbyte = binr.ReadByte(); // data size in next 2 bytes
                lowbyte = binr.ReadByte();
                byte[] modint = { lowbyte, highbyte, 0x00, 0x00 };
                count = BitConverter.ToInt32(modint, 0);
            }
            else
            {
                count = bt;     // we already have the data size
            }

            while (binr.ReadByte() == 0x00)
            {	//remove high order zeros in data
                count -= 1;
            }
            binr.BaseStream.Seek(-1, SeekOrigin.Current);		//last ReadByte wasn't a removed zero, so back up a byte
            return count;
        }
        #endregion

    }
}
#endif
