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
            // Use the byte-oriented path when available to skip the UTF-16 string round-trip.
            if (JsonSerializer is IUtf8JsonSerializer utf8JsonSerializer)
            {
                return utf8JsonSerializer.SerializeToUtf8Bytes(obj!);
            }

            return Encoding.UTF8.GetBytes(JsonSerializer.Serialize(obj!));
        }

        public T Deserialize<T>(byte[] bytes)
        {
            if (JsonSerializer is IUtf8JsonSerializer utf8JsonSerializer)
            {
                return (T)utf8JsonSerializer.Deserialize(typeof(T), bytes);
            }

            return (T)JsonSerializer.Deserialize(typeof(T), Encoding.UTF8.GetString(bytes));
        }
    }
}
