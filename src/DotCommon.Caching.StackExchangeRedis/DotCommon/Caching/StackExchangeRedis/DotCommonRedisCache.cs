using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace DotCommon.Caching.StackExchangeRedis
{
    public class DotCommonRedisCache : RedisCache, ICacheSupportsMultipleItems
    {
        protected static readonly string AbsoluteExpirationKey;
        protected static readonly string SlidingExpirationKey;
        protected static readonly string DataKey;
        protected static readonly long NotPresent;
        protected static readonly RedisValue[] HashMembersAbsoluteExpirationSlidingExpirationData;
        protected static readonly RedisValue[] HashMembersAbsoluteExpirationSlidingExpiration;

        protected static readonly FieldInfo RedisDatabaseField;
        protected static readonly MethodInfo ConnectMethod;
        protected static readonly MethodInfo ConnectAsyncMethod;
        protected static readonly MethodInfo MapMetadataMethod;
        protected static readonly MethodInfo GetAbsoluteExpirationMethod;
        protected static readonly MethodInfo GetExpirationInSecondsMethod;
        protected static readonly MethodInfo OnRedisErrorMethod;
        protected static readonly MethodInfo RecycleMethodInfo;

        protected RedisKey InstancePrefix { get; }

        static DotCommonRedisCache()
        {
            var type = typeof(RedisCache);

            RedisDatabaseField = GetRequiredField(type, "_cache", BindingFlags.Instance | BindingFlags.NonPublic);

            ConnectMethod = GetRequiredMethod(type, "Connect", BindingFlags.Instance | BindingFlags.NonPublic);

            ConnectAsyncMethod = GetRequiredMethod(type, "ConnectAsync", BindingFlags.Instance | BindingFlags.NonPublic);

            MapMetadataMethod = GetRequiredMethod(type, "MapMetadata", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static);

            GetAbsoluteExpirationMethod = GetRequiredMethod(type, "GetAbsoluteExpiration", BindingFlags.Static | BindingFlags.NonPublic);

            GetExpirationInSecondsMethod = GetRequiredMethod(type, "GetExpirationInSeconds", BindingFlags.Static | BindingFlags.NonPublic);

            OnRedisErrorMethod = GetRequiredMethod(type, "OnRedisError", BindingFlags.Instance | BindingFlags.NonPublic);

            RecycleMethodInfo = GetRequiredMethod(type, "Recycle", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static);

            AbsoluteExpirationKey = GetRequiredFieldValue(type, "AbsoluteExpirationKey", BindingFlags.Static | BindingFlags.NonPublic);

            SlidingExpirationKey = GetRequiredFieldValue(type, "SlidingExpirationKey", BindingFlags.Static | BindingFlags.NonPublic);

            DataKey = GetRequiredFieldValue(type, "DataKey", BindingFlags.Static | BindingFlags.NonPublic);

            NotPresent = (long)GetRequiredField(type, "NotPresent", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null)!;

            HashMembersAbsoluteExpirationSlidingExpirationData = new RedisValue[] { AbsoluteExpirationKey, SlidingExpirationKey, DataKey };

            HashMembersAbsoluteExpirationSlidingExpiration = new RedisValue[] { AbsoluteExpirationKey, SlidingExpirationKey };
        }

        public DotCommonRedisCache(IOptions<RedisCacheOptions> optionsAccessor)
            : base(optionsAccessor)
        {
            var instanceName = optionsAccessor.Value.InstanceName;
            if (!string.IsNullOrEmpty(instanceName))
            {
                InstancePrefix = (RedisKey)Encoding.UTF8.GetBytes(instanceName);
            }
        }

        protected virtual IDatabase Connect()
        {
            return (IDatabase)ConnectMethod.Invoke(this, Array.Empty<object>())!;
        }

        protected virtual async ValueTask<IDatabase> ConnectAsync(CancellationToken token = default)
        {
            return await (ValueTask<IDatabase>)ConnectAsyncMethod.Invoke(this, new object[] { token })!;
        }

        protected virtual void Recycle(byte[]? lease)
        {
            RecycleMethodInfo.Invoke(this, new object[] { lease! });
        }

        public virtual byte[]?[] GetMany(IEnumerable<string> keys)
        {
            if (keys == null)
            {
                throw new ArgumentNullException(nameof(keys));
            }

            return GetAndRefreshMany(keys, true);
        }

        public virtual async Task<byte[]?[]> GetManyAsync(IEnumerable<string> keys, CancellationToken token = default)
        {
            if (keys == null)
            {
                throw new ArgumentNullException(nameof(keys));
            }

            return await GetAndRefreshManyAsync(keys, true, token);
        }

        public virtual void SetMany(IEnumerable<KeyValuePair<string, byte[]>> items, DistributedCacheEntryOptions options)
        {
            var cache = Connect();

            try
            {
                Task.WhenAll(PipelineSetMany(cache, items, options, out var leases)).GetAwaiter().GetResult();
                foreach (var lease in leases)
                {
                    Recycle(lease);
                }
            }
            catch (Exception ex)
            {
                OnRedisError(ex, cache);
                throw;
            }
        }

        public virtual async Task SetManyAsync(IEnumerable<KeyValuePair<string, byte[]>> items, DistributedCacheEntryOptions options, CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();

            var cache = await ConnectAsync(token);

            try
            {
                await Task.WhenAll(PipelineSetMany(cache, items, options, out var leases));
                foreach (var lease in leases)
                {
                    Recycle(lease);
                }
            }
            catch (Exception ex)
            {
                OnRedisError(ex, cache);
                throw;
            }
        }

        public virtual void RefreshMany(IEnumerable<string> keys)
        {
            if (keys == null)
            {
                throw new ArgumentNullException(nameof(keys));
            }

            GetAndRefreshMany(keys, false);
        }

        public virtual async Task RefreshManyAsync(IEnumerable<string> keys, CancellationToken token = default)
        {
            if (keys == null)
            {
                throw new ArgumentNullException(nameof(keys));
            }

            await GetAndRefreshManyAsync(keys, false, token);
        }

        public virtual void RemoveMany(IEnumerable<string> keys)
        {
            if (keys == null)
            {
                throw new ArgumentNullException(nameof(keys));
            }

            var cache = Connect();

            try
            {
                Task.WhenAll(PipelineRemoveManyAsync(cache, keys)).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                OnRedisError(ex, cache);
                throw;
            }
        }

        public virtual async Task RemoveManyAsync(IEnumerable<string> keys, CancellationToken token = default)
        {
            if (keys == null)
            {
                throw new ArgumentNullException(nameof(keys));
            }

            token.ThrowIfCancellationRequested();
            var cache = await ConnectAsync(token);

            try
            {
                await Task.WhenAll(PipelineRemoveManyAsync(cache, keys));
            }
            catch (Exception ex)
            {
                OnRedisError(ex, cache);
                throw;
            }
        }

        protected virtual Task[] PipelineRemoveManyAsync(IDatabase cache, IEnumerable<string> keys)
        {
            return keys.Select(key => cache.KeyDeleteAsync(InstancePrefix.Append(key))).ToArray<Task>();
        }

        protected virtual byte[]?[] GetAndRefreshMany(IEnumerable<string> keys, bool getData)
        {
            var cache = Connect();

            var keyArray = keys.Select(key => InstancePrefix.Append(key)).ToArray();
            byte[]?[] bytes;

            try
            {
                var results = HashMemberGetMany(cache, keyArray, GetHashFields(getData));

                Task.WhenAll(PipelineRefreshManyAndOutData(cache, keyArray, results, out bytes)).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                OnRedisError(ex, cache);
                throw;
            }

            return bytes;
        }

        protected virtual async Task<byte[]?[]> GetAndRefreshManyAsync(IEnumerable<string> keys, bool getData, CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();

            var cache = await ConnectAsync(token);

            var keyArray = keys.Select(key => InstancePrefix.Append(key)).ToArray();
            byte[]?[] bytes;

            try
            {
                var results = await HashMemberGetManyAsync(cache, keyArray, GetHashFields(getData));
                await Task.WhenAll(PipelineRefreshManyAndOutData(cache, keyArray, results, out bytes));
            }
            catch (Exception ex)
            {
                OnRedisError(ex, cache);
                throw;
            }

            return bytes;
        }

        protected virtual RedisValue[][] HashMemberGetMany(IDatabase cache, RedisKey[] keys, RedisValue[] hashFields)
        {
            var results = new RedisValue[keys.Length][];
            var tasks = new List<Task<RedisValue[]>>();

            for (var i = 0; i < keys.Length; i++)
            {
                tasks.Add(cache.HashGetAsync(keys[i], hashFields));
            }

            Task.WhenAll(tasks).GetAwaiter().GetResult();

            for (var i = 0; i < keys.Length; i++)
            {
                results[i] = tasks[i].GetAwaiter().GetResult();
            }

            return results;
        }

        protected virtual async Task<RedisValue[][]> HashMemberGetManyAsync(IDatabase cache, RedisKey[] keys, RedisValue[] hashFields)
        {
            var results = new RedisValue[keys.Length][];
            var tasks = new List<Task<RedisValue[]>>();

            for (var i = 0; i < keys.Length; i++)
            {
                tasks.Add(cache.HashGetAsync(keys[i], hashFields));
            }

            await Task.WhenAll(tasks.ToArray());

            for (var i = 0; i < keys.Length; i++)
            {
                results[i] = await tasks[i];
            }

            return results;
        }

        protected virtual Task[] PipelineRefreshManyAndOutData(IDatabase cache, RedisKey[] keys, RedisValue[][] results, out byte[]?[] bytes)
        {
            bytes = new byte[keys.Length][];
            var tasks = new Task[keys.Length];

            for (var i = 0; i < keys.Length; i++)
            {
                if (results[i].Length >= 2)
                {
                    MapMetadata(results[i], out var absExpr, out var sldExpr);

                    if (sldExpr.HasValue)
                    {
                        TimeSpan? expr;

                        if (absExpr.HasValue)
                        {
                            var relExpr = absExpr.Value - DateTimeOffset.Now;
                            expr = relExpr <= sldExpr.Value ? relExpr : sldExpr;
                        }
                        else
                        {
                            expr = sldExpr;
                        }

                        tasks[i] = cache.KeyExpireAsync(keys[i], expr);
                    }
                    else
                    {
                        tasks[i] = Task.CompletedTask;
                    }
                }
                else
                {
                    tasks[i] = Task.CompletedTask;
                }

                if (results[i].Length >= 3 && results[i][2].HasValue)
                {
                    bytes[i] = results[i][2];
                }
                else
                {
                    bytes[i] = null;
                }
            }

            return tasks;
        }

        protected virtual Task[] PipelineSetMany(IDatabase cache, IEnumerable<KeyValuePair<string, byte[]>> items, DistributedCacheEntryOptions options, out List<byte[]?> leases)
        {
            var tasks = new List<Task>();
            leases = new List<byte[]?>();

            var creationTime = DateTimeOffset.UtcNow;

            var absoluteExpiration = GetAbsoluteExpiration(creationTime, options);

            foreach (var item in items)
            {
                var prefixedKey = InstancePrefix.Append(item.Key);
                var ttl = GetExpirationInSeconds(creationTime, absoluteExpiration, options);
                var fields = GetHashEntries(Linearize(new ReadOnlySequence<byte>(item.Value), out var lease), absoluteExpiration, options.SlidingExpiration);
                leases.Add(lease);
                if (ttl is null)
                {
                    tasks.Add(cache.HashSetAsync(prefixedKey, fields));
                }
                else
                {
                    tasks.Add(cache.HashSetAsync(prefixedKey, fields));
                    tasks.Add(cache.KeyExpireAsync(prefixedKey, TimeSpan.FromSeconds(ttl.GetValueOrDefault())));
                }
            }

            return tasks.ToArray();
        }

        protected virtual void MapMetadata(RedisValue[] results, out DateTimeOffset? absoluteExpiration, out TimeSpan? slidingExpiration)
        {
            var parameters = new object?[] { results, null, null };
            MapMetadataMethod.Invoke(this, parameters);

            absoluteExpiration = (DateTimeOffset?)parameters[1];
            slidingExpiration = (TimeSpan?)parameters[2];
        }

        protected virtual long? GetExpirationInSeconds(DateTimeOffset creationTime, DateTimeOffset? absoluteExpiration, DistributedCacheEntryOptions options)
        {
            return (long?)GetExpirationInSecondsMethod.Invoke(null, new object?[] { creationTime, absoluteExpiration, options });
        }

        protected virtual DateTimeOffset? GetAbsoluteExpiration(DateTimeOffset creationTime, DistributedCacheEntryOptions options)
        {
            return (DateTimeOffset?)GetAbsoluteExpirationMethod.Invoke(null, new object[] { creationTime, options });
        }

        protected virtual void OnRedisError(Exception ex, IDatabase cache)
        {
            OnRedisErrorMethod.Invoke(this, new object[] { ex, cache });
        }

        private static ReadOnlyMemory<byte> Linearize(in ReadOnlySequence<byte> value, out byte[]? lease)
        {
            if (value.IsSingleSegment)
            {
                lease = null;
                return value.First;
            }
            var length = checked((int)value.Length);
            lease = ArrayPool<byte>.Shared.Rent(length);
            value.CopyTo(lease);
            return new ReadOnlyMemory<byte>(lease, 0, length);
        }

        private static RedisValue[] GetHashFields(bool getData)
        {
            return getData
                ? HashMembersAbsoluteExpirationSlidingExpirationData
                : HashMembersAbsoluteExpirationSlidingExpiration;
        }

        private static HashEntry[] GetHashEntries(RedisValue value, DateTimeOffset? absoluteExpiration, TimeSpan? slidingExpiration)
        {
            return new HashEntry[]
            {
                new HashEntry(AbsoluteExpirationKey, absoluteExpiration?.Ticks ?? NotPresent),
                new HashEntry(SlidingExpirationKey, slidingExpiration?.Ticks ?? NotPresent),
                new HashEntry(DataKey, value)
            };
        }

        private static MethodInfo GetRequiredMethod(Type type, string name, BindingFlags flags)
        {
            return type.GetMethod(name, flags) ??
                   throw new MissingMethodException(type.FullName, name);
        }

        private static FieldInfo GetRequiredField(Type type, string name, BindingFlags flags)
        {
            return type.GetField(name, flags) ??
                   throw new MissingFieldException(type.FullName, name);
        }

        private static string GetRequiredFieldValue(Type type, string name, BindingFlags flags)
        {
            return GetRequiredField(type, name, flags).GetValue(null)?.ToString() ??
                   throw new InvalidOperationException($"Field '{name}' on '{type.FullName}' is null.");
        }
    }
}
