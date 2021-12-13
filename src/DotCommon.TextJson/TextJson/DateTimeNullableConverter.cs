using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DotCommon.TextJson
{
    /// <summary>
    /// Nullable dateTime converter
    /// </summary>
    public class DateTimeNullableConverter : JsonConverter<DateTime?>
    {
        /// <summary>
        /// Format
        /// </summary>
        /// <value></value>
        protected string Format { get; }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="format"></param>
        public DateTimeNullableConverter(string format)
        {
            Format = format;
        }

        /// <summary>
        /// Reade
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="typeToConvert"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TryGetDateTime(out DateTime dateTime))
            {
                return dateTime;
            }
            return null;
        }

        /// <summary>
        /// Write
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="options"></param>
        public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value?.ToString(Format) ?? "");
        }
    }
}