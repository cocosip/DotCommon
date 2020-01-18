using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotCommon.Utility
{
    /// <summary>字节相关帮助类
    /// </summary>
    public static class ByteBufferUtil
    {
        /// <summary>将字符串转换成Byte[] 并且附带长度
        /// </summary>
        /// <param name="source">字符串</param>
        /// <param name="encode">编码格式</param>
        /// <returns></returns>
        public static byte[] EncodeString(string source, string encode = "utf-8")
        {
            var stringBytes = Encoding.GetEncoding(encode).GetBytes(source);
            var lengthBytes = BitConverter.GetBytes(stringBytes.Length);
            return Combine(lengthBytes, stringBytes);
        }

        /// <summary>>从byte数组中取出String字符串,消息的格式为[消息长度,int4字节][消息内容]
        /// </summary>
        /// <param name="sourceBuffer">源数组</param>
        /// <param name="startOffset">开始偏移量</param>
        /// <param name="nextStartOffset">下一个偏移量</param>
        /// <param name="encode">编码格式</param>
        /// <returns></returns>
        public static string DecodeString(byte[] sourceBuffer, int startOffset, out int nextStartOffset,
            string encode = "utf-8")
        {
            var encoding = Encoding.GetEncoding(encode);
            return encoding.GetString(DecodeBytes(sourceBuffer, startOffset, out nextStartOffset));
        }

        /// <summary>从byte数组中取出short数字
        /// </summary>
        /// <param name="sourceBuffer">源数组</param>
        /// <param name="startOffset">开始偏移量</param>
        /// <param name="nextStartOffset">下一个偏移量</param>
        /// <returns></returns>
        public static short DecodeShort(byte[] sourceBuffer, int startOffset, out int nextStartOffset)
        {
            var shortBytes = new byte[2];
            Buffer.BlockCopy(sourceBuffer, startOffset, shortBytes, 0, 2);
            nextStartOffset = startOffset + 2;
            return BitConverter.ToInt16(shortBytes, 0);
        }

        /// <summary>从byte数组中取出int数字
        /// </summary>
        /// <param name="sourceBuffer">源数组</param>
        /// <param name="startOffset">开始偏移量</param>
        /// <param name="nextStartOffset">下一个偏移量</param>
        /// <returns></returns>
        public static int DecodeInt(byte[] sourceBuffer, int startOffset, out int nextStartOffset)
        {
            var intBytes = new byte[4];
            Buffer.BlockCopy(sourceBuffer, startOffset, intBytes, 0, 4);
            nextStartOffset = startOffset + 4;
            return BitConverter.ToInt32(intBytes, 0);
        }

        /// <summary>从byte数组中取出long数字
        /// </summary>
        /// <param name="sourceBuffer">源数组</param>
        /// <param name="startOffset">开始偏移量</param>
        /// <param name="nextStartOffset">下一个偏移量</param>
        /// <returns></returns>
        public static long DecodeLong(byte[] sourceBuffer, int startOffset, out int nextStartOffset)
        {
            var longBytes = new byte[8];
            Buffer.BlockCopy(sourceBuffer, startOffset, longBytes, 0, 8);
            nextStartOffset = startOffset + 8;
            return BitConverter.ToInt64(longBytes, 0);
        }

        /// <summary>将时间转换成long类型,再转换成byte
        /// </summary>
        /// <param name="dateTime">时间</param>
        /// <returns></returns>
        public static byte[] EncodeDateTime(DateTime dateTime)
        {
            return BitConverter.GetBytes(dateTime.Ticks);
        }

        /// <summary>从byte数组中取出时间
        /// </summary>
        /// <param name="sourceBuffer">源数组</param>
        /// <param name="startOffset">开始偏移量</param>
        /// <param name="nextStartOffset">下一个偏移量</param>
        /// <returns></returns>
        public static DateTime DecodeDateTime(byte[] sourceBuffer, int startOffset, out int nextStartOffset)
        {
            var longBytes = new byte[8];
            Buffer.BlockCopy(sourceBuffer, startOffset, longBytes, 0, 8);
            nextStartOffset = startOffset + 8;
            return new DateTime(BitConverter.ToInt64(longBytes, 0));
        }

        /// <summary>转换Byte类型的消息,转换为[消息长度4byte][消息内容,消息内容为byte]
        /// </summary>
        /// <param name="sourceBuffer">二进制数组</param>
        /// <returns></returns>
        public static byte[] EncodeBytes(byte[] sourceBuffer)
        {
            var lengthBytes = BitConverter.GetBytes(sourceBuffer.Length);
            return Combine(lengthBytes, sourceBuffer);
        }

        /// <summary>从byte数组中取出byte[]
        /// </summary>
        /// <param name="sourceBuffer">源数组</param>
        /// <param name="startOffset">开始偏移量</param>
        /// <param name="nextStartOffset">下一个偏移量</param>
        /// <returns></returns>
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

        /// <summary>SwapLong
        /// </summary>
        public static long SwapLong(long value)
        {
            return ((SwapInt((int)value) & 0xFFFFFFFF) << 32)
                   | (SwapInt((int)(value >> 32)) & 0xFFFFFFFF);
        }

        /// <summary>SwapInt
        /// </summary>
        public static int SwapInt(int value)
        {
            return ((SwapShort((short)value) & 0xFFFF) << 16)
                   | (SwapShort((short)(value >> 16)) & 0xFFFF);
        }

        /// <summary>SwapShort
        /// </summary>
        public static short SwapShort(short value)
        {
            return (short)(((value & 0xFF) << 8) | (value >> 8) & 0xFF);
        }

        /// <summary> 将byte[]数组转换为十六进制
        /// </summary>
        public static string ByteBufferToHex16(byte[] byteBuffer)
        {
            return byteBuffer.Aggregate("", (current, b) => current + b.ToString("X2"));
            
            //var hex16String = new StringBuilder(byteBuffer.Length);
            //for (var i = 0; i < byteBuffer.Length; i++)
            //{
            //    hex16String.Append(byteBuffer[i].ToString("X2"));
            //}
            //return hex16String.ToString();
        }

        /// <summary>将十六进制字符串转换为byte[]数组
        /// </summary>
        /// <param name="hex16String">十六进制字符串</param>
        /// <returns></returns>
        public static byte[] Hex16ToByteBuffer(string hex16String)
        {
            if (hex16String.Length % 2 != 0)
            {
                throw new ArgumentException("不是有效的十六进制字符串");
            }
            //长度
            var len = hex16String.Length / 2;
            var byteArray = new byte[len];
            var sourceSpan = hex16String.AsSpan();
            for (var i = 0; i < len; i++)
            {
                var hexString = sourceSpan.Slice(i * 2, 2).ToString();
                byteArray[i] = Convert.ToByte(hexString, 16);
            }
            return byteArray;
        }

    }
}
