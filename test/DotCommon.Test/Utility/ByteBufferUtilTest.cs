using System;
using System.Text;
using Xunit;
using DotCommon.Utility;

namespace DotCommon.Test.Utility
{
    public class ByteBufferUtilTest
    {
        [Fact]
        public void EncodeString_ShouldReturnCorrectBytesWithLengthPrefix()
        {
            var testString = "Hello World!";
            var expectedBytes = ByteBufferUtil.Combine(BitConverter.GetBytes(testString.Length), Encoding.UTF8.GetBytes(testString));
            var actualBytes = ByteBufferUtil.EncodeString(testString);
            Assert.Equal(expectedBytes, actualBytes);
        }

        [Fact]
        public void DecodeString_ShouldReturnCorrectString()
        {
            var testString = "Hello World!";
            var encodedBytes = ByteBufferUtil.EncodeString(testString);
            var decodedString = ByteBufferUtil.DecodeString(encodedBytes, 0, out int nextOffset);
            Assert.Equal(testString, decodedString);
            Assert.Equal(encodedBytes.Length, nextOffset);
        }

        [Fact]
        public void EncodeBytes_ShouldReturnCorrectBytesWithLengthPrefix()
        {
            var testBytes = new byte[] { 0x01, 0x02, 0x03, 0x04 };
            var expectedBytes = ByteBufferUtil.Combine(BitConverter.GetBytes(testBytes.Length), testBytes);
            var actualBytes = ByteBufferUtil.EncodeBytes(testBytes);
            Assert.Equal(expectedBytes, actualBytes);
        }

        [Fact]
        public void DecodeBytes_ShouldReturnCorrectBytes()
        {
            var testBytes = new byte[] { 0x01, 0x02, 0x03, 0x04 };
            var encodedBytes = ByteBufferUtil.EncodeBytes(testBytes);
            var decodedBytes = ByteBufferUtil.DecodeBytes(encodedBytes, 0, out int nextOffset);
            Assert.Equal(testBytes, decodedBytes);
            Assert.Equal(encodedBytes.Length, nextOffset);
        }

        [Fact]
        public void Combine_ShouldConcatenateByteArrays()
        {
            var arr1 = new byte[] { 0x01, 0x02 };
            var arr2 = new byte[] { 0x03, 0x04 };
            var arr3 = new byte[] { 0x05 };
            var expected = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05 };
            var actual = ByteBufferUtil.Combine(arr1, arr2, arr3);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ByteBufferToHex16_ShouldReturnCorrectHexString()
        {
            var bytes = new byte[] { 0x0A, 0xBC, 0xEF, 0x12 };
            var expectedHex = "0ABCEF12";
            var actualHex = ByteBufferUtil.ByteBufferToHex16(bytes);
            Assert.Equal(expectedHex, actualHex);
        }

        [Fact]
        public void Hex16ToByteBuffer_ShouldReturnCorrectByteArray()
        {
            var hexString = "0ABCEF12";
            var expectedBytes = new byte[] { 0x0A, 0xBC, 0xEF, 0x12 };
            var actualBytes = ByteBufferUtil.Hex16ToByteBuffer(hexString);
            Assert.Equal(expectedBytes, actualBytes);
        }

        [Fact]
        public void Hex16ToByteBuffer_ShouldThrowArgumentExceptionForOddLengthString()
        {
            var hexString = "ABC";
            var exception = Record.Exception(() => ByteBufferUtil.Hex16ToByteBuffer(hexString));
            Assert.IsType<ArgumentException>(exception);
            Assert.Contains("Invalid hexadecimal string: length must be even.", exception.Message);
        }

        [Fact]
        public void EncodeDateTime_ShouldReturnCorrectBytes()
        {
            var dateTime = new DateTime(2023, 1, 1, 10, 30, 0, DateTimeKind.Utc);
            var expectedBytes = BitConverter.GetBytes(dateTime.Ticks);
            var actualBytes = ByteBufferUtil.EncodeDateTime(dateTime);
            Assert.Equal(expectedBytes, actualBytes);
        }

        [Fact]
        public void DecodeDateTime_ShouldReturnCorrectDateTime()
        {
            var dateTime = new DateTime(2023, 1, 1, 10, 30, 0, DateTimeKind.Utc);
            var encodedBytes = ByteBufferUtil.EncodeDateTime(dateTime);
            var decodedDateTime = ByteBufferUtil.DecodeDateTime(encodedBytes, 0, out int nextOffset);
            Assert.Equal(dateTime, decodedDateTime);
            Assert.Equal(encodedBytes.Length, nextOffset);
        }

        [Fact]
        public void DecodeShort_ShouldReturnCorrectShort()
        {
            var testShort = (short)12345;
            var bytes = BitConverter.GetBytes(testShort);
            var decodedShort = ByteBufferUtil.DecodeShort(bytes, 0, out int nextOffset);
            Assert.Equal(testShort, decodedShort);
            Assert.Equal(bytes.Length, nextOffset);
        }

        [Fact]
        public void DecodeInt_ShouldReturnCorrectInt()
        {
            var testInt = 123456789;
            var bytes = BitConverter.GetBytes(testInt);
            var decodedInt = ByteBufferUtil.DecodeInt(bytes, 0, out int nextOffset);
            Assert.Equal(testInt, decodedInt);
            Assert.Equal(bytes.Length, nextOffset);
        }

        [Fact]
        public void DecodeLong_ShouldReturnCorrectLong()
        {
            var testLong = 1234567890123456789L;
            var bytes = BitConverter.GetBytes(testLong);
            var decodedLong = ByteBufferUtil.DecodeLong(bytes, 0, out int nextOffset);
            Assert.Equal(testLong, decodedLong);
            Assert.Equal(bytes.Length, nextOffset);
        }

        [Fact]
        public void LongToBuffer_ShouldReturnCorrectBytes()
        {
            long value = 0x0102030405060708L;
            byte[] expected = { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08 };
            byte[] actual = ByteBufferUtil.LongToBuffer(value);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void BufferToLong_ShouldReturnCorrectLong()
        {
            byte[] buffer = { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08 };
            long expected = 0x0102030405060708L;
            long actual = ByteBufferUtil.BufferToLong(buffer);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SwapLong_ShouldSwapByteOrder()
        {
            long original = 0x0102030405060708L;
            long expected = 0x0807060504030201L; // Assuming little-endian to big-endian swap
            long actual = ByteBufferUtil.SwapLong(original);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SwapInt_ShouldSwapByteOrder()
        {
            int original = 0x01020304;
            int expected = 0x04030201;
            int actual = ByteBufferUtil.SwapInt(original);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SwapShort_ShouldSwapByteOrder()
        {
            short original = 0x0102;
            short expected = 0x0201;
            short actual = ByteBufferUtil.SwapShort(original);
            Assert.Equal(expected, actual);
        }
    }
}
