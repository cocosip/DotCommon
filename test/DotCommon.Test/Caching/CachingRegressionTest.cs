using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DotCommon;
using DotCommon.Caching;
using DotCommon.Caching.Hybrid;
using DotCommon.Threading;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace DotCommon.Test.Caching
{
    public class CachingRegressionTest
    {
        [Fact]
        public void GetOrAddMany_FactoryReturnsOutOfOrder_MapsByKey()
        {
            // Arrange
            var cache = CreateCache();
            var keys = new[] { "a", "b", "c" };

            // Act
            var result = cache.GetOrAddMany(
                keys,
                _ =>
                [
                    new KeyValuePair<string, TestCacheItem>("c", new TestCacheItem("value-c")),
                    new KeyValuePair<string, TestCacheItem>("b", new TestCacheItem("value-b")),
                    new KeyValuePair<string, TestCacheItem>("a", new TestCacheItem("value-a"))
                ]);

            // Assert
            Assert.Equal(3, result.Length);
            Assert.Equal("value-a", result.Single(x => x.Key == "a").Value?.Value);
            Assert.Equal("value-b", result.Single(x => x.Key == "b").Value?.Value);
            Assert.Equal("value-c", result.Single(x => x.Key == "c").Value?.Value);
        }

        [Fact]
        public void GetOrAddMany_FactoryMissesKey_ThrowsDotCommonException()
        {
            // Arrange
            var cache = CreateCache();

            // Act & Assert
            Assert.Throws<DotCommonException>(() =>
                cache.GetOrAddMany(
                    new[] { "a", "b" },
                    _ => [new KeyValuePair<string, TestCacheItem>("a", new TestCacheItem("value-a"))]));
        }

        [Fact]
        public void AddDotCommonCaching_RegistersGenericHybridCacheService()
        {
            // Arrange
            IServiceCollection services = new ServiceCollection();

            // Act
            services.AddDotCommonCaching();

            // Assert
            Assert.Contains(services,
                x => x.ServiceType == typeof(IHybridCache<,>) && x.ImplementationType == typeof(DotCommonHybridCache<,>));
        }

        private static DistributedCache<TestCacheItem, string> CreateCache()
        {
            var options = Options.Create(new DotCommonDistributedCacheOptions
            {
                HideErrors = false,
                KeyPrefix = ""
            });

            var serializer = new TestDistributedCacheSerializer();
            var normalizer = new DistributedCacheKeyNormalizer(options);
            var serviceScopeFactory = new Mock<IServiceScopeFactory>().Object;

            return new DistributedCache<TestCacheItem, string>(
                options,
                new InMemoryMultiItemCache(),
                NullCancellationTokenProvider.Instance,
                serializer,
                normalizer,
                serviceScopeFactory);
        }

        private sealed class TestCacheItem
        {
            public string Value { get; set; }

            public TestCacheItem(string value)
            {
                Value = value;
            }
        }

        private sealed class InMemoryMultiItemCache : IDistributedCache, ICacheSupportsMultipleItems
        {
            private readonly Dictionary<string, byte[]> _store = new Dictionary<string, byte[]>();

            public byte[] Get(string key)
            {
                return _store.TryGetValue(key, out var value) ? value : null;
            }

            public Task<byte[]> GetAsync(string key, CancellationToken token = default)
            {
                return Task.FromResult(Get(key));
            }

            public void Set(string key, byte[] value, DistributedCacheEntryOptions options)
            {
                _store[key] = value;
            }

            public Task SetAsync(string key, byte[] value, DistributedCacheEntryOptions options,
                CancellationToken token = default)
            {
                Set(key, value, options);
                return Task.CompletedTask;
            }

            public void Refresh(string key)
            {
            }

            public Task RefreshAsync(string key, CancellationToken token = default)
            {
                return Task.CompletedTask;
            }

            public void Remove(string key)
            {
                _store.Remove(key);
            }

            public Task RemoveAsync(string key, CancellationToken token = default)
            {
                Remove(key);
                return Task.CompletedTask;
            }

            public byte[][] GetMany(IEnumerable<string> keys)
            {
                return keys.Select(Get).ToArray();
            }

            public Task<byte[][]> GetManyAsync(IEnumerable<string> keys, CancellationToken token = default)
            {
                return Task.FromResult(GetMany(keys));
            }

            public void SetMany(IEnumerable<KeyValuePair<string, byte[]>> items, DistributedCacheEntryOptions options)
            {
                foreach (var item in items)
                {
                    _store[item.Key] = item.Value;
                }
            }

            public Task SetManyAsync(IEnumerable<KeyValuePair<string, byte[]>> items, DistributedCacheEntryOptions options,
                CancellationToken token = default)
            {
                SetMany(items, options);
                return Task.CompletedTask;
            }

            public void RefreshMany(IEnumerable<string> keys)
            {
            }

            public Task RefreshManyAsync(IEnumerable<string> keys, CancellationToken token = default)
            {
                return Task.CompletedTask;
            }

            public void RemoveMany(IEnumerable<string> keys)
            {
                foreach (var key in keys)
                {
                    _store.Remove(key);
                }
            }

            public Task RemoveManyAsync(IEnumerable<string> keys, CancellationToken token = default)
            {
                RemoveMany(keys);
                return Task.CompletedTask;
            }
        }

        private sealed class TestDistributedCacheSerializer : IDistributedCacheSerializer
        {
            public byte[] Serialize<T>(T obj)
            {
                if (obj is TestCacheItem item)
                {
                    return Encoding.UTF8.GetBytes(item.Value);
                }

                throw new InvalidOperationException($"Unexpected type: {typeof(T).FullName}");
            }

            public T Deserialize<T>(byte[] bytes)
            {
                if (typeof(T) == typeof(TestCacheItem))
                {
                    var item = new TestCacheItem(Encoding.UTF8.GetString(bytes));
                    return (T)(object)item;
                }

                throw new InvalidOperationException($"Unexpected type: {typeof(T).FullName}");
            }
        }
    }
}
