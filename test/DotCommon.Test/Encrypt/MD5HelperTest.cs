using System;
using System.Text;
using DotCommon.Encrypt;
using Xunit;

namespace DotCommon.Test.Encrypt
{
    public class MD5HelperTest
    {
        [Fact]
        public void GetMD5_String_Test()
        {
            // 测试正常字符串
            var actual = MD5Helper.GetMD5("helloworld");
            Assert.Equal("FC5E038D38A57032085441E7FE7010B0", actual);

            // 测试空字符串
            var emptyActual = MD5Helper.GetMD5("");
            Assert.Equal("D41D8CD98F00B204E9800998ECF8427E", emptyActual);

            // 测试中文字符串
            var chineseActual = MD5Helper.GetMD5("你好世界");
            Assert.Equal("65396EE4AAD0B4F17AACD1C6112EE364", chineseActual);
        }

        [Fact]
        public void GetMD5_String_With_Encoding_Test()
        {
            // 测试指定编码
            var actual = MD5Helper.GetMD5("hello", "UTF-8");
            Assert.Equal("5D41402ABC4B2A76B9719D911017C592", actual);

            // 测试不同编码可能产生不同结果
            var utf8Actual = MD5Helper.GetMD5("你好", "UTF-8");
            var asciiActual = MD5Helper.GetMD5("hello", "ASCII");
            // 不同编码会产生不同的字节序列，因此MD5值不同
            Assert.NotEqual(utf8Actual, asciiActual);
        }

        [Fact]
        public void GetMD5_ByteArray_Test()
        {
            // 测试字节数组
            var data = new byte[] { 1, 2, 3, 4, 5 };
            var actual = MD5Helper.GetMD5(data);
            Assert.Equal("7CFDD07889B3295D6A550914AB35E068", actual);

            // 测试空字节数组
            var emptyData = new byte[0];
            var emptyActual = MD5Helper.GetMD5(emptyData);
            Assert.Equal("D41D8CD98F00B204E9800998ECF8427E", emptyActual);
        }

        [Fact]
        public void GetMD5Internal_Test()
        {
            // 测试内部方法
            var data = new byte[] { 1, 2, 3, 4, 5 };
            var actual = MD5Helper.GetMD5Internal(data);
            Assert.Equal(16, actual.Length); // MD5 hash is 16 bytes

            // 测试空字节数组
            var emptyData = new byte[0];
            var emptyActual = MD5Helper.GetMD5Internal(emptyData);
            Assert.Equal(16, emptyActual.Length);
        }

        [Fact]
        public void GetMD5_Null_Input_Test()
        {
            // 测试字符串为null的情况
            Assert.Throws<ArgumentNullException>(() => MD5Helper.GetMD5((string)null));

            // 测试字节数组为null的情况
            Assert.Throws<ArgumentNullException>(() => MD5Helper.GetMD5((byte[])null));

            // 测试GetMD5Internal方法中字节数组为null的情况
            Assert.Throws<ArgumentNullException>(() => MD5Helper.GetMD5Internal(null));
        }

        [Fact]
        public void GetMD5_Invalid_Encoding_Test()
        {
            // 测试无效编码名称
            Assert.Throws<ArgumentException>(() => MD5Helper.GetMD5("test", ""));
            Assert.Throws<ArgumentException>(() => MD5Helper.GetMD5("test", null));
            Assert.Throws<ArgumentException>(() => MD5Helper.GetMD5("test", "InvalidEncoding"));
        }
    }
}