using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using DotCommon.Json;
using DotCommon.Json.SystemTextJson.JsonConverters;
using DotCommon.Timing;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace DotCommon.Test.Json
{
    public class DotCommonDateTimeConverterTest
    {
        private readonly Mock<IClock> _mockClock;
        private readonly DotCommonJsonOptions _jsonOptions;

        public DotCommonDateTimeConverterTest()
        {
            _mockClock = new Mock<IClock>();
            _mockClock.Setup(x => x.Normalize(It.IsAny<DateTime>()))
                .Returns((DateTime dt) => DateTime.SpecifyKind(dt, DateTimeKind.Utc));
            _jsonOptions = new DotCommonJsonOptions();
        }

        private DotCommonDateTimeConverter CreateConverter()
        {
            return new DotCommonDateTimeConverter(_mockClock.Object, Options.Create(_jsonOptions));
        }

        [Fact]
        public void Read_WithIsoDateTime_ShouldParseCorrectly()
        {
            var converter = CreateConverter();
            var json = "\"2024-01-15T10:30:00Z\"";
            var result = JsonSerializer.Deserialize<DateTime>(json, CreateOptions(converter));
            Assert.Equal(2024, result.Year);
        }

        [Fact]
        public void Read_WithInputFormats_ShouldParseCorrectly()
        {
            _jsonOptions.InputDateTimeFormats = new List<string> { "yyyy-MM-dd", "dd/MM/yyyy" };
            var converter = CreateConverter();
            var json = "\"2024-01-15\"";
            var result = JsonSerializer.Deserialize<DateTime>(json, CreateOptions(converter));
            Assert.Equal(2024, result.Year);
            Assert.Equal(1, result.Month);
            Assert.Equal(15, result.Day);
        }

        [Fact]
        public void Write_ShouldWriteDateTime()
        {
            var converter = CreateConverter();
            var date = new DateTime(2024, 1, 15, 10, 30, 0, DateTimeKind.Utc);
            var json = JsonSerializer.Serialize(date, CreateOptions(converter));
            Assert.Contains("2024-01-15", json);
        }

        [Fact]
        public void Write_WithOutputFormat_ShouldFormatCorrectly()
        {
            _jsonOptions.OutputDateTimeFormat = "yyyy/MM/dd";
            var converter = CreateConverter();
            var date = new DateTime(2024, 1, 15, 10, 30, 0, DateTimeKind.Utc);
            var json = JsonSerializer.Serialize(date, CreateOptions(converter));
            Assert.Contains("2024", json);
            Assert.Contains("01", json);
            Assert.Contains("15", json);
        }

        [Fact]
        public void SkipDateTimeNormalization_ShouldSkipNormalize()
        {
            var converter = CreateConverter().SkipDateTimeNormalization();
            var date = new DateTime(2024, 1, 15, 10, 30, 0, DateTimeKind.Local);
            var json = JsonSerializer.Serialize(date, CreateOptions(converter));
            _mockClock.Verify(x => x.Normalize(It.IsAny<DateTime>()), Times.Never);
        }

        private JsonSerializerOptions CreateOptions(DotCommonDateTimeConverter converter)
        {
            var options = new JsonSerializerOptions();
            options.Converters.Add(converter);
            return options;
        }
    }

    public class DotCommonNullableDateTimeConverterTest
    {
        private readonly Mock<IClock> _mockClock;
        private readonly DotCommonJsonOptions _jsonOptions;

        public DotCommonNullableDateTimeConverterTest()
        {
            _mockClock = new Mock<IClock>();
            _mockClock.Setup(x => x.Normalize(It.IsAny<DateTime>()))
                .Returns((DateTime dt) => DateTime.SpecifyKind(dt, DateTimeKind.Utc));
            _jsonOptions = new DotCommonJsonOptions();
        }

        private DotCommonNullableDateTimeConverter CreateConverter()
        {
            return new DotCommonNullableDateTimeConverter(_mockClock.Object, Options.Create(_jsonOptions));
        }

        [Fact]
        public void Read_WithIsoDateTime_ShouldParseCorrectly()
        {
            var converter = CreateConverter();
            var json = "\"2024-01-15T10:30:00Z\"";
            var result = JsonSerializer.Deserialize<DateTime?>(json, CreateOptions(converter));
            Assert.True(result.HasValue);
            Assert.Equal(2024, result.Value.Year);
        }

        [Fact]
        public void Read_WithInputFormats_ShouldParseCorrectly()
        {
            _jsonOptions.InputDateTimeFormats = new List<string> { "yyyy-MM-dd" };
            var converter = CreateConverter();
            var json = "\"2024-01-15\"";
            var result = JsonSerializer.Deserialize<DateTime?>(json, CreateOptions(converter));
            Assert.True(result.HasValue);
            Assert.Equal(2024, result.Value.Year);
        }

        [Fact]
        public void Write_WithValue_ShouldWriteDateTime()
        {
            var converter = CreateConverter();
            var date = new DateTime(2024, 1, 15, 10, 30, 0, DateTimeKind.Utc);
            var json = JsonSerializer.Serialize<DateTime?>(date, CreateOptions(converter));
            Assert.Contains("2024-01-15", json);
        }

        [Fact]
        public void Write_WithNull_ShouldWriteNull()
        {
            var converter = CreateConverter();
            var json = JsonSerializer.Serialize<DateTime?>(null, CreateOptions(converter));
            Assert.Equal("null", json);
        }

        [Fact]
        public void Write_WithOutputFormat_ShouldFormatCorrectly()
        {
            _jsonOptions.OutputDateTimeFormat = "yyyy/MM/dd";
            var converter = CreateConverter();
            var date = new DateTime(2024, 1, 15, 10, 30, 0, DateTimeKind.Utc);
            var json = JsonSerializer.Serialize<DateTime?>(date, CreateOptions(converter));
            Assert.Contains("2024", json);
            Assert.Contains("01", json);
            Assert.Contains("15", json);
        }

        [Fact]
        public void SkipDateTimeNormalization_ShouldSkipNormalize()
        {
            var converter = CreateConverter().SkipDateTimeNormalization();
            var date = new DateTime(2024, 1, 15, 10, 30, 0, DateTimeKind.Local);
            JsonSerializer.Serialize<DateTime?>(date, CreateOptions(converter));
            _mockClock.Verify(x => x.Normalize(It.IsAny<DateTime>()), Times.Never);
        }

        private JsonSerializerOptions CreateOptions(DotCommonNullableDateTimeConverter converter)
        {
            var options = new JsonSerializerOptions();
            options.Converters.Add(converter);
            return options;
        }
    }
}