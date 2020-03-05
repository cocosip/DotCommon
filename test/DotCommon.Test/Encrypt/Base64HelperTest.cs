using DotCommon.Encrypt;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DotCommon.Test.Encrypt
{
    public class Base64HelperTest
    {

        [Fact]
        public void Encrypt_Decrypt_Test()
        {
            var source1 = "helloworld";

            var encrypted1 = Base64Helper.Base64Encode(source1, "utf-8");
            var decrypted1 = Base64Helper.Base64Decode(encrypted1, "utf-8");
            Assert.Equal(source1, decrypted1);


            var source2 = "abc1234";
            var encrypted2 = "YWJjMTIzNA==";
            Assert.Equal(encrypted2, Base64Helper.Base64Encode(source2));

            var encrypted3 = "YnJvdGhlcg==";
            var source3 = "brother";
            Assert.Equal(source3, Base64Helper.Base64Decode(encrypted3));

        }

        [Fact]
        public void FormatImage_Test()
        {
            var base64 = "YnJvdGhlcg==";
            var imageBase64 = Base64Helper.FormatImage(base64, "image/jpg");
            Assert.Equal("data:image/jpg;base64,YnJvdGhlcg==", imageBase64);

        }
    }
}
