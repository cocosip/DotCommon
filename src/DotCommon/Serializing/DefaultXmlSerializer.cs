using System;
using System.IO;
using System.Runtime.Serialization;

namespace DotCommon.Serializing
{
    /// <summary>Serialize by DataContractSerializer
    /// </summary>
    public class DefaultXmlSerializer : IXmlSerializer
    {
        public string Serialize(object obj)
        {
            var serializer = new DataContractSerializer(obj.GetType());
            var stream = new MemoryStream();
            serializer.WriteObject(stream, obj);
            stream.Position = 0;
            var sr = new StreamReader(stream);
            var resultStr = sr.ReadToEnd();
            sr.Dispose();
            stream.Dispose();
            return resultStr;
        }

        public object Deserialize(string value, Type type)
        {
            var serializer = new DataContractSerializer(type);
            var ms = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(value.ToCharArray()));
            var obj = serializer.ReadObject(ms);
            ms.Dispose();
            return obj;
        }

        public T Deserialize<T>(string value) where T : class
        {
            var serializer = new DataContractSerializer(typeof(T));
            var ms = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(value.ToCharArray()));
            var obj = (T)serializer.ReadObject(ms);
            ms.Dispose();
            return obj;
        }
    }
}
