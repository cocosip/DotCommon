using Org.BouncyCastle.Asn1.GM;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Signers;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities.Encoders;
using System;
using System.Text;

namespace DotCommon.Crypto
{
    /// <summary>
    /// 国密SM2工具
    /// </summary>
    public static class SM2Util
    {

        static readonly string Sm2CurveName = "sm2p256v1";

        /// <summary>
        /// 生成密钥对
        /// </summary>
        /// <returns>私钥,公钥</returns>
        public static AsymmetricCipherKeyPair GetKeyPair()
        {
            var generator = new ECKeyPairGenerator();
            generator.Init(new ECKeyGenerationParameters(new ECDomainParameters(GMNamedCurves.GetByName(Sm2CurveName)), new SecureRandom()));
            return generator.GenerateKeyPair();
        }

        /// <summary>
        /// 生成密钥对
        /// </summary>
        /// <returns>私钥,公钥</returns>
        public static (string, string) GenerateKeyPair()
        {
            var k = GetKeyPair();
            var aPub = ((ECPublicKeyParameters)k.Public).Q.GetEncoded();
            var aPriv = ((ECPrivateKeyParameters)k.Private).D.ToByteArray();

            var publicKey = Hex.ToHexString(aPub);
            var privateKey = Hex.ToHexString(aPriv);
            return (privateKey, publicKey);
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="publicKey"></param>
        /// <param name="data"></param>
        /// <param name="c132"></param>
        /// <returns></returns>
        public static byte[] Encrypt(byte[] publicKey, byte[] data, bool c132 = false)
        {
            var engine = new SM2Engine();
            var x9ec = GMNamedCurves.GetByName(Sm2CurveName);
            var p = new ECPublicKeyParameters(x9ec.Curve.DecodePoint(publicKey), new ECDomainParameters(x9ec));
            engine.Init(true, new ParametersWithRandom(p));
            var v = engine.ProcessBlock(data, 0, data.Length);
            if (c132)
            {
                v = C123ToC132(v);
            }

            return v;
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="publicKey">公钥</param>
        /// <param name="data">数据</param>
        /// <param name="codeName">编码</param>
        /// <param name="c132"></param>
        /// <returns></returns>
        public static string Encrypt(string publicKey, string data, string codeName = "utf-8", bool c132 = false)
        {
            var pub = Hex.DecodeStrict(publicKey);
            var buffer = Encoding.GetEncoding(codeName).GetBytes(data);
            var v = Encrypt(pub, buffer, c132);
            return Hex.ToHexString(v);
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="privateKey">私钥</param>
        /// <param name="data">数据</param>
        /// <param name="c132"></param>
        /// <returns></returns>
        public static byte[] Decrypt(byte[] privateKey, byte[] data, bool c132 = false)
        {
            var engine = new SM2Engine();
            var x9ec = GMNamedCurves.GetByName(Sm2CurveName);
            var p = new ECPrivateKeyParameters(new BigInteger(privateKey), new ECDomainParameters(x9ec));
            engine.Init(false, p);

            if (c132)
            {
                data = C132ToC123(data);
            }

            var v = engine.ProcessBlock(data, 0, data.Length);
            return v;
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="privateKey">私钥</param>
        /// <param name="data">数据</param>
        /// <param name="codeName">编码</param>
        /// <param name="c132"></param>
        /// <returns></returns>
        public static string Decrypt(string privateKey, string data, string codeName = "utf-8", bool c132 = false)
        {
            var aPriv = Hex.DecodeStrict(privateKey);
            var buffer = Hex.DecodeStrict(data);
            var v = Decrypt(aPriv, buffer, c132);
            return Encoding.GetEncoding(codeName).GetString(v);
        }

        /// <summary>
        /// 签名
        /// </summary>
        /// <param name="privateKey">私钥</param>
        /// <param name="data">数据</param>
        /// <param name="id">Id</param>
        /// <returns></returns>
        public static byte[] Sign(byte[] privateKey, byte[] data, byte[] id = null)
        {
            var x9ec = GMNamedCurves.GetByName(Sm2CurveName);
            var p = new ECPrivateKeyParameters(new BigInteger(privateKey), new ECDomainParameters(x9ec));

            var signer = new SM2Signer();
            ICipherParameters cp;

            if (id != null)
            {
                cp = new ParametersWithID(new ParametersWithRandom(p), id);
            }
            else
            {
                cp = new ParametersWithRandom(p);
            }

            signer.Init(true, cp);
            signer.BlockUpdate(data, 0, data.Length);
            return signer.GenerateSignature();
        }

        /// <summary>
        /// 签名
        /// </summary>
        /// <param name="privateKey"></param>
        /// <param name="data"></param>
        /// <param name="codeName"></param>
        /// <returns></returns>
        public static string Sign(string privateKey, string data, string codeName = "utf-8")
        {
            var aPriv = Hex.Decode(Encoding.Default.GetBytes(privateKey));
            var buffer = Encoding.GetEncoding(codeName).GetBytes(data);
            var v = Sign(aPriv, buffer);
            return Hex.ToHexString(v);
        }

        /// <summary>
        /// 验签
        /// </summary>
        /// <param name="publicKey"></param>
        /// <param name="data"></param>
        /// <param name="signature"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool VerifySign(byte[] publicKey, byte[] data, byte[] signature, byte[] id = null)
        {
            var x9ec = GMNamedCurves.GetByName(Sm2CurveName);
            var p = new ECPublicKeyParameters(x9ec.Curve.DecodePoint(publicKey), new ECDomainParameters(x9ec));
            var signer = new SM2Signer();

            ICipherParameters cp;
            if (id != null)
            {
                cp = new ParametersWithID(p, id);
            }
            else
            {
                cp = p;
            }
            signer.Init(false, cp);
            signer.BlockUpdate(data, 0, data.Length);
            return signer.VerifySignature(signature);
        }

        /// <summary>
        /// 验签
        /// </summary>
        /// <param name="publicKey"></param>
        /// <param name="data"></param>
        /// <param name="signature"></param>
        /// <param name="codeName"></param>
        /// <returns></returns>
        public static bool VerifySign(string publicKey, string data, string signature, string codeName = "utf-8")
        {
            var aPub = Hex.DecodeStrict(publicKey);
            var s = Hex.DecodeStrict(signature);
            var buffer = Encoding.GetEncoding(codeName).GetBytes(data);
            return VerifySign(aPub, buffer, s);
        }


        /// <summary>
        /// C123->C132
        /// </summary>
        /// <param name="c1c2c3"></param>
        /// <returns></returns>
        public static byte[] C123ToC132(byte[] c1c2c3)
        {
            var x9ec = GMNamedCurves.GetByName(Sm2CurveName);
            var c1Len = (x9ec.Curve.FieldSize + 7) / 8 * 2 + 1;
            var c3Len = 32;
            var result = new byte[c1c2c3.Length];
            Array.Copy(c1c2c3, 0, result, 0, c1Len);
            Array.Copy(c1c2c3, c1c2c3.Length - c3Len, result, c1Len, c3Len);
            Array.Copy(c1c2c3, c1Len, result, c1Len + c3Len, c1c2c3.Length - c1Len - c3Len);

            return result;
        }

        /// <summary>
        /// C132->C123
        /// </summary>
        /// <param name="c1c3c2"></param>
        /// <returns></returns>
        public static byte[] C132ToC123(byte[] c1c3c2)
        {
            var x9ec = GMNamedCurves.GetByName(Sm2CurveName);
            var c1Len = (x9ec.Curve.FieldSize + 7) / 8 * 2 + 1;
            var c3Len = 32;
            var result = new byte[c1c3c2.Length];
            Array.Copy(c1c3c2, 0, result, 0, c1Len);
            Array.Copy(c1c3c2, c1Len + c3Len, result, c1Len, c1c3c2.Length - c1Len - c3Len);
            Array.Copy(c1c3c2, c1Len, result, c1c3c2.Length - c3Len, c3Len);

            return result;
        }


    }
}
