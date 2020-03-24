using DotCommon.Serializing;
using ProtoBuf;
using System;
using System.IO;

namespace DotCommon.ProtoBuf
{
    /// <summary>ProtoBuf序列化类
    /// </summary>
    public class ProtocolBufSerializer : IBinarySerializer
    {
        /// <summary>序列化
        /// </summary>
        public byte[] Serialize(object obj)
        {
            using (var stream = new MemoryStream())
            {
                Serializer.Serialize(stream, obj);
                return stream.ToArray();
            }
        }

        /// <summary>反序列化
        /// </summary>
        public T Deserialize<T>(byte[] data)
        {
            using (var stream = new MemoryStream(data))
            {
                return Serializer.Deserialize<T>(stream);
            }
        }

        /// <summary>反序列化
        /// </summary>
        public object Deserialize(byte[] data, Type type)
        {
            using (var stream = new MemoryStream(data))
            {
                return Serializer.NonGeneric.Deserialize(type, stream);
            }
        }
    }
}
