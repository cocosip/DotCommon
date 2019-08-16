using DotCommon.Utility;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DotCommon.Test.Utility
{
    public class ByteBufferUtilTest
    {
        [Fact]
        public void Combine_Test()
        {
            var b1 = new byte[] { 1, 2 };
            var b2 = new byte[] { 3, 4 };
            var actual = ByteBufferUtil.Combine(b1, b2);
            Assert.Equal(4, actual.Length);
            Assert.Equal(new byte[] { 1, 2, 3, 4 }, actual);
        }

        [Fact]
        public void Swap_Test()
        {
            Assert.Equal(16777216, ByteBufferUtil.SwapInt(1));
            Assert.Equal(2560, ByteBufferUtil.SwapShort(10));
            Assert.Equal(-6879810281250226176, ByteBufferUtil.SwapLong(100000));

        }

        [Fact]
        public void Encode_Test()
        {
            var r1 = ByteBufferUtil.EncodeString("hellow");
            var sourceBytes1 = Encoding.UTF8.GetBytes("hellow");
            var dest1 = new byte[4];
            Array.Copy(r1, dest1, dest1.Length);
            Assert.Equal(sourceBytes1.Length, BitConverter.ToInt32(dest1));

            var r2 = ByteBufferUtil.DecodeString(r1, 0, out int len);
            Assert.Equal("hellow", r2);

            var short_1 = BitConverter.GetBytes((short)1);
            var int_1 = BitConverter.GetBytes(2);
            var long_1 = BitConverter.GetBytes(3L);
            var combineByte1 = ByteBufferUtil.Combine(short_1, int_1, long_1);

            var startOffset = 0;
            var short_2 = ByteBufferUtil.DecodeShort(combineByte1, startOffset, out startOffset);
            var int_2 = ByteBufferUtil.DecodeInt(combineByte1, startOffset, out startOffset);
            var long_2 = ByteBufferUtil.DecodeLong(combineByte1, startOffset, out startOffset);
            Assert.Equal(1, short_2);
            Assert.Equal(2, int_2);
            Assert.Equal(3, long_2);

            var d1 = new DateTime(2019, 1, 1, 1, 1, 1);
            var b1 = ByteBufferUtil.EncodeDateTime(d1);
            var d2 = ByteBufferUtil.DecodeDateTime(b1, 0, out int n1);
            Assert.Equal(d1, d2);

            var byte1 = new byte[] { 1, 2, 3 };
            var b2 = ByteBufferUtil.EncodeBytes(byte1);
            var byte2 = ByteBufferUtil.DecodeBytes(b2, 0, out int n2);
            Assert.Equal(byte1, byte2);


        }

    }
}
