#if Netstandard2_0
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace DotCommon.Encrypt
{
    /// <summary>
    /// RSA密码,密钥相关操作
    /// </summary>
    public static class RSAHelper
    {
        /// <summary>固定内容 encoded OID sequence for PKCS #1 rsaEncryption szOID_RSA_RSA ="1.2.840.113549.1.1.1"
        /// </summary>
        private static readonly byte[] SeqOID = { 0x30, 0x0D, 0x06, 0x09, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x01, 0x01, 0x05, 0x00 };

        /// <summary>固定版本号
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

        /// <summary>
        /// 生成RSA密钥对(Pem密钥格式)
        /// </summary>
        /// <param name="format">格式,PKCS1或者PKCS8</param>
        /// <param name="keySize">512,1024,1536,2048</param>
        /// <returns>公钥,私钥</returns>
        public static RSAKeyPair GenerateKeyPair(RSAKeyFormat format = RSAKeyFormat.PKCS1, int keySize = 1024)
        {
            using var rsa = RSA.Create();
            rsa.KeySize = keySize;

            var publicParameters = rsa.ExportParameters(false);
            var privateParameters = rsa.ExportParameters(true);
            string publicKey = ExportPublicKey(publicParameters);
            string privateKey = format == RSAKeyFormat.PKCS1 ? ExportPrivateKeyPKCS1(privateParameters) : ExportPrivateKeyPKCS8(privateParameters);
            return new RSAKeyPair(publicKey, privateKey);
        }

        /// <summary>
        /// 根据RSAParameters参数生成公钥
        /// </summary>
        /// <param name="rsaParameters">RSA参数</param>
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

            //内容,Modulus+Exponent
            var bodyBytes = TLVFormat(0x03, contentBytes.ToArray());
            bodyBytes.InsertRange(0, SeqOID);

            //密钥最终的二进制
            var keyBytes = TLVFormat(0x30, bodyBytes.ToArray());
            return Convert.ToBase64String(keyBytes.ToArray());
        }

        /// <summary>
        /// 根据RSAParameters参数生成PKCS1私钥
        /// </summary>
        /// <param name="rsaParameters">RSA参数</param>
        /// <returns></returns>
        public static string ExportPrivateKeyPKCS1(RSAParameters rsaParameters)
        {
            //密钥中,除了头部长度以外的数据
            var bodyBytes = new List<byte>();
            //版本号
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
            //密钥二进制
            var keyBytes = TLVFormat(0x30, bodyBytes.ToArray());
            return Convert.ToBase64String(keyBytes.ToArray());
        }

        /// <summary>
        /// 根据RSAParameters参数生成PKCS8私钥
        /// </summary>
        public static string ExportPrivateKeyPKCS8(RSAParameters rsaParameters)
        {
            //第二个Verison之后的数据
            var contentBytes = new List<byte>();
            //版本号
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

            //不包含第一个Version的tlv
            var bodyTlvBytes = TLVFormat(0x30, contentBytes.ToArray());
            //SeqOid之后的tlv
            var bodyBytes = TLVFormat(0x04, bodyTlvBytes.ToArray());
            bodyBytes.InsertRange(0, SeqOID);
            bodyBytes.InsertRange(0, Version);

            //密钥二进制
            var keyBytes = TLVFormat(0x30, bodyBytes.ToArray());
            return Convert.ToBase64String(keyBytes.ToArray());
        }

        /// <summary>
        /// 将PKCS8编码格式的私钥转换成PKCS1编码格式的私钥
        /// </summary>
        /// <param name="pkcs8Key">PKCS8格式密钥</param>
        /// <returns></returns>
        public static string PKCS8ToPKCS1(string pkcs8Key)
        {
            var rsaParams = ReadPrivateKeyInfo(pkcs8Key);
            return ExportPrivateKeyPKCS1(rsaParams);
        }

        /// <summary>
        /// 将PKCS1编码格式的私钥转换成PKCS8编码格式的私钥
        /// </summary>
        /// <param name="pkcs1Key">PKCS1格式密钥</param>
        /// <returns></returns>
        public static string PKCS1ToPKCS8(string pkcs1Key)
        {
            var rsaParams = ReadPrivateKeyInfo(pkcs1Key);
            return ExportPrivateKeyPKCS8(rsaParams);
        }

        /// <summary>
        /// 从公钥中读取RSA参数
        /// </summary>
        public static RSAParameters ReadPublicKeyInfo(string publicKey)
        {
            var rsaKeyInfo = new RSAParameters();
            //公钥二进制数据
            var keyBytes = Convert.FromBase64String(publicKey);
            var keySpan = new Span<byte>(keyBytes);
            if (keySpan[0] != 0x30)
            {
                throw new ArgumentException("RSA公钥头部标志位不正确,应该为0x30.");
            }
            var bodySpan = ReadContent(ReadTLV(keySpan));
            if (!SeqOID.SequenceEqual(bodySpan.Slice(0, 15).ToArray()))
            {
                throw new ArgumentException("RSA公钥OID sequence不正确.");
            }
            //内容
            var contentSpan = bodySpan.Slice(15);
            if (contentSpan[0] != 0x03)
            {
                throw new ArgumentException("RSA公钥内容志位不正确,应该为0x03.");
            }
            //固定0x00
            var fixedSpan = ReadTLV(contentSpan);
            if (fixedSpan[0] != 0x00)
            {
                throw new ArgumentException("RSA公钥固定内容不正确,应为0x00");
            }
            //内容
            var keyContentSpan = ReadTLV(fixedSpan.Slice(1));
            if (keyContentSpan[0] != 0x02)
            {
                throw new ArgumentException("RSA公钥Modulus标志不正确,应为0x02.");
            }

            var tlvs = SplitTLVs(keyContentSpan);
            //Modulus
            rsaKeyInfo.Modulus = ReadContent(tlvs[0]).ToArray();
            //Exponent
            rsaKeyInfo.Exponent = ReadContent(tlvs[1]).ToArray();
            return rsaKeyInfo;
        }

        /// <summary>
        /// 从私钥中读取RSA参数
        /// </summary>
        public static RSAParameters ReadPrivateKeyInfo(string privateKey)
        {
            var rsaKeyInfo = new RSAParameters();
            var keyBytes = Convert.FromBase64String(privateKey);
            var keySpan = new Span<byte>(keyBytes);
            if (keySpan[0] != 0x30)
            {
                throw new ArgumentException("RSA私钥头部标志位不正确,应该为0x30.");
            }
            //Body
            var bodySpan = ReadTLV(keySpan);
            if (!Version.SequenceEqual(bodySpan.Slice(0, 3).ToArray()))
            {
                throw new ArgumentException("RSA私钥第一个固定版本不正确.");
            }
            //内容
            var contentSpan = bodySpan.Slice(3);

            if (contentSpan[0] == 0x30)
            {
                //PKCS8
                if (!SeqOID.SequenceEqual(contentSpan.Slice(0, 15).ToArray()))
                {
                    throw new ArgumentException("RSA私钥为PKCS8格式,OID sequence不正确");
                }
                //去除了OID之后的数据
                var itemSpan1 = contentSpan.Slice(15);
                var itemSpan2 = ReadTLV(itemSpan1);
                var secondVersionSpan = ReadTLV(itemSpan2);
                //第二个版本读取
                if (!Version.SequenceEqual(secondVersionSpan.Slice(0, 3).ToArray()))
                {
                    throw new ArgumentException("RSA私钥为PKCS8格式,第二个固定版本不正确.");
                }
                //两种格式私钥的内容
                contentSpan = secondVersionSpan.Slice(3);
            }
            //多个并列tlv
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
        /// 获取RSA私钥的格式
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
            //内容
            var contentSpan = bodySpan.Slice(3);
            if (contentSpan[0] == 0x30)
            {
                keyFormat = RSAKeyFormat.PKCS8;
            }
            return keyFormat;
        }


        #region RSA加密解密,签名解签

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="data">原数据</param>
        /// <param name="publicKey">公钥</param>
        /// <param name="encryptionPadding">算法</param>
        /// <returns></returns>
        public static byte[] Encrypt(byte[] data, string publicKey, RSAEncryptionPadding encryptionPadding = null)
        {
            using var rsa = CreateRsaFromPublicKey(publicKey);
            encryptionPadding ??= RSAEncryptionPadding.Pkcs1;
            var encryptedData = rsa.Encrypt(data, encryptionPadding);
            return encryptedData;
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="data">原数据</param>
        /// <param name="publicKey">公钥</param>
        /// <param name="encryptionPadding">算法</param>
        /// <param name="encode">编码</param>
        /// <returns></returns>
        public static string EncryptAsBase64(string data, string publicKey, RSAEncryptionPadding encryptionPadding = null, string encode = "utf-8")
        {
            var encryptedData = Encrypt(Encoding.GetEncoding(encode).GetBytes(data), publicKey, encryptionPadding);
            return Convert.ToBase64String(encryptedData);
        }


        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="data">待解密数据</param>
        /// <param name="privateKey"></param>
        /// <param name="encryptionPadding">算法</param>
        /// <returns></returns>
        public static byte[] Decrypt(byte[] data, string privateKey, RSAEncryptionPadding encryptionPadding = null)
        {
            using var rsa = CreateRsaFromPrivateKey(privateKey);
            encryptionPadding ??= RSAEncryptionPadding.Pkcs1;
            var decryptedData = rsa.Decrypt(data, encryptionPadding);
            return decryptedData;
        }


        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="data">原数据</param>
        /// <param name="privateKey">私钥</param>
        /// <param name="encryptionPadding">算法</param>
        /// <param name="encode">编码</param>
        /// <returns></returns>
        public static string DecryptFromBase64(string data, string privateKey, RSAEncryptionPadding encryptionPadding = null, string encode = "utf-8")
        {
            var decryptedData = Decrypt(Convert.FromBase64String(data), privateKey, encryptionPadding);
            return Encoding.GetEncoding(encode).GetString(decryptedData);
        }


        /// <summary>
        /// 数据签名
        /// </summary>
        /// <param name="data">待签名数据</param>
        /// <param name="privateKey">私钥</param>
        /// <param name="signaturePadding">签名类型</param>
        /// <param name="hashAlgorithmName">哈希算法</param>
        /// <returns></returns>

        public static byte[] SignData(byte[] data, string privateKey, RSASignaturePadding signaturePadding = null, string hashAlgorithmName = "SHA256")
        {
            using var rsa = CreateRsaFromPrivateKey(privateKey);
            signaturePadding ??= RSASignaturePadding.Pkcs1;
            var signedData = rsa.SignData(data, HashAlgorithmNameDict[hashAlgorithmName], signaturePadding);
            return signedData;
        }
        /// <summary>
        /// 数据签名
        /// </summary>
        /// <param name="data">待签名数据</param>
        /// <param name="privateKey">私钥</param>
        /// <param name="signaturePadding">签名类型</param>
        /// <param name="hashAlgorithmName">哈希算法</param>
        /// <param name="encode">编码</param>
        /// <returns></returns>
        public static string SignDataAsBase64(string data, string privateKey, RSASignaturePadding signaturePadding = null, string hashAlgorithmName = "SHA256", string encode = "utf-8")
        {
            var signedData = SignData(Encoding.GetEncoding(encode).GetBytes(data), privateKey, signaturePadding, hashAlgorithmName);
            return Convert.ToBase64String(signedData);
        }

        /// <summary>
        /// 签名校验
        /// </summary>
        /// <param name="data">原数据</param>
        /// <param name="signature">签名后数据</param>
        /// <param name="publicKey">公寓奥</param>
        /// <param name="signaturePadding">签名算法</param>
        /// <param name="hashAlgorithmName">哈希算法</param>
        /// <returns></returns>
        public static bool VerifyData(byte[] data, byte[] signature, string publicKey, RSASignaturePadding signaturePadding = null, string hashAlgorithmName = "SHA256")
        {
            using var rsa = CreateRsaFromPublicKey(publicKey);
            signaturePadding ??= RSASignaturePadding.Pkcs1;
            return rsa.VerifyData(data, signature, HashAlgorithmNameDict[hashAlgorithmName], signaturePadding);
        }

        /// <summary>
        /// 签名校验
        /// </summary>
        /// <param name="data">源数据</param>
        /// <param name="base64Signature">签名后数据Base64编码</param>
        /// <param name="publicKey">公寓奥</param>
        /// <param name="signaturePadding">签名算法</param>
        /// <param name="hashAlgorithmName">哈希算法</param>
        /// <param name="encode">编码</param>
        /// <returns></returns>
        public static bool VerifyBase64Data(string data, string base64Signature, string publicKey, RSASignaturePadding signaturePadding = null, string hashAlgorithmName = "SHA256", string encode = "utf-8")
        {
            var dataBytes = Encoding.GetEncoding(encode).GetBytes(data);
            var signature = Convert.FromBase64String(base64Signature);
            return VerifyData(dataBytes, signature, publicKey, signaturePadding, hashAlgorithmName);
        }

        /// <summary>
        /// 根据RSA公钥生成RSA对象
        /// </summary>
        private static RSA CreateRsaFromPublicKey(string publicKey)
        {
            var rsaParams = ReadPublicKeyInfo(publicKey);
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

        /// <summary>
        /// 根据私钥生成RSA对象
        /// </summary>
        private static RSA CreateRsaFromPrivateKey(string privateKey)
        {
            RSAParameters rsaParams = ReadPrivateKeyInfo(privateKey);
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

        #endregion

        /// <summary>
        /// TLV格式化(flag+长度数据占用位数+长度数值+数据)
        /// </summary>
        /// <param name="flag">标志</param>
        /// <param name="content"></param>
        /// <returns></returns>
        private static List<byte> TLVFormat(byte flag, byte[] content)
        {
            var tlvBytes = new List<byte>
            {
                flag
            };
            var contentLength = content.Length;
            //判断是否需要添加0x00
            if (content[0] >= 0x80)
            {
                contentLength++;
            }
            //长度小于0x80(128,因为byte最大表示0-127),数据长度占用位没有了
            if (contentLength >= 0x80)
            {
                //小于256,0xFF=255
                if (contentLength <= 0xFF)
                {
                    tlvBytes.Add(0x81);
                    tlvBytes.Add((byte)contentLength);
                }
                else
                {
                    tlvBytes.Add(0x82);
                    var lengthBytes = BitConverter.GetBytes((ushort)contentLength).Reverse();
                    tlvBytes.AddRange(lengthBytes);
                }
            }
            else
            {
                //长度小于128,直接添加1位的长度
                tlvBytes.Add((byte)contentLength);
            }
            //判断是否需要添加0x00
            if (content[0] >= 0x80)
            {
                tlvBytes.Add(0x00);
            }
            //添加内容
            tlvBytes.AddRange(content);
            return tlvBytes;
        }

        /// <summary>
        /// 读取TLV结构中的数据
        /// </summary>
        private static Span<byte> ReadTLV(Span<byte> tlvSpan)
        {
            //长度
            int length;
            Span<byte> contentSpan;
            //如果该值小于0x80,代表没有长度占用位
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
                throw new ArgumentException("TLV长度占用位不正确,应为0x81或者0x82");
            }
            return contentSpan;
        }

        /// <summary>
        /// 分割多个TLV格式数据,返回数据列表
        /// </summary>
        private static List<byte[]> SplitTLVs(Span<byte> tlvs)
        {
            var tlvList = new List<byte[]>();
            var tlvSpan = tlvs.Slice(0);
            var readLength = 0;
            while (readLength < tlvs.Length)
            {
                //长度
                int length;
                //当前Item的长度
                int itemLength = 1;
                Span<byte> contentSpan;
                //如果该值小于0x80,代表没有长度占用位
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
                    //长度占用2个字节
                    var lengthSpan = tlvSpan.Slice(2, 2);
                    lengthSpan.Reverse();
                    length = BitConverter.ToUInt16(lengthSpan.ToArray(), 0);
                    contentSpan = tlvSpan.Slice(4, length);
                    itemLength += 3;
                }
                else
                {
                    throw new ArgumentException("TLV长度占用位不正确,应为0x81或者0x82");
                }
                tlvList.Add(contentSpan.ToArray());
                itemLength += length;
                tlvSpan = tlvSpan.Slice(itemLength);
                readLength += itemLength;
            }
            return tlvList;
        }

        /// <summary>
        /// 读取内容数据
        /// </summary>
        private static Span<byte> ReadContent(Span<byte> contentSpan)
        {
            if (contentSpan[0] == 0x00)
            {
                if (contentSpan[1] < 0x80)
                {
                    throw new ArgumentException("RSA读取内容数据首位小于0x80不需要补0x00");
                }
                return contentSpan.Slice(1);
            }
            else
            {
                return contentSpan.Slice(0);
            }
        }
    }

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
#endif