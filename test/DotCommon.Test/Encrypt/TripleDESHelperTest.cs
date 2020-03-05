using DotCommon.Encrypt;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DotCommon.Test.Encrypt
{
    public class TripleDESHelperTest
    {
        [Fact]
        public void Encrypt_Decrypt_Test()
        {
            var source = "helloworld";
            var sourceBytes = Encoding.UTF8.GetBytes(source);
            var key = "MTIzNDU2Nzg4NzY1NDMyMQ=="; //1234567887654321
            var iv = "MTIzNDU2Nzg5MGFiY2RlZg==";//1234567890abcdef

            var keyBytes = Convert.FromBase64String(key);
            var ivBytes = Convert.FromBase64String(iv);

            var encrypted1 = TripleDESHelper.Encrypt(sourceBytes);
            var decrypted1 = Encoding.UTF8.GetString(TripleDESHelper.Decrypt(encrypted1));
            Assert.Equal(source, decrypted1);

 

        }
    }
}
