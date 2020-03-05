using DotCommon.Encrypt;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DotCommon.Test.Encrypt
{
    public class AESHelperTest
    {
        [Fact]
        public void EncryptTest()
        {
            var key = Encoding.UTF8.GetBytes("1234567890abcdef");
            var encryptedBytes = AESHelper.Encrypt(data: Encoding.UTF8.GetBytes("helloworld"), key: key);
            var decryptedBytes = AESHelper.Decrypt(encryptedBytes, key: key);
            Assert.Equal("helloworld", Encoding.UTF8.GetString(decryptedBytes));
        }

        /// <summary>与第三方加密结果比对
        /// </summary>
        [Fact]
        public void EncryptResultTest()
        {
            var str = "helloworld";
            var strBytes = Encoding.UTF8.GetBytes(str);
            var keyBytes = Convert.FromBase64String("MTIzNDU2Nzg4NzY1NDMyMQ=="); //1234567887654321
            var ivBytes = Convert.FromBase64String("MTIzNDU2Nzg5MGFiY2RlZg==");//1234567890abcdef

            var encryptedBytes = AESHelper.Encrypt(strBytes, keyBytes, ivBytes);

            var encryptedBase64 = Convert.ToBase64String(encryptedBytes);
         
            Assert.Equal("5vpVXOvT+drFQQSH3KXi6Q==", encryptedBase64);
 

        }
    }
}
