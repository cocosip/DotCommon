using System.Text;
using DotCommon.Json;

namespace DotCommon.Caching
{
    public class Utf8JsonDistributedCacheSerializer : IDistributedCacheSerializer
    {
        protected IJsonSerializer JsonSerializer { get; }

        public Utf8JsonDistributedCacheSerializer(IJsonSerializer jsonSerializer)
        {
            JsonSerializer = jsonSerializer;
        }

        public byte[] Serialize<T>(T obj)
        {
            return Encoding.UTF8.GetBytes(JsonSerializer.Serialize(obj!));
        }

        public T Deserialize<T>(byte[] bytes)
        {
            return (T)JsonSerializer.Deserialize(typeof(T), Encoding.UTF8.GetString(bytes));
        }
    }
}
