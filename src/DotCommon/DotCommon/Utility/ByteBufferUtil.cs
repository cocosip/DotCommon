using System;
using System.Linq;
using System.Text;

namespace DotCommon.Utility
{
    /// <summary>
    /// Utility class for byte array manipulations and conversions.
    /// Provides methods for encoding/decoding strings, numbers, and dates to/from byte arrays,
    /// as well as hexadecimal conversions and byte array combinations.
    /// </summary>
    public static class ByteBufferUtil
    {
        /// <summary>
        /// Encodes a string into a byte array, prefixed with its length (4 bytes).
        /// The format is [length (4 bytes)][string content bytes].
        /// </summary>
        /// <param name="source">The string to encode.</param>
        /// <param name="encode">The encoding to use (default is "utf-8").</param>
        /// <returns>A byte array representing the encoded string with its length prefix.</returns>
        public static byte[] EncodeString(string source, string encode = "utf-8")
        {
            var stringBytes = Encoding.GetEncoding(encode).GetBytes(source);
            var lengthBytes = BitConverter.GetBytes(stringBytes.Length);
            return Combine(lengthBytes, stringBytes);
        }

        /// <summary>
        /// Decodes a string from a byte array. The message format is assumed to be
        /// [message length (4 bytes)][message content bytes].
        /// </summary>
        /// <param name="sourceBuffer">The source byte array.</param>
        /// <param name="startOffset">The starting offset in the source buffer to begin decoding.</param>
        /// <param name="nextStartOffset">Output parameter: The offset in the source buffer where the next data block starts.</param>
        /// <param name="encode">The encoding to use (default is "utf-8").</param>
        /// <returns>The decoded string.</returns>
        public static string DecodeString(byte[] sourceBuffer, int startOffset, out int nextStartOffset,
            string encode = "utf-8")
        {
            var encoding = Encoding.GetEncoding(encode);
            return encoding.GetString(DecodeBytes(sourceBuffer, startOffset, out nextStartOffset));
        }

        /// <summary>
        /// Decodes a short (2-byte integer) from a byte array.
        /// </summary>
        /// <param name="sourceBuffer">The source byte array.</param>
        /// <param name="startOffset">The starting offset in the source buffer.</param>
        /// <param name="nextStartOffset">Output parameter: The offset in the source buffer where the next data block starts.</param>
        /// <returns>The decoded short value.</returns>
        public static short DecodeShort(byte[] sourceBuffer, int startOffset, out int nextStartOffset)
        {
            var shortBytes = new byte[2];
            Buffer.BlockCopy(sourceBuffer, startOffset, shortBytes, 0, 2);
            nextStartOffset = startOffset + 2;
            return BitConverter.ToInt16(shortBytes, 0);
        }

        /// <summary>
        /// Decodes an int (4-byte integer) from a byte array.
        /// </summary>
        /// <param name="sourceBuffer">The source byte array.</param>
        /// <param name="startOffset">The starting offset in the source buffer.</param>
        /// <param name="nextStartOffset">Output parameter: The offset in the source buffer where the next data block starts.</param>
        /// <returns>The decoded int value.</returns>
        public static int DecodeInt(byte[] sourceBuffer, int startOffset, out int nextStartOffset)
        {
            var intBytes = new byte[4];
            Buffer.BlockCopy(sourceBuffer, startOffset, intBytes, 0, 4);
            nextStartOffset = startOffset + 4;
            return BitConverter.ToInt32(intBytes, 0);
        }

        /// <summary>
        /// Decodes a long (8-byte integer) from a byte array.
        /// </summary>
        /// <param name="sourceBuffer">The source byte array.</param>
        /// <param name="startOffset">The starting offset in the source buffer.</param>
        /// <param name="nextStartOffset">Output parameter: The offset in the source buffer where the next data block starts.</param>
        /// <returns>The decoded long value.</returns>
        public static long DecodeLong(byte[] sourceBuffer, int startOffset, out int nextStartOffset)
        {
            var longBytes = new byte[8];
            Buffer.BlockCopy(sourceBuffer, startOffset, longBytes, 0, 8);
            nextStartOffset = startOffset + 8;
            return BitConverter.ToInt64(longBytes, 0);
        }

        /// <summary>
        /// Encodes a <see cref="DateTime"/> object into an 8-byte array representing its Ticks value.
        /// </summary>
        /// <param name="dateTime">The DateTime object to encode.</param>
        /// <returns>A byte array representing the DateTime's Ticks.</returns>
        public static byte[] EncodeDateTime(DateTime dateTime)
        {
            return BitConverter.GetBytes(dateTime.Ticks);
        }

        /// <summary>
        /// Decodes a <see cref="DateTime"/> object from an 8-byte array.
        /// </summary>
        /// <param name="sourceBuffer">The source byte array.</param>
        /// <param name="startOffset">The starting offset in the source buffer.</param>
        /// <param name="nextStartOffset">Output parameter: The offset in the source buffer where the next data block starts.</param>
        /// <returns>The decoded DateTime object.</returns>
        public static DateTime DecodeDateTime(byte[] sourceBuffer, int startOffset, out int nextStartOffset)
        {
            var longBytes = new byte[8];
            Buffer.BlockCopy(sourceBuffer, startOffset, longBytes, 0, 8);
            nextStartOffset = startOffset + 8;
            return new DateTime(BitConverter.ToInt64(longBytes, 0));
        }

        /// <summary>
        /// Encodes a byte array by prefixing it with its length (4 bytes).
        /// The format is [length (4 bytes)][content bytes].
        /// </summary>
        /// <param name="sourceBuffer">The byte array to encode.</param>
        /// <returns>A new byte array with the length prefix.</returns>
        public static byte[] EncodeBytes(byte[] sourceBuffer)
        {
            var lengthBytes = BitConverter.GetBytes(sourceBuffer.Length);
            return Combine(lengthBytes, sourceBuffer);
        }

        /// <summary>
        /// Decodes a byte array from a source buffer, assuming it's prefixed with its length (4 bytes).
        /// </summary>
        /// <param name="sourceBuffer">The source byte array.</param>
        /// <param name="startOffset">The starting offset in the source buffer.</param>
        /// <param name="nextStartOffset">Output parameter: The offset in the source buffer where the next data block starts.</param>
        /// <returns>The decoded byte array.</returns>
        public static byte[] DecodeBytes(byte[] sourceBuffer, int startOffset, out int nextStartOffset)
        {
            var lengthBytes = new byte[4];
            Buffer.BlockCopy(sourceBuffer, startOffset, lengthBytes, 0, 4);
            startOffset += 4;

            var length = BitConverter.ToInt32(lengthBytes, 0);
            var dataBytes = new byte[length];
            Buffer.BlockCopy(sourceBuffer, startOffset, dataBytes, 0, length);
            startOffset += length;
            nextStartOffset = startOffset;
            return dataBytes;
        }

        /// <summary>
        /// Combines multiple byte arrays into a single new byte array.
        /// </summary>
        /// <param name="arrays">An array of byte arrays to combine.</param>
        /// <returns>A new byte array containing all the combined bytes.</returns>
        public static byte[] Combine(params byte[][] arrays)
        {
            var destination = new byte[arrays.Sum(x => x.Length)];
            var offset = 0;
            foreach (var data in arrays)
            {
                Buffer.BlockCopy(data, 0, destination, offset, data.Length);
                offset += data.Length;
            }
            return destination;
        }

        /// <summary>
        /// Swaps the byte order of a 64-bit integer (long).
        /// This is typically used for endianness conversion.
        /// </summary>
        /// <param name="value">The long value to swap.</param>
        /// <returns>The long value with its byte order swapped.</returns>
        public static long SwapLong(long value)
        {
            return ((SwapInt((int)value) & 0xFFFFFFFF) << 32)
                   | (SwapInt((int)(value >> 32)) & 0xFFFFFFFF);
        }

        /// <summary>
        /// Swaps the byte order of a 32-bit integer (int).
        /// This is typically used for endianness conversion.
        /// </summary>
        /// <param name="value">The int value to swap.</param>
        /// <returns>The int value with its byte order swapped.</returns>
        public static int SwapInt(int value)
        {
            return ((SwapShort((short)value) & 0xFFFF) << 16)
                   | (SwapShort((short)(value >> 16)) & 0xFFFF);
        }

        /// <summary>
        /// Swaps the byte order of a 16-bit integer (short).
        /// This is typically used for endianness conversion.
        /// </summary>
        /// <param name="value">The short value to swap.</param>
        /// <returns>The short value with its byte order swapped.</returns>
        public static short SwapShort(short value)
        {
            return (short)(((value & 0xFF) << 8) | (value >> 8) & 0xFF);
        }

        /// <summary>
        /// Converts a byte array to its hexadecimal string representation.
        /// Each byte is converted to two hexadecimal characters.
        /// </summary>
        /// <param name="byteBuffer">The byte array to convert.</param>
        /// <returns>A string representing the hexadecimal value of the byte array.</returns>
        public static string ByteBufferToHex16(byte[] byteBuffer)
        {
            // Using StringBuilder for better performance compared to string concatenation in a loop.
            var hex16String = new StringBuilder(byteBuffer.Length * 2);
            foreach (byte b in byteBuffer)
            {
                hex16String.Append(b.ToString("X2"));
            }
            return hex16String.ToString();
        }

        /// <summary>
        /// Converts a hexadecimal string to a byte array.
        /// The input string must have an even number of characters.
        /// </summary>
        /// <param name="hex16String">The hexadecimal string to convert.</param>
        /// <returns>A byte array representing the decoded hexadecimal string.</returns>
        /// <exception cref="ArgumentException">Thrown if the input string has an odd length.</exception>
        public static byte[] Hex16ToByteBuffer(string hex16String)
        {
            if (hex16String.Length % 2 != 0)
            {
                throw new ArgumentException("Invalid hexadecimal string: length must be even.");
            }
            var len = hex16String.Length / 2;
            var byteArray = new byte[len];
            for (var i = 0; i < len; i++)
            {
                // Directly convert substring to byte
                byteArray[i] = Convert.ToByte(hex16String.Substring(i * 2, 2), 16);
            }
            return byteArray;
        }


        /// <summary>
        /// Converts a long integer to an 8-byte array in big-endian order.
        /// </summary>
        /// <param name="l">The long integer to convert.</param>
        /// <returns>An 8-byte array representing the long integer.</returns>
        public static byte[] LongToBuffer(long l)
        {
            byte[] buffer = new byte[8];
            buffer[0] = (byte)((l >> 56) & 0xFF);
            buffer[1] = (byte)((l >> 48) & 0xFF);
            buffer[2] = (byte)((l >> 40) & 0xFF);
            buffer[3] = (byte)((l >> 32) & 0xFF);
            buffer[4] = (byte)((l >> 24) & 0xFF);
            buffer[5] = (byte)((l >> 16) & 0xFF);
            buffer[6] = (byte)((l >> 8) & 0xFF);
            buffer[7] = (byte)(l & 0xFF);
            return buffer;
        }

        /// <summary>
        /// Converts an 8-byte array (assumed to be in big-endian order) to a long integer.
        /// Handles signed bytes by converting them to unsigned before shifting.
        /// </summary>
        /// <param name="buffer">The 8-byte array to convert.</param>
        /// <param name="offset">The starting offset in the buffer (default is 0).</param>
        /// <returns>The decoded long integer.</returns>
        public static long BufferToLong(byte[] buffer, int offset = 0)
        {
#pragma warning disable CS0675 // Bitwise-or operator used on a sign-extended operand; consider casting to a smaller unsigned type first
            return (((long)(buffer[offset] >= 0 ? buffer[offset] : 256 + buffer[offset])) << 56) |
                  (((long)(buffer[offset + 1] >= 0 ? buffer[offset + 1] : 256 + buffer[offset + 1])) << 48) |
                  (((long)(buffer[offset + 2] >= 0 ? buffer[offset + 2] : 256 + buffer[offset + 2])) << 40) |
                  (((long)(buffer[offset + 3] >= 0 ? buffer[offset + 3] : 256 + buffer[offset + 3])) << 32) |
                  (((long)(buffer[offset + 4] >= 0 ? buffer[offset + 4] : 256 + buffer[offset + 4])) << 24) |
                  (((long)(buffer[offset + 5] >= 0 ? buffer[offset + 5] : 256 + buffer[offset + 5])) << 16) |
                  (((long)(buffer[offset + 6] >= 0 ? buffer[offset + 6] : 256 + buffer[offset + 6])) << 8) |
                  ((buffer[offset + 7] >= 0 ? buffer[offset + 7] : 256 + buffer[offset + 7]));
#pragma warning restore CS0675 

        }

    }
}
