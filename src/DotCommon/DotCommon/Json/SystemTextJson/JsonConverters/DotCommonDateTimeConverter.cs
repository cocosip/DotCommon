using System;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using DotCommon.Timing;
using Microsoft.Extensions.Options;

namespace DotCommon.Json.SystemTextJson.JsonConverters
{
    public class DotCommonDateTimeConverter : JsonConverter<DateTime>
    {
        private readonly IClock _clock;
        private readonly DotCommonJsonOptions _options;
        private bool _skipDateTimeNormalization;

        public DotCommonDateTimeConverter(IClock clock, IOptions<DotCommonJsonOptions> abpJsonOptions)
        {
            _clock = clock;
            _options = abpJsonOptions.Value;
        }

        public virtual DotCommonDateTimeConverter SkipDateTimeNormalization()
        {
            _skipDateTimeNormalization = true;
            return this;
        }

        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (_options.InputDateTimeFormats.Any())
            {
                if (reader.TokenType == JsonTokenType.String)
                {
                    foreach (var format in _options.InputDateTimeFormats)
                    {
                        var s = reader.GetString();
                        if (DateTime.TryParseExact(s, format, CultureInfo.CurrentUICulture, DateTimeStyles.None, out var d1))
                        {
                            return Normalize(d1);
                        }
                    }
                }
                else
                {
                    throw new JsonException("Reader's TokenType is not String!");
                }
            }

            if (reader.TryGetDateTime(out var d3))
            {
                return Normalize(d3);
            }

            var dateText = reader.GetString();
            if (!dateText.IsNullOrWhiteSpace())
            {
                if (DateTime.TryParse(dateText, CultureInfo.CurrentUICulture, DateTimeStyles.None, out var d4))
                {
                    return Normalize(d4);
                }
            }

            throw new JsonException("Can't get datetime from the reader!");
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            if (_options.OutputDateTimeFormat.IsNullOrWhiteSpace())
            {
                writer.WriteStringValue(Normalize(value));
            }
            else
            {
                writer.WriteStringValue(Normalize(value).ToString(_options.OutputDateTimeFormat, CultureInfo.CurrentUICulture));
            }
        }

        protected virtual DateTime Normalize(DateTime dateTime)
        {
            return _skipDateTimeNormalization ? dateTime : _clock.Normalize(dateTime);
        }
    }
}
