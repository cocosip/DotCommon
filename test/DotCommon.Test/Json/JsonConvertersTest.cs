using System;
using System.Collections.Generic;
using System.Text.Json;
using DotCommon.Json.SystemTextJson.JsonConverters;
using Xunit;

namespace DotCommon.Test.Json
{
    public class DotCommonStringToBooleanConverterTest
    {
        private readonly JsonSerializerOptions _options;

        public DotCommonStringToBooleanConverterTest()
        {
            _options = new JsonSerializerOptions();
            _options.Converters.Add(new DotCommonStringToBooleanConverter());
        }

        [Theory]
        [InlineData("\"true\"", true)]
        [InlineData("\"false\"", false)]
        [InlineData("\"True\"", true)]
        [InlineData("\"False\"", false)]
        [InlineData("true", true)]
        [InlineData("false", false)]
        public void Read_ShouldConvertToBoolean(string json, bool expected)
        {
            var result = JsonSerializer.Deserialize<bool>(json, _options);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Write_ShouldWriteBooleanValue()
        {
            var value = true;
            var json = JsonSerializer.Serialize(value, _options);
            Assert.Equal("true", json);
        }

        [Fact]
        public void Write_ShouldWriteFalseValue()
        {
            var value = false;
            var json = JsonSerializer.Serialize(value, _options);
            Assert.Equal("false", json);
        }

        [Fact]
        public void Read_WithInvalidString_ShouldFallbackToGetBoolean()
        {
            var json = "\"invalid\"";
            Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<bool>(json, _options));
        }
    }

    public class DotCommonStringToGuidConverterTest
    {
        private readonly JsonSerializerOptions _options;

        public DotCommonStringToGuidConverterTest()
        {
            _options = new JsonSerializerOptions();
            _options.Converters.Add(new DotCommonStringToGuidConverter());
        }

        [Fact]
        public void Read_WithDFormat_ShouldConvert()
        {
            var guid = Guid.NewGuid();
            var json = $"\"{guid:D}\"";
            var result = JsonSerializer.Deserialize<Guid>(json, _options);
            Assert.Equal(guid, result);
        }

        [Fact]
        public void Read_WithNFormat_ShouldConvert()
        {
            var guid = Guid.NewGuid();
            var json = $"\"{guid:N}\"";
            var result = JsonSerializer.Deserialize<Guid>(json, _options);
            Assert.Equal(guid, result);
        }

        [Fact]
        public void Read_WithBFormat_ShouldConvert()
        {
            var guid = Guid.NewGuid();
            var json = $"\"{guid:B}\"";
            var result = JsonSerializer.Deserialize<Guid>(json, _options);
            Assert.Equal(guid, result);
        }

        [Fact]
        public void Read_WithPFormat_ShouldConvert()
        {
            var guid = Guid.NewGuid();
            var json = $"\"{guid:P}\"";
            var result = JsonSerializer.Deserialize<Guid>(json, _options);
            Assert.Equal(guid, result);
        }

        [Fact]
        public void Read_WithGuidJsonFormat_ShouldConvert()
        {
            var guid = Guid.NewGuid();
            var json = JsonSerializer.Serialize(guid);
            var result = JsonSerializer.Deserialize<Guid>(json, _options);
            Assert.Equal(guid, result);
        }

        [Fact]
        public void Write_ShouldWriteGuidValue()
        {
            var guid = Guid.NewGuid();
            var json = JsonSerializer.Serialize(guid, _options);
            Assert.Contains(guid.ToString(), json);
        }
    }

    public class DotCommonStringToEnumConverterTest
    {
        private readonly JsonSerializerOptions _options;

        public DotCommonStringToEnumConverterTest()
        {
            _options = new JsonSerializerOptions();
            _options.Converters.Add(new DotCommonStringToEnumConverter<TestEnum>());
        }

        [Fact]
        public void Read_WithStringValue_ShouldConvert()
        {
            var json = "\"Value1\"";
            var result = JsonSerializer.Deserialize<TestEnum>(json, _options);
            Assert.Equal(TestEnum.Value1, result);
        }

        [Fact]
        public void Read_WithIntegerValue_ShouldConvert()
        {
            var json = "1";
            var result = JsonSerializer.Deserialize<TestEnum>(json, _options);
            Assert.Equal(TestEnum.Value1, result);
        }

        [Fact]
        public void Write_ShouldWriteEnumValue()
        {
            var value = TestEnum.Value2;
            var json = JsonSerializer.Serialize(value, _options);
            Assert.Equal("2", json);
        }

        [Fact]
        public void ReadAsPropertyName_ShouldConvert()
        {
            var json = "{\"Value1\":\"test\"}";
            var result = JsonSerializer.Deserialize<Dictionary<TestEnum, string>>(json, _options);
            Assert.True(result.ContainsKey(TestEnum.Value1));
        }
    }

    public class DotCommonStringToEnumFactoryTest
    {
        [Fact]
        public void CreateConverter_ShouldReturnConverter()
        {
            var factory = new DotCommonStringToEnumFactory();
            var converter = factory.CreateConverter(typeof(TestEnum), new JsonSerializerOptions());
            Assert.NotNull(converter);
        }

        [Fact]
        public void CanConvert_WithEnumType_ShouldReturnTrue()
        {
            var factory = new DotCommonStringToEnumFactory();
            Assert.True(factory.CanConvert(typeof(TestEnum)));
        }

        [Fact]
        public void CanConvert_WithNonEnumType_ShouldReturnFalse()
        {
            var factory = new DotCommonStringToEnumFactory();
            Assert.False(factory.CanConvert(typeof(string)));
        }
    }

    public enum TestEnum
    {
        Default = 0,
        Value1 = 1,
        Value2 = 2
    }

    public class DotCommonNullableStringToGuidConverterTest
    {
        private readonly JsonSerializerOptions _options;

        public DotCommonNullableStringToGuidConverterTest()
        {
            _options = new JsonSerializerOptions();
            _options.Converters.Add(new DotCommonNullableStringToGuidConverter());
        }

        [Fact]
        public void Read_WithValidGuid_ShouldReturnGuid()
        {
            var guid = Guid.NewGuid();
            var json = $"\"{guid:D}\"";
            var result = JsonSerializer.Deserialize<Guid?>(json, _options);
            Assert.Equal(guid, result);
        }

        [Fact]
        public void Read_WithNull_ShouldReturnNull()
        {
            var json = "null";
            var result = JsonSerializer.Deserialize<Guid?>(json, _options);
            Assert.Null(result);
        }

        [Fact]
        public void Write_WithValue_ShouldWriteGuid()
        {
            var guid = Guid.NewGuid();
            var json = JsonSerializer.Serialize<Guid?>(guid, _options);
            Assert.Contains(guid.ToString(), json);
        }

        [Fact]
        public void Write_WithNull_ShouldWriteNull()
        {
            var json = JsonSerializer.Serialize<Guid?>(null, _options);
            Assert.Equal("null", json);
        }
    }

    public class ObjectToInferredTypesConverterTest
    {
        private readonly JsonSerializerOptions _options;

        public ObjectToInferredTypesConverterTest()
        {
            _options = new JsonSerializerOptions();
            _options.Converters.Add(new ObjectToInferredTypesConverter());
        }

        [Fact]
        public void Read_WithTrue_ShouldReturnBooleanTrue()
        {
            var json = "true";
            var result = JsonSerializer.Deserialize<object>(json, _options);
            Assert.True((bool)result!);
        }

        [Fact]
        public void Read_WithFalse_ShouldReturnBooleanFalse()
        {
            var json = "false";
            var result = JsonSerializer.Deserialize<object>(json, _options);
            Assert.False((bool)result!);
        }

        [Fact]
        public void Read_WithInteger_ShouldReturnLong()
        {
            var json = "42";
            var result = JsonSerializer.Deserialize<object>(json, _options);
            Assert.Equal(42L, result);
        }

        [Fact]
        public void Read_WithDecimal_ShouldReturnDouble()
        {
            var json = "3.14";
            var result = JsonSerializer.Deserialize<object>(json, _options);
            Assert.Equal(3.14, result);
        }

        [Fact]
        public void Read_WithString_ShouldReturnString()
        {
            var json = "\"hello\"";
            var result = JsonSerializer.Deserialize<object>(json, _options);
            Assert.Equal("hello", result);
        }

        [Fact]
        public void Write_ShouldSerializeObject()
        {
            var obj = (object)42;
            var json = JsonSerializer.Serialize(obj, _options);
            Assert.Equal("42", json);
        }
    }
}