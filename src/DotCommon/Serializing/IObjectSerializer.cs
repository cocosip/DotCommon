namespace DotCommon.Serializing
{
    /// <summary>
    /// 对象序列化
    /// </summary>
    public interface IObjectSerializer
    {
        /// <summary>
        /// 对象序列化
        /// </summary>
        byte[] Serialize<T>(T o);

        /// <summary>
        /// 对象反序列化
        /// </summary>
        T Deserialize<T>(byte[] bytes);
    }

    /// <summary>
    /// 对象序列化
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IObjectSerializer<T>
    {
        /// <summary>
        /// 对象序列化
        /// </summary>
        byte[] Serialize(T o);

        /// <summary>
        /// 对象反序列化
        /// </summary>
        T Deserialize(byte[] bytes);
    }
}
