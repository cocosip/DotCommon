using System;

namespace DotCommon.Json
{
    /// <summary>
    /// Optional byte-oriented JSON serializer. When an <see cref="IJsonSerializer"/> also implements
    /// this interface, callers can avoid the UTF-16 string round-trip on serialization/deserialization.
    /// </summary>
    public interface IUtf8JsonSerializer
    {
        byte[] SerializeToUtf8Bytes(object obj, bool camelCase = true, bool indented = false);

        object Deserialize(Type type, byte[] utf8Json, bool camelCase = true);
    }
}
