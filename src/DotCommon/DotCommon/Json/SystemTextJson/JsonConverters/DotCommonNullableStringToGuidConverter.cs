using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DotCommon.Json.SystemTextJson.JsonConverters
{
    public class DotCommonNullableStringToGuidConverter : JsonConverter<Guid?>
    {
        private static readonly string[] Formats = { "N", "D", "B", "P", "X" };

        public override Guid? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                var guidString = reader.GetString();
                foreach (var format in Formats)
                {
                    if (Guid.TryParseExact(guidString, format, out var guid))
                    {
                        return guid;
                    }
                }
            }

            if (reader.TryGetGuid(out var guid2))
            {
                return guid2;
            }

            return null;
        }

        public override void Write(Utf8JsonWriter writer, Guid? value, JsonSerializerOptions options)
        {
            if (value == null)
            {
                writer.WriteNullValue();
            }
            else
            {
                writer.WriteStringValue(value.Value);
            }
        }
    }
}
