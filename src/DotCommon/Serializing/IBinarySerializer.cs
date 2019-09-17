using System;

namespace DotCommon.Serializing
{
    /// <summary>二进制序列化接口
    /// </summary>
    public interface IBinarySerializer
    {
        /// <summary> 将对象序列化成二进制
        /// </summary>
        byte[] Serialize(object o);

        /// <summary> 将二进制反序列化成byte
        /// </summary>
        object Deserialize(byte[] data, Type type);

        /// <summary> 将二进制反序列化成对象
        /// </summary>
        T Deserialize<T>(byte[] data);
    }
}
