using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace DotCommon.Serializing
{
    public class DefaultJsonSerializer : IJsonSerializer
    {
        public string Serialize(object obj)
        {
            var serializer = new DataContractJsonSerializer(obj.GetType());
            using (var stream = new MemoryStream())
            {
                serializer.WriteObject(stream, obj);
                stream.Position = 0;
                using (var sr = new StreamReader(stream))
                {
                    return sr.ReadToEnd();
                }
            }
        }

        public object Deserialize(string value, Type type)
        {
            var serializer = new DataContractJsonSerializer(type);
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(value.ToCharArray())))
            {
                return serializer.ReadObject(stream);
            }
        }
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
