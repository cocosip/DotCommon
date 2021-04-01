using DotCommon.Encrypt;
using DotCommon.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotCommon.ConsoleTest
{
    public class RSASignTest
    {
        private RSAKeyPair _keyPair;
        private string _value = "helloworld@123";
        public RSASignTest()
        {

        }

        public void Run()
        {
            GenerateKeys();
            var privateSign = PrivateSign();
            PublicKeyVerifySign(privateSign);
            PrivateKeyVerifySign(privateSign);
        }

        private void GenerateKeys()
        {
            _keyPair = RSAHelper.GenerateKeyPair(RSAKeyFormat.PKCS8);
            Console.WriteLine("RSA生成公钥:-----@@{0}@@------", _keyPair.PublicKey);
            Console.WriteLine("RSA生成私钥:-----@@{0}@@------", _keyPair.PrivateKey);
        }


        private string PrivateSign()
        {
            Console.WriteLine("------------私钥签名 BEGIN---------");
            Console.WriteLine("待签名字符串:{0}", _value);
            var sign = RSAHelper.SignDataAsBase64(_value, _keyPair.PrivateKey, RSAKeyFormat.PKCS8);
            Console.WriteLine("签名成Base64:{0}", sign);
            Console.WriteLine("------------私钥签名 END---------");
            return sign;
        }

        private string PublicSign()
        {
            Console.WriteLine("------------公钥签名 BEGIN---------");
            Console.WriteLine("待签名字符串:{0}", _value);
            var sign = RSAHelper.SignDataAsBase64(_value, _keyPair.PublicKey, RSAKeyFormat.PKCS8);
            Console.WriteLine("签名成Base64:{0}", sign);
            Console.WriteLine("------------公钥签名 END---------");
            return sign;
        }

        private bool PublicKeyVerifySign(string sign)
        {
            Console.WriteLine("------------公钥验签 BEGIN---------");
            Console.WriteLine("数据:{0}", _value);
            bool result = RSAHelper.VerifyBase64Data(_value, sign, _keyPair.PublicKey);
            Console.WriteLine("验签结果:{0}.", result);
            Console.WriteLine("------------公钥验签 END---------");
            return result;
        }


        private bool PrivateKeyVerifySign(string sign)
        {
            Console.WriteLine("------------私钥验签 BEGIN---------");
            Console.WriteLine("数据:{0}", _value);
            bool result = RSAHelper.VerifyBase64Data(_value, sign, _keyPair.PrivateKey);
            Console.WriteLine("验签结果:{0}.", result);
            Console.WriteLine("------------私钥验签 END---------");
            return result;
        }

    }
}
