using System;

namespace DotCommon.Serializing
{
    /// <summary>Xml序列化接口
    /// </summary>
    public interface IXmlSerializer
    {
        string Serialize(object o);
        object Deserialize(string value, Type type);
        T Deserialize<T>(string value) where T : class;
    }
}
