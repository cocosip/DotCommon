﻿using System;
using System.IO;

namespace DotCommon.Utility
{
    /// <summary>
    /// Utility class for stream operations.
    /// </summary>
    public static class StreamUtil
    {
        /// <summary>
        /// Converts a Stream to a byte array.
        /// </summary>
        /// <param name="stream">The stream to convert.</param>
        /// <param name="bufferLen">The initial buffer length. If less than 1, defaults to 0x8000 (32KB).</param>
        /// <returns>A byte array containing the stream data.</returns>
        /// <exception cref="ArgumentNullException">Thrown when stream is null.</exception>
        public static byte[] StreamToBuffer(Stream stream, int bufferLen = 0)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            // Reset stream position to 0 if seekable
            if (stream.CanSeek && stream.Position > 0)
            {
                stream.Seek(0, SeekOrigin.Begin);
            }

            // Set default buffer length if not specified
            if (bufferLen < 1)
            {
                bufferLen = 0x8000; // 32KB default buffer size
            }

            byte[] buffer = new byte[bufferLen];
            int read = 0;
            int block;

            // Read data from stream in chunks until all data is read
            while ((block = stream.Read(buffer, read, buffer.Length - read)) > 0)
            {
                read += block;

                // Check if we've reached the end of the buffer
                if (read == buffer.Length)
                {
                    // Try to read one more byte to see if we've reached the end of the stream
                    int nextByte = stream.ReadByte();
                    
                    // If we've reached the end of the stream, return the buffer
                    if (nextByte == -1)
                    {
                        // Resize the buffer to the actual read size
                        if (read < buffer.Length)
                        {
                            Array.Resize(ref buffer, read);
                        }
                        return buffer;
                    }

                    // If there's more data, expand the buffer and continue
                    byte[] newBuf = new byte[buffer.Length * 2];
                    Array.Copy(buffer, newBuf, buffer.Length);
                    newBuf[read] = (byte)nextByte;
                    buffer = newBuf;
                    read++;
                }
            }

            // Resize the buffer to the actual read size
            if (read < buffer.Length)
            {
                Array.Resize(ref buffer, read);
            }
            
            return buffer;
        }

        /// <summary>
        /// Converts a byte array to a Stream.
        /// </summary>
        /// <param name="buffer">The byte array to convert.</param>
        /// <returns>A MemoryStream containing the byte array data.</returns>
        /// <exception cref="ArgumentNullException">Thrown when buffer is null.</exception>
        public static Stream BufferToStream(byte[] buffer)
        {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));

            return new MemoryStream(buffer);
        }
    }
}