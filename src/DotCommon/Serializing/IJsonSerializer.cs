using System;

namespace DotCommon.Serializing
{
    /// <summary>Json序列化接口
    /// </summary>
    public interface IJsonSerializer
    {
        /// <summary>序列化对象
        /// </summary>
        string Serialize(object o);

        /// <summary>反序列化对象
        /// </summary>
        object Deserialize(string value, Type type);

        /// <summary>反序列化对象
        /// </summary>
        T Deserialize<T>(string value) where T : class;
    }
}
