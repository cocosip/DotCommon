using DotCommon.Encrypt;
using Xunit;

namespace DotCommon.Test.Encrypt
{
    public class RSAHelperTest
    {
        /// <summary>
        /// 生成密钥对
        /// </summary>
        [Fact]
        public void GenerateKeyPair_Test()
        {
            //Default pkcs1 keyPairs
            var keyPairs = RSAHelper.GenerateKeyPair();
            //pkcs1私钥转换成pkcs8
            var pkcs8 = RSAHelper.PKCS1ToPKCS8(keyPairs.PrivateKey);

            Assert.NotEmpty(keyPairs.PrivateKey);
            Assert.NotEmpty(keyPairs.PublicKey);
            Assert.NotEqual(keyPairs.PrivateKey, pkcs8);
        }

        [Fact]
        public void Encrypt_Dcrypt_Test()
        {
            var keypair = RSAHelper.GenerateKeyPair();

            var encrypted = RSAHelper.EncryptAsBase64("hello", keypair.PublicKey);
            var decrypted = RSAHelper.DecryptFromBase64(encrypted, keypair.PrivateKey);
            Assert.Equal("hello", decrypted);

            //pkcs8密钥
            var pkcs8 = RSAHelper.PKCS1ToPKCS8(keypair.PrivateKey);

            var decrypted2 = RSAHelper.DecryptFromBase64(encrypted, pkcs8, keyFormat: RSAKeyFormat.PKCS8);
            Assert.Equal("hello", decrypted2);

        }


        [Fact]
        public void Sign_Verify_Test()
        {
            var keypair = RSAHelper.GenerateKeyPair();
            var signature = RSAHelper.SignDataAsBase64("csharpcode", keypair.PrivateKey);
            var verify = RSAHelper.VerifyBase64Data("csharpcode", signature, keypair.PublicKey);
            Assert.True(verify);
            
            var pkcs8 = RSAHelper.PKCS1ToPKCS8(keypair.PrivateKey);
            var signature2 = RSAHelper.SignDataAsBase64("csharpcode", pkcs8, RSAKeyFormat.PKCS8);
            Assert.Equal(signature, signature2);
        }

        [Fact]
        public void PKCS1_To_PKCS8_Test()
        {
            var pkcs1 = @"MIIEowIBAAKCAQEAmwD/7sMs82/PR32JDRmrZKpP2j6V9WeVBcNS3jvJJPRhW1dm8JvnBdnx1vv1xBUlmXIEXMc4D0T2VmZSsFoouCxhX3xrcQsAo129LSmteFo0p39R2RhyXXm939LU0xpoARGAFGf4LLzgGqjhOwq4HjZcYm4tR5xnUZZuKKexwK4CpCfoDxc8njBMvcWQxoe01FFw5sZQbPRKmpYGMUWn/1b6YsJ5XwOXCGmGurtwrdyioRi6viy7sya7EECOiC/3/Xr6mLAi4pMfMra6NyOZRmRbHcr7YsOU/FSrAMbkmIkgyK81Xq/RXxwmmsr19d+4u+M/sX8sKbDlMAXxPEyJbwIDAQABAoIBAGsX5bwxXX1UYwIh2e8TLTf/8+v2EeXcCzpQGZEx/Wdq8VkKqjTTgmqoik7fBo7TjYbXH+x6OGFUZF6Nk9QEdt3Iy/4NGK6hy28T9QCfKxtcN8UCUqqMGXg8BNP/9stkEzepv7RZoT5HwQ6qX/NLFKrLJEldlitEBJ56MOIl+soEVb4pyEiW0+QXRamE7dUcu5pHHONWJVhRIJ+iNqeU77YNtFZ5daWJfnVWCf6yZDa0swWtnkFCci0dzpM6UEXMgEm/PP44UWudGVVX7XepWKTKrb50eKC6exM9hpXLqstbVNofuNV4h1RaexO7hvsgkICrHSHxmLHXQoSdotOjegECgYEA+a8Algb6zvOzsEvXKNjnUdypQi/No1bmZIhdRaYaEyWZN6lOD+7vkxhEmMemkI2P3PZuXK/EzfDXv8R/MOQjZqmqQaDX2jguOV4baRH1eSR6cfkhpd1KVRLOHg9fvo9VKr/7+KvkcX2fgHb9A4ILytNrHD84+4bNJaxK6X7dwbcCgYEAnuzVgjuti4vZL7VapO54q79gOg20a7FWIA0j0gmRIEbZkDl3mZFfQUawPZ4R9t7Y7dzGv9lkj/SDePApAy5KC68ARBvoemVhy0/lS/3KtX3NOIPP65Bm0dYHN/ucsnrJCXOwDebNBVk+sAKRjNQ3Ac1bNDKwPRmpvS+aPer9FgkCgYAEeAplpQhWLex4alMWixNQ1sc8xQhENSj4gaxRs9BhqVmdsm0QQfGNy3Bm36PukGoTxWFiTU8TdI0YBJdWM68ihkTi2dMjN8A0DKgm8EhZe7qpUZE1m8lZznVb+mB3U69tjXQgFkuHcH0rWnXa2zgE0FXpcoQ9lEMVuoi4tymW7wKBgGAZ+uoryD/QK19MHSoLAnFo4nl4fBd7PwdWqsiB/H42Ga268nWskJtacYxxH4/XJfqAPLcacFMUmsPxBfvka+YwxspozeXllINrJs8TAxdIoWaBXqOlyGcvM2JJBnJvCU5r5JQjcuq/EsdAZl9wnGq8kWA5HpV0BU5fkLfMd4pxAoGBALX3uSZp/8D959AnBm8Dqzh/EEib8XaAqikCCh0GVNs2JET/G+KUpC6ld+XjAZ7dUS7QmOldKhbIZGXcQbDfbKU88VMi2Hh/vK3/R/GFMpFE+SOW3y7+PtMcHixMPqsOi6HCwK+2SoRhSSzA2yd2bwgQjr3ZKI/63Lb4P2WJ21jK";

            var pkcs8 = RSAHelper.PKCS1ToPKCS8(pkcs1);

            var expected = @"MIIEvQIBADANBgkqhkiG9w0BAQEFAASCBKcwggSjAgEAAoIBAQCbAP/uwyzzb89HfYkNGatkqk/aPpX1Z5UFw1LeO8kk9GFbV2bwm+cF2fHW+/XEFSWZcgRcxzgPRPZWZlKwWii4LGFffGtxCwCjXb0tKa14WjSnf1HZGHJdeb3f0tTTGmgBEYAUZ/gsvOAaqOE7CrgeNlxibi1HnGdRlm4op7HArgKkJ+gPFzyeMEy9xZDGh7TUUXDmxlBs9EqalgYxRaf/VvpiwnlfA5cIaYa6u3Ct3KKhGLq+LLuzJrsQQI6IL/f9evqYsCLikx8ytro3I5lGZFsdyvtiw5T8VKsAxuSYiSDIrzVer9FfHCaayvX137i74z+xfywpsOUwBfE8TIlvAgMBAAECggEAaxflvDFdfVRjAiHZ7xMtN//z6/YR5dwLOlAZkTH9Z2rxWQqqNNOCaqiKTt8GjtONhtcf7Ho4YVRkXo2T1AR23cjL/g0YrqHLbxP1AJ8rG1w3xQJSqowZeDwE0//2y2QTN6m/tFmhPkfBDqpf80sUqsskSV2WK0QEnnow4iX6ygRVvinISJbT5BdFqYTt1Ry7mkcc41YlWFEgn6I2p5Tvtg20Vnl1pYl+dVYJ/rJkNrSzBa2eQUJyLR3OkzpQRcyASb88/jhRa50ZVVftd6lYpMqtvnR4oLp7Ez2Glcuqy1tU2h+41XiHVFp7E7uG+yCQgKsdIfGYsddChJ2i06N6AQKBgQD5rwCWBvrO87OwS9co2OdR3KlCL82jVuZkiF1FphoTJZk3qU4P7u+TGESYx6aQjY/c9m5cr8TN8Ne/xH8w5CNmqapBoNfaOC45XhtpEfV5JHpx+SGl3UpVEs4eD1++j1Uqv/v4q+RxfZ+Adv0DggvK02scPzj7hs0lrErpft3BtwKBgQCe7NWCO62Li9kvtVqk7nirv2A6DbRrsVYgDSPSCZEgRtmQOXeZkV9BRrA9nhH23tjt3Ma/2WSP9IN48CkDLkoLrwBEG+h6ZWHLT+VL/cq1fc04g8/rkGbR1gc3+5yyeskJc7AN5s0FWT6wApGM1DcBzVs0MrA9Gam9L5o96v0WCQKBgAR4CmWlCFYt7HhqUxaLE1DWxzzFCEQ1KPiBrFGz0GGpWZ2ybRBB8Y3LcGbfo+6QahPFYWJNTxN0jRgEl1YzryKGROLZ0yM3wDQMqCbwSFl7uqlRkTWbyVnOdVv6YHdTr22NdCAWS4dwfStaddrbOATQVelyhD2UQxW6iLi3KZbvAoGAYBn66ivIP9ArX0wdKgsCcWjieXh8F3s/B1aqyIH8fjYZrbrydayQm1pxjHEfj9cl+oA8txpwUxSaw/EF++Rr5jDGymjN5eWUg2smzxMDF0ihZoFeo6XIZy8zYkkGcm8JTmvklCNy6r8Sx0BmX3CcaryRYDkelXQFTl+Qt8x3inECgYEAtfe5Jmn/wP3n0CcGbwOrOH8QSJvxdoCqKQIKHQZU2zYkRP8b4pSkLqV35eMBnt1RLtCY6V0qFshkZdxBsN9spTzxUyLYeH+8rf9H8YUykUT5I5bfLv4+0xweLEw+qw6LocLAr7ZKhGFJLMDbJ3ZvCBCOvdkoj/rctvg/ZYnbWMo=";

            Assert.Equal(expected, pkcs8);
        }

        [Fact]
        public void PKCS8_To_PKCS1_Test()
        {
            var pkcs8 = @"MIIEvQIBADANBgkqhkiG9w0BAQEFAASCBKcwggSjAgEAAoIBAQCakPfEGSh1yGUK8tK4ybuknGMFbELnVTSf8mk/A08O516jrfauVZzXRBYNyhA2iKjEqql5UVnkMLp5iNNi0z9vnitCu1qyQwbusX56/Nu44xWhiqtYSy9D4Dp8OW9HfI/Vd/BSpJfbqQf/dJOjfWw+RjH3s/+4e6VdzHSlRgBJ9peSvO4r0jyAfShxBfiPiROlmATs9DIGJ7ppkR09z9WNwq1Q8n0bIae5SYEu7NtJOMpBTQZlcXsHujlFNdwrsI0Gdq4qr/6wKEeLNwnMDt5qJwmLD0CJFEt6PwYSfl2CJv3AD4gvGvxuYml2Zz75Ld4/GO4PGzFHTCbD6ulEX+AXAgMBAAECggEALlKN/rYvewUgyyQ05O6Iju/TeVeVwXC2wczLn/fk5KmQomtLh7netGESXvWU4FMaiT5AZjxsPUghSrUn7PE0jMYRXwF5T/1ogXauWjuXuhRMckp1yZNWyffUb1BjOkBpdudoHcx4OnuxCu0sJg47t3ogFYVbeTWNPMw+lR2ijoeialeMBAUJ9KM8Tq8oe1C/UCwjkjU+0UELAGwymmESUKxnSrO4NnslnF1lwO8RX41BsRE3CoUWps48pp33mi/5lu99rFOXtRf2LD8mODypkucpECbdh8y++cNp1NNZ2VDOTwPTCV7w8/4Qqk1uh1XxYjuWC91CilbaaRA2sPU9+QKBgQDK2smAdym/YThcGHM0hFWtRk/3ppaOcysIl7mdddt3CfuItiPsSXkpqQNmPp5/L8HqzNjn6M2NpkPS3PoRsV6mbBlhTd8yez9prnmFetXSUbGxGNi9eg5kya2IlMCLYNFVLgBS/l/M/QJSjWgSP1L03Oh/x60jPqzC/8IxwN9R3QKBgQDDD4ois26oS3o5owRoH0HOqG9nm9TzlUhrKBJmNsYGQIoUT5Z3+UAq3HNiN5P/B2dzVX/Id1jMGzyqqvxGCajuc1pOupJqrXpjKGlJC/zsjxwnkJHpA8ITSQjVnFRutiXXfsEe4v835xxh7cdq6f8VpQPCf6QxTw2RrEXP1TMsgwKBgQC2cPuVPJchdpC3042ZiAN+aaR2OS9jQpVLjxnzUhJmB2ZgWGAKnTIJk+ZoyJzICu2+/Sl/udNVOFn7hZ6q1vkneEWDTAQXZN1tcOv+brFsDOlhO9WC5AmaAADAu/hH4nWixFKqUflbZZn3IRKehrFXZG7+rVk8P5wlbNz83lh7SQKBgDbJG4wph2//WwHrNmakONB5YGjiTMppaGCobZqF0dKZt+6CeAmUWSBwsHuqjoe3paewIObGFoulLsHkqlxlhCwM/BVWs58AwHovNHsOwegTPd4i7chbhNvzzGZqtRAvWquO72DkcrhZ0g/zP+pYdIu4wDiVryEmSfJbR3RbNjdPAoGANP2xlrVUrXCJbiAsWOoQ3V+HjoXFmdQAoAyG3UAOYT90a73KqyK3n692F9r60kFkBO9+EXAFwGAvHli6c+cE1mQWhROAT2pZZFwax+MvIvhLQ2aYBChCO5IclH8hGY0oZnHOkHYlsgghwajW8B1ReqpkwBz0AnMb8WCvIjvLpYo=";

            var pkcs1 = RSAHelper.PKCS8ToPKCS1(pkcs8);

            var expected = @"MIIEowIBAAKCAQEAmpD3xBkodchlCvLSuMm7pJxjBWxC51U0n/JpPwNPDudeo632rlWc10QWDcoQNoioxKqpeVFZ5DC6eYjTYtM/b54rQrtaskMG7rF+evzbuOMVoYqrWEsvQ+A6fDlvR3yP1XfwUqSX26kH/3STo31sPkYx97P/uHulXcx0pUYASfaXkrzuK9I8gH0ocQX4j4kTpZgE7PQyBie6aZEdPc/VjcKtUPJ9GyGnuUmBLuzbSTjKQU0GZXF7B7o5RTXcK7CNBnauKq/+sChHizcJzA7eaicJiw9AiRRLej8GEn5dgib9wA+ILxr8bmJpdmc++S3ePxjuDxsxR0wmw+rpRF/gFwIDAQABAoIBAC5Sjf62L3sFIMskNOTuiI7v03lXlcFwtsHMy5/35OSpkKJrS4e53rRhEl71lOBTGok+QGY8bD1IIUq1J+zxNIzGEV8BeU/9aIF2rlo7l7oUTHJKdcmTVsn31G9QYzpAaXbnaB3MeDp7sQrtLCYOO7d6IBWFW3k1jTzMPpUdoo6HompXjAQFCfSjPE6vKHtQv1AsI5I1PtFBCwBsMpphElCsZ0qzuDZ7JZxdZcDvEV+NQbERNwqFFqbOPKad95ov+ZbvfaxTl7UX9iw/Jjg8qZLnKRAm3YfMvvnDadTTWdlQzk8D0wle8PP+EKpNbodV8WI7lgvdQopW2mkQNrD1PfkCgYEAytrJgHcpv2E4XBhzNIRVrUZP96aWjnMrCJe5nXXbdwn7iLYj7El5KakDZj6efy/B6szY5+jNjaZD0tz6EbFepmwZYU3fMns/aa55hXrV0lGxsRjYvXoOZMmtiJTAi2DRVS4AUv5fzP0CUo1oEj9S9Nzof8etIz6swv/CMcDfUd0CgYEAww+KIrNuqEt6OaMEaB9BzqhvZ5vU85VIaygSZjbGBkCKFE+Wd/lAKtxzYjeT/wdnc1V/yHdYzBs8qqr8Rgmo7nNaTrqSaq16YyhpSQv87I8cJ5CR6QPCE0kI1ZxUbrYl137BHuL/N+ccYe3Haun/FaUDwn+kMU8NkaxFz9UzLIMCgYEAtnD7lTyXIXaQt9ONmYgDfmmkdjkvY0KVS48Z81ISZgdmYFhgCp0yCZPmaMicyArtvv0pf7nTVThZ+4Weqtb5J3hFg0wEF2TdbXDr/m6xbAzpYTvVguQJmgAAwLv4R+J1osRSqlH5W2WZ9yESnoaxV2Ru/q1ZPD+cJWzc/N5Ye0kCgYA2yRuMKYdv/1sB6zZmpDjQeWBo4kzKaWhgqG2ahdHSmbfugngJlFkgcLB7qo6Ht6WnsCDmxhaLpS7B5KpcZYQsDPwVVrOfAMB6LzR7DsHoEz3eIu3IW4Tb88xmarUQL1qrju9g5HK4WdIP8z/qWHSLuMA4la8hJknyW0d0WzY3TwKBgDT9sZa1VK1wiW4gLFjqEN1fh46FxZnUAKAMht1ADmE/dGu9yqsit5+vdhfa+tJBZATvfhFwBcBgLx5YunPnBNZkFoUTgE9qWWRcGsfjLyL4S0NmmAQoQjuSHJR/IRmNKGZxzpB2JbIIIcGo1vAdUXqqZMAc9AJzG/FgryI7y6WK";

            Assert.Equal(expected, pkcs1);
        }
    }
}
