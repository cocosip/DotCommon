using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace DotCommon.Serializing
{
    /// <summary>Json序列化
    /// </summary>
    public class DefaultJsonSerializer : IJsonSerializer
    {
        /// <summary>序列化对象
        /// </summary>
        public string Serialize(object o)
        {
            var serializer = new DataContractJsonSerializer(o.GetType());
            using (var stream = new MemoryStream())
            {
                serializer.WriteObject(stream, o);
                stream.Position = 0;
                using (var sr = new StreamReader(stream))
                {
                    return sr.ReadToEnd();
                }
            }
        }

        /// <summary>反序列化对象
        /// </summary>
        public object Deserialize(string value, Type type)
        {
            var serializer = new DataContractJsonSerializer(type);
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(value.ToCharArray())))
            {
                return serializer.ReadObject(stream);
            }
        }

        /// <summary>反序列化对象
        /// </summary>
        public T Deserialize<T>(string value) where T : class
        {
            var serializer = new DataContractJsonSerializer(typeof(T));
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(value.ToCharArray())))
            {
                var obj = (T)serializer.ReadObject(stream);
                return obj;
            }
        }
    }
}
