using System;
using System.Linq;
using System.Text;

namespace DotCommon.Utility
{
    /// <summary>字节相关帮助类
    /// </summary>
    public class ByteBufferUtil
    {
        /// <summary>编码String类型,转换成Byte[] 并且附带长度
        /// </summary>
        public static byte[] EncodeString(string sourceString, string code = "utf-8")
        {
            var encoding = Encoding.GetEncoding(code);
            var stringBytes = encoding.GetBytes(sourceString);
            var lengthBytes = BitConverter.GetBytes(stringBytes.Length);
            return Combine(lengthBytes, stringBytes);
        }

        /// <summary>从byte数组中取出String字符串,消息的格式为[消息长度,int4字节][消息内容]
        /// </summary>
        public static string DecodeString(byte[] sourceBuffer, int startOffset, out int nextStartOffset,
            string code = "utf-8")
        {
            var encoding = Encoding.GetEncoding(code);
            return encoding.GetString(DecodeBytes(sourceBuffer, startOffset, out nextStartOffset));
        }

        /// <summary>从byte数组中取出short数字
        /// </summary>
        public static short DecodeShort(byte[] sourceBuffer, int startOffset, out int nextStartOffset)
        {
            var shortBytes = new byte[2];
            Buffer.BlockCopy(sourceBuffer, startOffset, shortBytes, 0, 2);
            nextStartOffset = startOffset + 2;
            return BitConverter.ToInt16(shortBytes, 0);
        }

        /// <summary>从byte数组中取出int数字
        /// </summary>
        public static int DecodeInt(byte[] sourceBuffer, int startOffset, out int nextStartOffset)
        {
            var intBytes = new byte[4];
            Buffer.BlockCopy(sourceBuffer, startOffset, intBytes, 0, 4);
            nextStartOffset = startOffset + 4;
            return BitConverter.ToInt32(intBytes, 0);
        }

        /// <summary>从byte数组中取出long数字
        /// </summary>
        public static long DecodeLong(byte[] sourceBuffer, int startOffset, out int nextStartOffset)
        {
            var longBytes = new byte[8];
            Buffer.BlockCopy(sourceBuffer, startOffset, longBytes, 0, 8);
            nextStartOffset = startOffset + 8;
            return BitConverter.ToInt64(longBytes, 0);
        }

        /// <summary>将时间转换成long类型,再转换成byte
        /// </summary>
        public static byte[] EncodeDateTime(DateTime datetime)
        {
            return BitConverter.GetBytes(datetime.Ticks);
        }

        /// <summary>从byte数组中取出时间
        /// </summary>
        public static DateTime DecodeDateTime(byte[] sourceBuffer, int startOffset, out int nextStartOffset)
        {
            var longBytes = new byte[8];
            Buffer.BlockCopy(sourceBuffer, startOffset, longBytes, 0, 8);
            nextStartOffset = startOffset + 8;
            return new DateTime(BitConverter.ToInt64(longBytes, 0));
        }

        /// <summary>转换Byte类型的消息,转换为[消息长度4byte][消息内容,消息内容为byte]
        /// </summary>
        public static byte[] EncodeBytes(byte[] sourceBuffer)
        {
            var lengthBytes = BitConverter.GetBytes(sourceBuffer.Length);
            return Combine(lengthBytes, sourceBuffer);
        }

        /// <summary>如果消息是这样的格式:[消息长度4byte][消息内容],取出该消息返回byte
        /// </summary>
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

        /// <summary>合并byte数组
        /// </summary>
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

        public static long SwapLong(long value)
        {
            return ((SwapInt((int)value) & 0xFFFFFFFF) << 32)
                   | (SwapInt((int)(value >> 32)) & 0xFFFFFFFF);
        }

        public static int SwapInt(int value)
        {
            return ((SwapShort((short)value) & 0xFFFF) << 16)
                   | (SwapShort((short)(value >> 16)) & 0xFFFF);
        }

        public static short SwapShort(short value)
        {
            return (short)(((value & 0xFF) << 8) | (value >> 8) & 0xFF);
        }


        #region 将Bytes数组转换为十六进制
        /// <summary> 将Bytes数组转换为十六进制
        /// </summary>
        public static string ByteArrayToString(byte[] inputBytes)
        {
            var output = new StringBuilder(inputBytes.Length);
            for (var i = 0; i <= inputBytes.Length - 1; i++)
            {
                output.Append(inputBytes[i].ToString("X2"));
            }
            return output.ToString();
        }
        #endregion
    }
}
