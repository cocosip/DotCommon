using System;
using System.Text.Json;
using DotCommon.Serialization;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DotCommon.Test.Serialization
{
    public class DefaultObjectSerializerTest
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly DefaultObjectSerializer _serializer;

        public DefaultObjectSerializerTest()
        {
            var services = new ServiceCollection();
            _serviceProvider = services.BuildServiceProvider();
            _serializer = new DefaultObjectSerializer(_serviceProvider);
        }

        [Fact]
        public void Serialize_WithNull_ShouldReturnNull()
        {
            var result = _serializer.Serialize<string>(null);
            Assert.Null(result);
        }

        [Fact]
        public void Serialize_WithValue_ShouldReturnBytes()
        {
            var obj = new TestClass { Name = "Test", Value = 42 };
            var result = _serializer.Serialize(obj);

            Assert.NotNull(result);
            Assert.True(result.Length > 0);
        }

        [Fact]
        public void Deserialize_WithNull_ShouldReturnDefault()
        {
            var result = _serializer.Deserialize<TestClass>(null);
            Assert.Null(result);
        }

        [Fact]
        public void Deserialize_WithBytes_ShouldReturnObject()
        {
            var original = new TestClass { Name = "Test", Value = 42 };
            var bytes = JsonSerializer.SerializeToUtf8Bytes(original);

            var result = _serializer.Deserialize<TestClass>(bytes);

            Assert.NotNull(result);
            Assert.Equal("Test", result.Name);
            Assert.Equal(42, result.Value);
        }

        [Fact]
        public void SerializeDeserialize_RoundTrip_ShouldPreserveData()
        {
            var original = new TestClass { Name = "Hello World", Value = 123 };

            var bytes = _serializer.Serialize(original);
            var result = _serializer.Deserialize<TestClass>(bytes);

            Assert.NotNull(result);
            Assert.Equal(original.Name, result.Name);
            Assert.Equal(original.Value, result.Value);
        }

        [Fact]
        public void Serialize_WithPrimitiveType_ShouldWork()
        {
            var bytes = _serializer.Serialize(42);
            var result = _serializer.Deserialize<int>(bytes);

            Assert.Equal(42, result);
        }

        [Fact]
        public void Serialize_WithString_ShouldWork()
        {
            var bytes = _serializer.Serialize("Hello");
            var result = _serializer.Deserialize<string>(bytes);

            Assert.Equal("Hello", result);
        }

        [Fact]
        public void Serialize_WithList_ShouldWork()
        {
            var list = new System.Collections.Generic.List<int> { 1, 2, 3, 4, 5 };
            var bytes = _serializer.Serialize(list);
            var result = _serializer.Deserialize<System.Collections.Generic.List<int>>(bytes);

            Assert.Equal(new[] { 1, 2, 3, 4, 5 }, result);
        }

        [Fact]
        public void Serialize_WithSpecificSerializer_ShouldUseSpecificSerializer()
        {
            var services = new ServiceCollection();
            services.AddSingleton<IObjectSerializer<TestClass>, TestClassSerializer>();
            var serviceProvider = services.BuildServiceProvider();
            var serializer = new DefaultObjectSerializer(serviceProvider);

            var obj = new TestClass { Name = "Test", Value = 42 };
            var bytes = serializer.Serialize(obj);

            Assert.NotNull(bytes);
            var result = serializer.Deserialize<TestClass>(bytes);
            Assert.Equal("CustomSerialized", result.Name);
        }
    }

    public class TestClass
    {
        public string Name { get; set; } = "";
        public int Value { get; set; }
    }

    public class TestClassSerializer : IObjectSerializer<TestClass>
    {
        public byte[] Serialize(TestClass obj)
        {
            return JsonSerializer.SerializeToUtf8Bytes(new TestClass { Name = "CustomSerialized", Value = obj.Value });
        }

        public TestClass Deserialize(byte[] bytes)
        {
            return JsonSerializer.Deserialize<TestClass>(bytes)!;
        }
    }
}