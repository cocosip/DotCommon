using System;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using DotCommon.Timing;
using Microsoft.Extensions.Options;

namespace DotCommon.Json.SystemTextJson.JsonConverters
{
    /// <summary>
    /// A JSON converter for nullable DateTime values that handles custom date time formats and normalization
    /// </summary>
    public class DotCommonNullableDateTimeConverter : JsonConverter<DateTime?>
    {
        private readonly IClock _clock;
        private readonly DotCommonJsonOptions _options;
        private bool _skipDateTimeNormalization;

        /// <summary>
        /// Initializes a new instance of the DotCommonNullableDateTimeConverter class
        /// </summary>
        /// <param name="clock">Clock service used for date time normalization</param>
        /// <param name="dotCommonJsonOptions">JSON options containing input and output date time formats</param>
        public DotCommonNullableDateTimeConverter(IClock clock, IOptions<DotCommonJsonOptions> dotCommonJsonOptions)
        {
            _clock = clock;
            _options = dotCommonJsonOptions.Value;
        }

        /// <summary>
        /// Skips date time normalization for this converter instance
        /// </summary>
        /// <returns>The current converter instance with normalization disabled</returns>
        public virtual DotCommonNullableDateTimeConverter SkipDateTimeNormalization()
        {
            _skipDateTimeNormalization = true;
            return this;
        }

        /// <summary>
        /// Reads a nullable DateTime value from JSON
        /// </summary>
        /// <param name="reader">The JSON reader</param>
        /// <param name="typeToConvert">The type to convert to</param>
        /// <param name="options">JSON serializer options</param>
        /// <returns>A nullable DateTime value parsed from the JSON</returns>
        public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // Try to parse using custom input formats if any are specified
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

            // Try to get DateTime using default parsing
            if (reader.TryGetDateTime(out var d2))
            {
                return Normalize(d2);
            }

            // Try to parse using default DateTime parsing with current culture
            var dateText = reader.GetString();
            if (!dateText.IsNullOrWhiteSpace())
            {
                if (DateTime.TryParse(dateText, CultureInfo.CurrentUICulture, DateTimeStyles.None, out var d3))
                {
                    return Normalize(d3);
                }
            }

            return null;
        }

        /// <summary>
        /// Writes a nullable DateTime value to JSON
        /// </summary>
        /// <param name="writer">The JSON writer</param>
        /// <param name="value">The nullable DateTime value to write</param>
        /// <param name="options">JSON serializer options</param>
        public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
        {
            if (value == null)
            {
                writer.WriteNullValue();
            }
            else
            {
                if (_options.OutputDateTimeFormat.IsNullOrWhiteSpace())
                {
                    writer.WriteStringValue(Normalize(value.Value));
                }
                else
                {
                    writer.WriteStringValue(Normalize(value.Value).ToString(_options.OutputDateTimeFormat, CultureInfo.CurrentUICulture));
                }
            }
        }

        /// <summary>
        /// Normalizes a DateTime value using the clock service if normalization is not skipped
        /// </summary>
        /// <param name="dateTime">The DateTime value to normalize</param>
        /// <returns>The normalized DateTime value or the original value if normalization is skipped</returns>
        protected virtual DateTime Normalize(DateTime dateTime)
        {
            return _skipDateTimeNormalization ? dateTime : _clock.Normalize(dateTime);
        }
    }
}
