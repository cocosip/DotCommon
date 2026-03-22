using System.Collections.Concurrent;
using System.Collections.Generic;
using Xunit;

namespace System.Collections.Generic.Tests
{
    public class DictionaryExtensionsTest
    {
        [Fact]
        public void GetOrDefault_WithExistingKey_ShouldReturnValue()
        {
            var dict = new Dictionary<string, int> { ["key"] = 42 };
            var result = dict.GetOrDefault("key");
            Assert.Equal(42, result);
        }

        [Fact]
        public void GetOrDefault_WithMissingKey_ShouldReturnDefault()
        {
            var dict = new Dictionary<string, int>();
            var result = dict.GetOrDefault("missing");
            Assert.Equal(0, result);
        }

        [Fact]
        public void GetOrDefault_IDictionary_WithExistingKey_ShouldReturnValue()
        {
            IDictionary<string, string> dict = new Dictionary<string, string> { ["key"] = "value" };
            var result = dict.GetOrDefault("key");
            Assert.Equal("value", result);
        }

        [Fact]
        public void GetOrDefault_IDictionary_WithMissingKey_ShouldReturnDefault()
        {
            IDictionary<string, string> dict = new Dictionary<string, string>();
            var result = dict.GetOrDefault("missing");
            Assert.Null(result);
        }

        [Fact]
        public void GetOrDefault_IReadOnlyDictionary_WithExistingKey_ShouldReturnValue()
        {
            IReadOnlyDictionary<string, int> dict = new Dictionary<string, int> { ["key"] = 42 };
            var result = dict.GetOrDefault("key");
            Assert.Equal(42, result);
        }

        [Fact]
        public void GetOrDefault_ConcurrentDictionary_WithExistingKey_ShouldReturnValue()
        {
            var dict = new ConcurrentDictionary<string, int>();
            dict["key"] = 42;
            var result = dict.GetOrDefault("key");
            Assert.Equal(42, result);
        }

        [Fact]
        public void GetOrAdd_WithExistingKey_ShouldReturnExisting()
        {
            var dict = new Dictionary<string, int> { ["key"] = 42 };
            var result = dict.GetOrAdd("key", k => 100);
            Assert.Equal(42, result);
        }

        [Fact]
        public void GetOrAdd_WithMissingKey_ShouldAddAndReturn()
        {
            var dict = new Dictionary<string, int>();
            var result = dict.GetOrAdd("key", k => 100);
            Assert.Equal(100, result);
            Assert.Equal(100, dict["key"]);
        }

        [Fact]
        public void GetOrAdd_WithFactoryFunc_ShouldWork()
        {
            var dict = new Dictionary<string, int>();
            var result = dict.GetOrAdd("key", () => 42);
            Assert.Equal(42, result);
        }

        [Fact]
        public void GetOrAdd_ConcurrentDictionary_WithFactoryFunc_ShouldWork()
        {
            var dict = new ConcurrentDictionary<string, int>();
            var result = dict.GetOrAdd("key", () => 42);
            Assert.Equal(42, result);
        }

        [Fact]
        public void ConvertToDynamicObject_ShouldCreateDynamic()
        {
            var dict = new Dictionary<string, object> { ["Name"] = "Test", ["Value"] = 42 };
            dynamic obj = dict.ConvertToDynamicObject();

            Assert.Equal("Test", obj.Name);
            Assert.Equal(42, obj.Value);
        }
    }

    public class ConcurrentDictionaryExtensionsTest
    {
        [Fact]
        public void Remove_WithExistingKey_ShouldRemove()
        {
            var dict = new ConcurrentDictionary<string, int>();
            dict["key"] = 42;

            var result = dict.Remove("key");

            Assert.True(result);
            Assert.False(dict.ContainsKey("key"));
        }

        [Fact]
        public void Remove_WithMissingKey_ShouldReturnFalse()
        {
            var dict = new ConcurrentDictionary<string, int>();
            var result = dict.Remove("missing");
            Assert.False(result);
        }
    }
}