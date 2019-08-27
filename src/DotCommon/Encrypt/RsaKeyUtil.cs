using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace DotCommon.Encrypt
{
    /// <summary>RSA密钥工具类
    /// </summary>
    public static class RsaKeyUtil
    {
        /// <summary>固定内容 encoded OID sequence for PKCS #1 rsaEncryption szOID_RSA_RSA ="1.2.840.113549.1.1.1"
        /// </summary>
        public static byte[] SeqOID = { 0x30, 0x0D, 0x06, 0x09, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x01, 0x01, 0x05, 0x00 };

        /// <summary>固定版本号
        /// </summary>
        public static byte[] Version = { 0x02, 0x01, 0x00 };


        /// <summary>生成RSA密钥对
        /// </summary>
        public static (string, string) GenerateKeyPair(RSAPrivateKeyFormat format = RSAPrivateKeyFormat.PKCS1, int keySize = 1024)
        {
            using (var rsa = RSA.Create())
            {
                rsa.KeySize = keySize;

                var publicParameters = rsa.ExportParameters(false);
                var privateParameters = rsa.ExportParameters(true);
                string publicKey = ExportPublicKeyPem(publicParameters);
                string privateKey = format == RSAPrivateKeyFormat.PKCS1 ? ExportPrivateKeyPkcs1(privateParameters) : ExportPrivateKeyPkcs8(privateParameters);
                return (publicKey, privateKey);
            }
        }

        /// <summary>根据RSAParameters参数生成公钥
        /// </summary>
        public static string ExportPublicKeyPem(RSAParameters rsaParameters)
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
            return Convert.ToBase64String(keyBytes.ToArray(), Base64FormattingOptions.InsertLineBreaks);
        }

        /// <summary>根据RSAParameters参数生成PKCS1私钥
        /// </summary>
        public static string ExportPrivateKeyPkcs1(RSAParameters rsaParameters)
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

        /// <summary>根据RSAParameters参数生成PKCS8私钥
        /// </summary>
        public static string ExportPrivateKeyPkcs8(RSAParameters rsaParameters)
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

        /// <summary>TLV格式化(flag+长度数据占用位数+长度数值+数据)
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
                    tlvBytes.AddRange(BitConverter.GetBytes((ushort)contentLength));
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


    }

    /// <summary>RSA私钥编码格式
    /// </summary>
    public enum RSAPrivateKeyFormat
    {
        /// <summary>PKCS1
        /// </summary>
        PKCS1 = 1,

        /// <summary>PKCS8
        /// </summary>
        PKCS8 = 2
    }
}
