using System;

namespace DotCommon.Serializing
{
    /// <summary>二进制序列化接口
    /// </summary>
    public interface IBinarySerializer
    {
        byte[] Serialize(object obj);
        object Deserialize(byte[] data, Type type);
        T Deserialize<T>(byte[] data);
    }
}
