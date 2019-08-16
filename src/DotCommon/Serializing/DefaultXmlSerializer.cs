using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;

namespace DotCommon.Serializing
{
    /// <summary>Serialize by DataContractSerializer
    /// </summary>
    public class DefaultXmlSerializer : IXmlSerializer
    {
        public string Serialize(object o)
        {
            var serializer = new DataContractSerializer(o.GetType());
            using (var stream = new MemoryStream())
            {
                serializer.WriteObject(stream, o);
                stream.Position = 0;
                using (var sr = new StreamReader(stream))
                {
                    var resultStr = sr.ReadToEnd();
                    return resultStr;
                }
            }
        }

        public object Deserialize(string value, Type type)
        {
            var serializer = new DataContractSerializer(type);
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(value.ToCharArray())))
            {
                var o = serializer.ReadObject(stream);
                return o;
            }
        }

        public T Deserialize<T>(string value) where T : class
        {
            var serializer = new DataContractSerializer(typeof(T));
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(value.ToCharArray())))
            {
                var o = (T)serializer.ReadObject(stream);
                return o;
            }
        }
    }
}
