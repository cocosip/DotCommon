using System;
using System.Collections.Concurrent;
using System.Text.Json;
using Microsoft.Extensions.Options;

namespace DotCommon.Json.SystemTextJson
{
    public class DotCommonSystemTextJsonSerializer : IJsonSerializer, IUtf8JsonSerializer
    {
        protected DotCommonSystemTextJsonSerializerOptions Options { get; }

        public DotCommonSystemTextJsonSerializer(IOptions<DotCommonSystemTextJsonSerializerOptions> options)
        {
            Options = options.Value;
        }

        public string Serialize(object obj, bool camelCase = true, bool indented = false)
        {
            return JsonSerializer.Serialize(obj, CreateJsonSerializerOptions(camelCase, indented));
        }

        public T Deserialize<T>(string jsonString, bool camelCase = true)
        {
            return JsonSerializer.Deserialize<T>(jsonString, CreateJsonSerializerOptions(camelCase))!;
        }

        public object Deserialize(Type type, string jsonString, bool camelCase = true)
        {
            return JsonSerializer.Deserialize(jsonString, type, CreateJsonSerializerOptions(camelCase))!;
        }

        public byte[] SerializeToUtf8Bytes(object obj, bool camelCase = true, bool indented = false)
        {
            return JsonSerializer.SerializeToUtf8Bytes(obj, CreateJsonSerializerOptions(camelCase, indented));
        }

        public object Deserialize(Type type, byte[] utf8Json, bool camelCase = true)
        {
            return JsonSerializer.Deserialize(utf8Json, type, CreateJsonSerializerOptions(camelCase))!;
        }

        private static readonly ConcurrentDictionary<object, JsonSerializerOptions> JsonSerializerOptionsCache =
            new ConcurrentDictionary<object, JsonSerializerOptions>();

        protected virtual JsonSerializerOptions CreateJsonSerializerOptions(bool camelCase = true, bool indented = false)
        {
            return JsonSerializerOptionsCache.GetOrAdd(new
            {
                camelCase,
                indented,
                Options.JsonSerializerOptions
            }, _ => new JsonSerializerOptions(Options.JsonSerializerOptions)
            {
                PropertyNamingPolicy = camelCase ? JsonNamingPolicy.CamelCase : null,
                WriteIndented = indented
            });
        }
    }
}
