using System;

namespace DotCommon.Serializing
{
    /// <summary>Xml序列化接口
    /// </summary>
    public interface IXmlSerializer
    {
        string Serialize(object obj);
        object Deserialize(string value, Type type);
        T Deserialize<T>(string value) where T : class;
    }
}
