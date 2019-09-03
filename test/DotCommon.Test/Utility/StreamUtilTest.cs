using DotCommon.Utility;
using System;
using System.Text;
using Xunit;

namespace DotCommon.Test.Utility
{
    public class StreamUtilTest
    {
        [Fact]
        public void StreamByte_ToBytes_ToStream_Test()
        {

            var bytes = Encoding.UTF8.GetBytes("Hello,world");
            using (var stream = StreamUtil.BufferToStream(bytes))
            {
                stream.Position = 1;
                var actualBytes = StreamUtil.StreamToBuffer(stream, 5);

                var newArray = new byte[bytes.Length];
                Array.Copy(actualBytes, 0, newArray, 0, bytes.Length);
                Assert.Equal(bytes.Length, newArray.Length);
                Assert.Equal(bytes, newArray);
            }

            using (var stream = StreamUtil.BufferToStream(bytes))
            {
                var actualBytes = StreamUtil.StreamToBuffer(stream);
                var newArray = new byte[bytes.Length];
                Array.Copy(actualBytes, 0, newArray, 0, bytes.Length);
                Assert.Equal(bytes.Length, newArray.Length);
                Assert.Equal(bytes, newArray);
            }
        }
    }
}
