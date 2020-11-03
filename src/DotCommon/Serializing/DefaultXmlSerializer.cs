using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;

namespace DotCommon.Serializing
{
    /// <summary>
    /// Xml序列化
    /// </summary>
    public class DefaultXmlSerializer : IXmlSerializer
    {
        /// <summary>
        /// 序列化对象
        /// </summary>
        public string Serialize(object o)
        {
            var serializer = new DataContractSerializer(o.GetType());
            using var stream = new MemoryStream();
            serializer.WriteObject(stream, o);
            stream.Position = 0;
            using var sr = new StreamReader(stream);
            var resultStr = sr.ReadToEnd();
            return resultStr;
        }

        /// <summary>
        /// 反序列化对象
        /// </summary>
        public object Deserialize(string value, Type type)
        {
            var serializer = new DataContractSerializer(type);
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(value.ToCharArray()));
            var o = serializer.ReadObject(stream);
            return o;
        }

        /// <summary>
        /// 反序列化对象
        /// </summary>
        public T Deserialize<T>(string value) where T : class
        {
            var serializer = new DataContractSerializer(typeof(T));
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(value.ToCharArray()));
            var o = (T)serializer.ReadObject(stream);
            return o;
        }
    }
}
