using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DotCommon.Json.SystemTextJson.JsonConverters
{
    public class DotCommonStringToEnumConverter<T> : JsonConverter<T>
      where T : struct, Enum
    {
        private readonly JsonStringEnumConverter _innerJsonStringEnumConverter;

        private JsonSerializerOptions? _readJsonSerializerOptions;

        private JsonSerializerOptions? _writeJsonSerializerOptions;

        public DotCommonStringToEnumConverter()
            : this(namingPolicy: null, allowIntegerValues: true)
        {

        }

        public DotCommonStringToEnumConverter(JsonNamingPolicy? namingPolicy = null, bool allowIntegerValues = true)
        {
            _innerJsonStringEnumConverter = new JsonStringEnumConverter(namingPolicy, allowIntegerValues);
        }

        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert.IsEnum;
        }

        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            _readJsonSerializerOptions ??= JsonSerializerOptionsHelper.Create(options, x =>
                    x == this ||
                    x.GetType() == typeof(DotCommonStringToEnumFactory),
                _innerJsonStringEnumConverter.CreateConverter(typeToConvert, options));

            return JsonSerializer.Deserialize<T>(ref reader, _readJsonSerializerOptions);
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            _writeJsonSerializerOptions ??= JsonSerializerOptionsHelper.Create(options, x =>
                    x == this ||
                    x.GetType() == typeof(DotCommonStringToEnumFactory));

            JsonSerializer.Serialize(writer, value, _writeJsonSerializerOptions);
        }

        public override T ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return (T)Enum.Parse(typeToConvert, reader.GetString()!);
        }

        public override void WriteAsPropertyName(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            writer.WritePropertyName(Enum.GetName(typeof(T), value)!);
        }
    }
}
