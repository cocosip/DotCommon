using System;
using System.IO;

namespace DotCommon.Utility
{
    public static class StreamUtil
    {

        #region Stream与Byte转换

        /// <summary>将Stream刘转换成二进制数组
        /// </summary>
        public static byte[] StreamToBuffer(Stream stream, int bufferLen = 0)
        {
            //将流读取位置初始到0
            if (stream.Position > 0 && stream.CanSeek)
            {
                stream.Seek(0, SeekOrigin.Begin);
            }

            if (bufferLen < 1)
            {
                bufferLen = 0X8000;
            }
            byte[] buffer = new byte[bufferLen];
            int read = 0;
            int block;
            // 每次从流中读取缓存大小的数据 直到读取完所有的流为止
            while ((block = stream.Read(buffer, read, buffer.Length - read)) > 0)
            {
                // 重新设定读取位置
                read += block;
                // 检查是否到达了缓存的边界，检查是否还有可以读取的信息
                if (read == buffer.Length)
                {
                    // 尝试读取一个字节
                    int nextByte = stream.ReadByte();
                    // 读取失败则说明读取完成可以返回结果
                    if (nextByte == -1)
                    {
                        return buffer;
                    }
                    // 调整数组大小准备继续读取
                    byte[] newBuf = new byte[buffer.Length * 2];
                    Array.Copy(buffer, newBuf, buffer.Length);
                    newBuf[read] = (byte)nextByte;
                    // buffer是一个引用（指针），这里意在重新设定buffer指针指向一个更大的内存
                    buffer = newBuf;
                    read++;
                }
            }
            return buffer;
        }

        /// <summary>将byte字节数组转换成流
        /// </summary>
        public static Stream BufferToStream(byte[] buffer)
        {
            Stream stream = new MemoryStream(buffer);
            return stream;
        }

        #endregion

    }
}
